using UnityEngine;

public class BackgroundSwapper : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private ColorPickerController colorPicker;
    [SerializeField] private Color backgroundColor = new Color(0.731f, 0.905f, 1f, 1f);

    private void Awake()
    {
        mainCamera = Object.FindAnyObjectByType<Camera>();

        mainCamera.backgroundColor = backgroundColor;

        colorPicker.onColorChanged += ChangeBackground;
    }

    private void Start()
    {
        
    }

    private void ChangeBackground(Color backgroundColor)
    {
        mainCamera.backgroundColor = backgroundColor;
    }
}
