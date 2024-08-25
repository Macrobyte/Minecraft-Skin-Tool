using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelControlTool : MonoBehaviour
{
    [Header("Model Control Panel")]

    [SerializeField] private Button toggleDetachButton;

    [SerializeField] private Button toggleModel;


    private void Start()
    {
        toggleDetachButton.onClick.AddListener(PlayerModelHandler.Instance.ToggleDetachMode);
        
        toggleModel.onClick.AddListener(PlayerModelHandler.Instance.ToggleModel);
    }

    //private void RotateModelLeft()
    //{
    //    PlayerModelHandler.Instance.SetRotationDirection(PlayerModelHandler.RotationDirection.Left);
    //}

    //private void RotateModelRight()
    //{
    //    PlayerModelHandler.Instance.SetRotationDirection(PlayerModelHandler.RotationDirection.Right);
    //}
}
