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

    public enum ToggleType { Slide, Checkbox }

    public ToggleType toggleType;


    public void Start()
    {
        startPos = button.transform.localPosition.x;

        endPos = startPos * -1;

        switch (toggleType)
        {
            case ToggleType.Slide:
                button.onClick.AddListener(ButtonToggleSlide);
                break;
            case ToggleType.Checkbox:
                button.onClick.AddListener(ButtonToggleCheckbox);
                return;
        }
    }

    public void Toggle()
    {
        switch(toggleType)
        {
            case ToggleType.Slide:
                ButtonToggleSlide();
                break;
            case ToggleType.Checkbox:
                ButtonToggleCheckbox();
                return;
        }
    }

    private void ButtonToggleSlide()
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

    private void ButtonToggleCheckbox()
    {
        // Toggle the button
        isOn = !isOn;

        // Call the onToggle event
        onToggle?.Invoke(isOn);

        //Adjust the transparency of the button to show if it's on or off
        if (isOn)
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 1f);
        }
        else
        {
            button.image.color = new Color(button.image.color.r, button.image.color.g, button.image.color.b, 0f);
        }


    }

}
