using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
struct Tool
{
    public string name;
    public ToolWindow toolWindow;
    public Button toolbarButton;
}

public class ToolbarHandler : MonoBehaviour
{
    [SerializeField]
    private List<Tool> tools = new List<Tool>();
    
    
    private void Start()
    {
        foreach (Tool tool in tools)
        {
            tool.toolbarButton.onClick.AddListener(() => tool.toolWindow.ToggleWindow());
        }

        ToggleAllTools();


    }

    public void ToggleAllTools()
    {
        foreach (Tool tool in tools)
        {
            tool.toolWindow.ToggleWindow();
        }
    }
}

