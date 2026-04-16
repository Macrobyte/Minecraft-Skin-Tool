using System;
using UnityEngine;

public class PainterTool : MonoBehaviour
{
    [SerializeField] private ToggleButton toggleSkinOverlay;
    [SerializeField] private ToggleButton toggleGrid;


    private void Start()
    {
        toggleSkinOverlay.onToggle += ToggleSkinOverlay;

        toggleGrid.onToggle += ToggleGrid;

        toggleSkinOverlay.Toggle();
        toggleGrid.Toggle();
    }

    private void ToggleGrid(bool obj)
    {
        PlayerModelHandler.Instance.ToggleGrid(obj);
    }

    private void ToggleSkinOverlay(bool obj)
    {
        PlayerModelHandler.Instance.ToggleOverlay(obj);
    }
}
