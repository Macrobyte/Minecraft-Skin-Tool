using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolWindow : MonoBehaviour
{
    [Header("Window Name")]
    private TMP_Text windowNameText;

    [SerializeField]
    private string windowName;

    [Header("Window Behaviour")]
    [SerializeField]
    private Button minimizeButton;

    private void Awake()
    {
        // Find first text component in child object
        if(!windowNameText)
            windowNameText = GetComponentInChildren<TMP_Text>();

        windowNameText.text = windowName;


        minimizeButton.onClick.AddListener(ToggleWindow);
    }


    public void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }



}
