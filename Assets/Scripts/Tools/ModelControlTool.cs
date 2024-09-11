using System;
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
        public RawImage Icon;

    }

    [SerializeField] private List<ArmorSlot> armorSlots;

    private void Start()
    {            
        toggleModel.onClick.AddListener(PlayerModelHandler.Instance.ToggleModel);

        toggleArmor.onToggle += PlayerModelHandler.Instance.ToggleArmor;

        animationSlider.onValueChanged.AddListener(ChangeAnimation);

        foreach(ArmorSlot slot in armorSlots)
        {
            slot.LeftButton.onClick.AddListener(() => PlayerModelHandler.Instance.ChangeArmor(slot.name, -1));

            slot.RightButton.onClick.AddListener(() => PlayerModelHandler.Instance.ChangeArmor(slot.name, 1));
        }

        PlayerModelHandler.Instance.onArmorEquipped += UpdateArmorIcon;
    }

    private void UpdateArmorIcon(Armor armorEquipped)
    {
        //Debug.Log("Updating armor icon");   

        foreach(ArmorSlot slot in armorSlots)
        {
            if (armorEquipped.name.Contains(slot.name))
            {
                slot.Icon.texture = armorEquipped.icon;
            }
            
        }
    }

    private void ChangeAnimation(float sliderValue)
    {
        PlayerModelHandler.Instance.PlayAnimation((PlayerModelHandler.AnimationState)sliderValue);
    }





}
