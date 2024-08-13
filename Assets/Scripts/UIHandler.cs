using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("API Reference")]
    [SerializeField]
    private MojangAPIHandler mojangApi;

    [Header("Model Control Panel")]
    [SerializeField]
    private Button toggleDetachButton;

    [SerializeField]
    private Button toggleRotationButton;

    [SerializeField]
    private Button rotateLeftButton;

    [SerializeField]
    private Button rotateRightButton;

    [Header("Skin File Watcher")]
    [SerializeField]
    private Button watchSkinButton;

  


    private void Start()
    {
        mojangApi = FindObjectOfType<MojangAPIHandler>();

        //toggleDetachButton.onClick.AddListener(playerModel.ToggleDetachMode);

        //toggleRotationButton.onClick.AddListener(playerModel.ToggleRotationSpeed);

        rotateLeftButton.onClick.AddListener(RotateModelLeft);

        rotateRightButton.onClick.AddListener(RotateModelRight);

        watchSkinButton.onClick.AddListener(OpenFileBrowser);


    }


    

    #region Model Control Panel
    private void RotateModelLeft()
    {
        //playerModel.SetRotationDirection(PlayerModelHandler.RotationDirection.Left);
    }

    private void RotateModelRight()
    {
        //playerModel.SetRotationDirection(PlayerModelHandler.RotationDirection.Right);
    }
    #endregion

    private void OpenFileBrowser()
    {
        string path = EditorUtility.OpenFilePanel("Open a File", "", "txt");
        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log("Selected file: " + path);
            // Do something with the selected file
        }
    }

}
