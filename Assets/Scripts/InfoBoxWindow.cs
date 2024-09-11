using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoxWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;

    [SerializeField] private Button closeButton;

    private void OnEnable()
    {
        // Set this gameobject to the UI layer
        transform.SetParent(GameObject.Find("UI").transform, false);

        // Set the position to the center of the screen
        transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Set the close button to close the window
        closeButton.onClick.AddListener(CloseWindow);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void SetInfoText(string text)
    {
        infoText.text = text;
    }
}
