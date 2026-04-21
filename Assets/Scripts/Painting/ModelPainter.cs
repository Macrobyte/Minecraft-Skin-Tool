using CustomAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelPainter : MonoBehaviour
{
    [Space(5)]
    [Category("Painting Settings", TextAnchor.MiddleCenter)]
    [Space(5)]

    [SerializeField] private Color32 currentColor;
    [SerializeField, ReadOnly] private bool isPainting;
    
    public enum PaintTool
    {
        Brush,
        Eraser,
        BucketTool,
        FaceFill
    }
    [SerializeField] private PaintTool selectedPaintTool;

    private Texture2D paintTexture;
    private CommandManager _commandManager;
    private List<PixelData> _currentStrokePositions;
    private ColorPickerController colorPickerControl;

    private Camera mainCamera;

    private void Awake()
    {
        colorPickerControl = FindFirstObjectByType<ColorPickerController>();

        colorPickerControl.onColorChanged += ChangeColor;

        _commandManager = new CommandManager(10000);

        mainCamera = Camera.main;
    }

    private void Start()
    {
        Texture2D originalTexture = PlayerModelHandler.Instance.GetDefaultSkin();

        CopyTexture(originalTexture);

        PlayerModelHandler.Instance.ApplyTexture(paintTexture);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _commandManager.Undo();
            PlayerModelHandler.Instance.ApplyTexture(paintTexture);
            Debug.Log("Undo");
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _commandManager.Redo();
            PlayerModelHandler.Instance.ApplyTexture(paintTexture);
            Debug.Log("Redo");
        }


        OnPointerDown();
        OnPointerUp();
        OnDrag();
    }

    private void CopyTexture(Texture2D originalTexture)
    {
        paintTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        paintTexture.filterMode = FilterMode.Point;
        paintTexture.SetPixels(originalTexture.GetPixels());
        paintTexture.Apply();
    }

    private void Paint(PaintTool tool, Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector2 uv = hit.textureCoord;
            
            int uvX = Mathf.Clamp(Mathf.FloorToInt(uv.x * paintTexture.width), 0, paintTexture.width - 1);
            int uvY = Mathf.Clamp(Mathf.FloorToInt(uv.y * paintTexture.height), 0, paintTexture.height - 1);
                
            Vector2Int pixelPosition = new Vector2Int(uvX, uvY);

            switch(tool)
            {
                case PaintTool.Brush:

                    if (_currentStrokePositions.Exists(p => p.Position == pixelPosition))
                    {
                        return;
                    }
                    _currentStrokePositions.Add(BrushPaint(pixelPosition));

                    break;
                case PaintTool.Eraser:

                    if (_currentStrokePositions.Exists(p => p.Position == pixelPosition))
                    {
                        return;
                    }
                    _currentStrokePositions.Add(EraserPaint(pixelPosition));

                    break;
                case PaintTool.BucketTool:
                    _currentStrokePositions.AddRange(BucketFill(pixelPosition));
                    break;
                case PaintTool.FaceFill:
                    break;

            }

            if (_currentStrokePositions != null && _currentStrokePositions.Count > 0)
            {
                paintTexture.Apply();
                PlayerModelHandler.Instance.ApplyTexture(paintTexture);
            }
        }
    }

    private PixelData BrushPaint(Vector2Int pixelPos)
    {
        Color originalColor = paintTexture.GetPixel(pixelPos.x, pixelPos.y);

        Color appliedColor = currentColor;

        paintTexture.SetPixel(pixelPos.x, pixelPos.y, appliedColor); 

        return new PixelData(pixelPos, originalColor, appliedColor);
    }

    private PixelData EraserPaint(Vector2Int pixelPos)
    {
        Color originalColor = paintTexture.GetPixel(pixelPos.x, pixelPos.y);

        Color appliedColor = Color.clear;

        paintTexture.SetPixel(pixelPos.x, pixelPos.y, appliedColor);

        return new PixelData(pixelPos, originalColor, appliedColor);
    }

    private List<PixelData>BucketFill(Vector2Int pixelPos)
    {
        Color targetColor = paintTexture.GetPixel(pixelPos.x, pixelPos.y);

        if (targetColor == currentColor)
        {
            return new List<PixelData>();
        }

        List<Vector2Int> filledPixels = new List<Vector2Int>();
        Queue<Vector2Int> pixels = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        pixels.Enqueue(pixelPos);

        while (pixels.Count > 0)
        {
            Vector2Int current = pixels.Dequeue();

            if (!visited.Add(current))
                continue;

            if (!IsValidPixel(current.x, current.y, targetColor))
                continue;

            paintTexture.SetPixel(current.x, current.y, currentColor);

            filledPixels.Add(current);

            // Add adjacent pixels
            pixels.Enqueue(new Vector2Int(current.x + 1, current.y));
            pixels.Enqueue(new Vector2Int(current.x - 1, current.y));
            pixels.Enqueue(new Vector2Int(current.x, current.y + 1));
            pixels.Enqueue(new Vector2Int(current.x, current.y - 1));
        }

        return filledPixels.Select(p => new PixelData(p, targetColor, currentColor)).ToList();
    }
    
    private bool IsValidPixel(int x, int y, Color targetColor)
    {
        if (x < 0 || x >= paintTexture.width || y < 0 || y >= paintTexture.height)
            return false;
    
        return (Color32)paintTexture.GetPixel(x, y) == targetColor;
    }

#region Input Methods
    public void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPainting = true;

            _currentStrokePositions = new List<PixelData>();

            Paint(selectedPaintTool, Input.mousePosition);
        }
        
        Debug.Log("Stroke Start");
    }

    public void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isPainting = false;


            if (_currentStrokePositions != null && _currentStrokePositions.Count > 0)
            {
                _commandManager.ExecuteCommand(new PaintCommand(paintTexture, _currentStrokePositions));
            }

            _currentStrokePositions = null;
        }

        Debug.Log("Stroke End");
    }

    public void OnDrag()
    {
        if (Input.GetMouseButton(0) && isPainting)
        {
            if(selectedPaintTool == PaintTool.BucketTool)
            {
                return;
            }

            Paint(selectedPaintTool, Input.mousePosition);
        }
    }
    #endregion

    private void ChangeColor(Color color)
    {
        currentColor = color;
    }
}


