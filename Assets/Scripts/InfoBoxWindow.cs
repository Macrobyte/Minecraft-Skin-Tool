using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBoxWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    public void SetInfoText(string text)
    {
        infoText.text = text;
    }

    private void OnEnable()
    {
        // Set this gameobject to the UI layer
        transform.SetParent(GameObject.Find("UI").transform, false);

        // Set the position to the center of the screen
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
}
