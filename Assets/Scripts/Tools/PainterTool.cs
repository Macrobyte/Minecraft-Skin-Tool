using System;
using UnityEngine;

public class PainterTool : MonoBehaviour
{
    [SerializeField] private ToggleButton toggleButton;

    private void Start()
    {
        toggleButton.onToggle += ToggleSkinOverlay;

        toggleButton.Toggle();
    }

    private void ToggleSkinOverlay(bool obj)
    {
        PlayerModelHandler.Instance.ToggleOverlay(obj);
    }
}
