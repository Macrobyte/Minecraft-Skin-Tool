using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public Button button;
    public bool isOn = false;

    float startPos;

    float endPos;

    public Action<bool> onToggle;


    public void Start()
    {
        button.onClick.AddListener(ButtonToggle);

        startPos = button.transform.localPosition.x;

        endPos = startPos * -1;
    }

    private void ButtonToggle()
    {
        // Toggle the button
        isOn = !isOn;

        // Call the onToggle event
        onToggle?.Invoke(isOn);

        // Move the button to the other side
        if (isOn)
        {
            button.transform.localPosition = new Vector3(endPos, 0, 0);
        }
        else
        {
            button.transform.localPosition = new Vector3(startPos, 0, 0);
        }
    }
}
