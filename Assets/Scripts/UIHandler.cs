using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("API Reference")]
    [SerializeField]
    private MojangAPIHandler mojangApi;

    [Header("Profile Request Panel")]
    [SerializeField]
    private Button findPlayerButton;

    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_Text InfoMessage;

    [Header("Player Model Reference")]
    [SerializeField]
    private PlayerModelHandler playerModel;

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

        playerModel = FindObjectOfType<PlayerModelHandler>();

        findPlayerButton.onClick.AddListener(FindAndApplySkin);

        toggleDetachButton.onClick.AddListener(playerModel.ToggleDetachMode);

        toggleRotationButton.onClick.AddListener(playerModel.ToggleRotationSpeed);

        rotateLeftButton.onClick.AddListener(RotateModelLeft);

        rotateRightButton.onClick.AddListener(RotateModelRight);

        watchSkinButton.onClick.AddListener(OpenFileBrowser);


    }


    #region Profile Request Panel Control
    private void FindAndApplySkin()
    {
        if(usernameInput.text.Length <= 0)
        {
            InfoMessage.text = "Please enter a username";
            return;
        }

        mojangApi.FindProfile(usernameInput.text, SetInfoMessage, TryApplySkin);      
    }

    private void TryApplySkin(MinecraftProfile profile)
    {
        if(profile.skinTexture != null)
        {
            playerModel.ApplySkin(profile.skinTexture, profile.model);
        }
    }


    public void SetInfoMessage(string text)
    {
        // If message is 404 then user not found.
        if(text.Contains("404"))
        {
            InfoMessage.text = "User not found!";
            InfoMessage.color = Color.red;
            return;
        }
        else
        {
            InfoMessage.text = "User Found!";
            InfoMessage.color = Color.green;
        }   
    }
    #endregion

    #region Model Control Panel
    private void RotateModelLeft()
    {
        playerModel.SetRotationDirection(PlayerModelHandler.RotationDirection.Left);
    }

    private void RotateModelRight()
    {
        playerModel.SetRotationDirection(PlayerModelHandler.RotationDirection.Right);
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
