using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelControlTool : MonoBehaviour
{
    [Header("Model Control Panel")]

    [SerializeField] private Button toggleModel;

    [SerializeField] private Slider animationSlider;


    private void Start()
    {            
        toggleModel.onClick.AddListener(PlayerModelHandler.Instance.ToggleModel);

        animationSlider.onValueChanged.AddListener(ChangeAnimation);
    }

    private void ChangeAnimation(float sliderValue)
    {
        PlayerModelHandler.Instance.PlayAnimation((PlayerModelHandler.AnimationState)sliderValue);
    }

}
