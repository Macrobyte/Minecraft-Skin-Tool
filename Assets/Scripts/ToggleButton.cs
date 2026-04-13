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

    public Sprite onSprite;
    public Sprite offSprite;


    public void Start()
    {
        switch(toggleType)
        {
            case ToggleType.Slide:
                button.onClick.AddListener(ButtonToggleSlide);
                break;
            case ToggleType.Checkbox:
                button.onClick.AddListener(ButtonToggleCheckbox);
                return;
        }

        onSprite = this.GetComponent<Image>().sprite;

        startPos = button.transform.localPosition.x;

        endPos = startPos * -1;

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

        SwapSprite(isOn);

    }

    private void SwapSprite(bool state)
    {
        GetComponent<Image>().sprite = state ? onSprite : offSprite;
    }
}
