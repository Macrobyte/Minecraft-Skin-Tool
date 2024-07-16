using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyProductVersion : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI productVersionText;

    void Start()
    {
        productVersionText.text = Application.version;
    }
}
