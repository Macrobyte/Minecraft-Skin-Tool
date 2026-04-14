using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaintTester : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Color32 color;
    
    public PlayerModelHandler modelHandler;

    public bool isPainting;
    
    public Texture2D paintTexture;
    
    private CommandManager _commandManager;
    public List<PixelData> _currentStrokePositions;

    public enum PaintTool
    {
        Brush,
        Eraser,
        FloodFill,
        FullFill
    }
    
    public PaintTool selectedPaintTool;
    
    private void Awake()
    {
        modelHandler = FindFirstObjectByType<PlayerModelHandler>();

        _commandManager = new CommandManager(10000);
    }

    private void Start()
    {
        Texture2D originalTexture = modelHandler.GetDefaultSkin();

        CopyTexture(originalTexture);

        modelHandler.ApplyTexture(paintTexture);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    Texture2D originalTexture = modelHandler.GetDefaultSkin();

        //    CopyTexture(originalTexture);

        //    modelHandler.ApplyTexture(paintTexture);
        //}
            


        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _commandManager.Undo();
                modelHandler.ApplyTexture(paintTexture);
                Debug.Log("Undo");
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                _commandManager.Redo();
                modelHandler.ApplyTexture(paintTexture);
                Debug.Log("Redo");
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            isPainting = true;
            _currentStrokePositions = new List<PixelData>();
            Paint(selectedPaintTool, Input.mousePosition);
            Debug.Log("Stroke Start");
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPainting = false;

            if (_currentStrokePositions.Count > 0)
            {
                var strokeData = _currentStrokePositions
                    .Select(p => new PixelData(
                        p.Position,
                        p.OriginalColor,
                        selectedPaintTool == PaintTool.Eraser ? Color.clear : color))
                    .ToList();

                _commandManager.ExecuteCommand(new PaintCommand(paintTexture, strokeData));
                Debug.Log("Stroke End");
            }

            _currentStrokePositions = null;
        }

        if (Input.GetMouseButton(0) && isPainting)
        {
            Paint(selectedPaintTool, Input.mousePosition);
        }



    }

    private void CopyTexture(Texture2D originalTexture)
    {
        paintTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        paintTexture.filterMode = FilterMode.Point;
        paintTexture.SetPixels(originalTexture.GetPixels());
        paintTexture.Apply();
    }

    private void Paint(PaintTool tool,Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector2 uv = hit.textureCoord;
            
            int x = Mathf.Clamp(Mathf.FloorToInt(uv.x * paintTexture.width), 0, paintTexture.width - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt(uv.y * paintTexture.height), 0, paintTexture.height - 1);
                
            Vector2Int pixelPosition = new Vector2Int(x, y);

            if (tool == PaintTool.FloodFill)
            {
                Color targetColor = paintTexture.GetPixel(pixelPosition.x, pixelPosition.y);
                if (targetColor == color) return;

                List<Vector2Int> filledPixels = FloodFill(x, y, targetColor, color);
                foreach (var pos in filledPixels)
                {
                    paintTexture.SetPixel(pos.x, pos.y, targetColor);
                }
                paintTexture.Apply();
                modelHandler.ApplyTexture(paintTexture);
                
                // Save fill as a command for undo/redo
                List<PixelData> strokeData = filledPixels.Select(p => new PixelData(p, targetColor, color)).ToList();
                PaintCommand paintCommand = new PaintCommand(paintTexture, strokeData);
                _commandManager.ExecuteCommand(paintCommand);
            }
            else
            {
                if (!_currentStrokePositions.Exists(p => p.Position == pixelPosition))
                {
                    Color originalColor = paintTexture.GetPixel(x, y);

                    Color appliedColor = tool == PaintTool.Brush ? color  : Color.clear;
                
                    _currentStrokePositions.Add(new PixelData(pixelPosition, originalColor,appliedColor));
                    paintTexture.SetPixel(x, y, appliedColor);
                    paintTexture.Apply();
                    modelHandler.ApplyTexture(paintTexture);
                }
            }
            
        }
    }

    private List<Vector2Int> FloodFill(int startX, int startY, Color targetColor, Color32 fillColor)
    {
        List<Vector2Int> filledPixels = new List<Vector2Int>();
        Queue<Vector2Int> pixels = new Queue<Vector2Int>();
        pixels.Enqueue(new Vector2Int(startX, startY));

        while (pixels.Count > 0)
        {
            Vector2Int current = pixels.Dequeue();

            if (!IsValidPixel(current.x, current.y, targetColor)) 
                continue;

            paintTexture.SetPixel(current.x, current.y, fillColor);
            filledPixels.Add(current);

            // Add adjacent pixels
            pixels.Enqueue(new Vector2Int(current.x + 1, current.y));
            pixels.Enqueue(new Vector2Int(current.x - 1, current.y));
            pixels.Enqueue(new Vector2Int(current.x, current.y + 1));
            pixels.Enqueue(new Vector2Int(current.x, current.y - 1));
        }

        return filledPixels;
    }
    
    private bool IsValidPixel(int x, int y, Color targetColor)
    {
        if (x < 0 || x >= paintTexture.width || y < 0 || y >= paintTexture.height)
            return false;
    
        return paintTexture.GetPixel(x, y) == targetColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Middle)
            return;
        
        isPainting = true;
        _currentStrokePositions = new List<PixelData>();
        Paint(selectedPaintTool, eventData.position);
        Debug.Log("Stroke Start");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
            return;
        
        isPainting = false;

        List<PixelData> strokeData = new List<PixelData>();
        foreach (var pixel in _currentStrokePositions)
        {
            Color appliedColor = selectedPaintTool == PaintTool.Eraser ? Color.clear : color;
            strokeData.Add(new PixelData(pixel.Position, pixel.OriginalColor, appliedColor));
        }
        
        if (_currentStrokePositions.Count > 0)
        {
            
            
            PaintCommand paintCommand = new PaintCommand(paintTexture, strokeData);
            _commandManager.ExecuteCommand(paintCommand);
            
            Debug.Log("Stroke End");
        }

        _currentStrokePositions = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Middle)
            return;
        
        if (isPainting)
        {
            Paint(selectedPaintTool, eventData.position);
        }
    }
}


