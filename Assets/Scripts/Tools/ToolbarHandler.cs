using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
struct Tool
{
    public string name ;
    public ToolWindow toolWindow;
    public Button toolbarButton;
}

public class ToolbarHandler : MonoBehaviour
{
    [Header("Tools")]
    [SerializeField]
    private List<Tool> tools = new List<Tool>();

    [Header("Menu")]
    [SerializeField] private Button menuButton;
    [SerializeField] private MainMenuWindow menuWindow;

    private void Awake()
    {
        foreach (Tool tool in tools)
        {
            tool.toolbarButton.onClick.AddListener(() => tool.toolWindow.ToggleWindow());
        }

        menuButton.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        menuWindow.ToggleWindow();
    }

    private void Start()
    {   
        ToggleAllTools();
        menuWindow.ToggleWindow();
    }

    public void ToggleAllTools()
    {
        foreach (Tool tool in tools)
        {
            if (!tool.toolWindow)
                return;

            tool.toolWindow.ToggleWindow();
        }
    }
}

