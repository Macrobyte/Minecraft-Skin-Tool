using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreditsWindow : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    public void OnEnable()
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
}
