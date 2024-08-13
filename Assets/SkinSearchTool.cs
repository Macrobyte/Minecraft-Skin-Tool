using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSearchTool : MonoBehaviour
{
    [SerializeField]
    private Button findPlayerButton;

    [SerializeField]
    private TMP_InputField usernameInput;

    [SerializeField]
    private TMP_Text InfoMessage;


    private void Start()
    {
        findPlayerButton.onClick.AddListener(FindAndApplySkin);
    }

    private void FindAndApplySkin()
    {
        if (usernameInput.text.Length <= 0)
        {
            InfoMessage.text = "Please enter a username";
            return;
        }

        MojangAPIHandler.Instance.FindProfile(usernameInput.text, SetInfoMessage, TryApplySkin);
    }

    private void TryApplySkin(MinecraftProfile profile)
    {
        if (profile.skinTexture != null)
        {
            PlayerModelHandler.Instance.ApplySkin(profile.skinTexture, profile.model);
        }
    }


    public void SetInfoMessage(string text)
    {
        // If message is 404 then user not found.
        if (text.Contains("404"))
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
}
