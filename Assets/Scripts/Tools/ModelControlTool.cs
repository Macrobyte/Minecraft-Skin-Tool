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

    [SerializeField] private ToggleButton toggleArmor;

    [Serializable]
    private struct ArmorSlot
    {
        public string name;
        public Button LeftButton;
        public Button RightButton;
        public Image Icon;
    }

    [SerializeField] private List<ArmorSlot> armorSlots;

    private void Start()
    {            
        toggleModel.onClick.AddListener(PlayerModelHandler.Instance.ToggleModel);

        toggleArmor.onToggle += PlayerModelHandler.Instance.ToggleArmor;

        animationSlider.onValueChanged.AddListener(ChangeAnimation);

        armorSlots.ForEach(slot =>
        {
            slot.LeftButton.onClick.AddListener(() => PlayerModelHandler.Instance.ChangeArmor(slot.name, -1));
            slot.RightButton.onClick.AddListener(() => PlayerModelHandler.Instance.ChangeArmor(slot.name, 1));
        });
    }

    private void ChangeAnimation(float sliderValue)
    {
        PlayerModelHandler.Instance.PlayAnimation((PlayerModelHandler.AnimationState)sliderValue);
    }





}
