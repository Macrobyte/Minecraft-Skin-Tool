using SFB;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWindow : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button saveCurrentButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton;

    [Header("Info Window")]
    [SerializeField] private GameObject infoWindowPrefab;

    [Header("Credits Window")]
    [SerializeField] private GameObject creditsWindowPrefab;

    private void Awake()
    {
        saveCurrentButton.onClick.AddListener(SaveCurrent);
        creditsButton.onClick.AddListener(OpenCreditsWindow);
        quitButton.onClick.AddListener(Quit);
        backButton.onClick.AddListener(Back);
    }

    private void OpenCreditsWindow()
    {
        GameObject creditsWindow = GameObject.Instantiate(creditsWindowPrefab);
    }

    private void Back()
    {
        ToggleWindow();
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void SaveCurrent()
    {
        Texture2D textureToSave = PlayerModelHandler.Instance.GetCurrentSkin();
        
        SaveTextureToFile(textureToSave);

        GameObject infoWindow = GameObject.Instantiate(infoWindowPrefab);
        infoWindow.GetComponent<InfoBoxWindow>().SetInfoText("Texture saved to file.");

    }

    public void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void SaveTextureToFile(Texture2D texture)
    {
        // Choose path to save to
        string pathToSaveTo = StandaloneFileBrowser.SaveFilePanel("Save File", "", "MyTexture", "png");

        // Encode texture to PNG using Texture2D.EncodeToPNG
        byte[] textureByteArray = texture.EncodeToPNG();

        // Write the byte array to the chosen path
        File.WriteAllBytes(pathToSaveTo, textureByteArray);
    }
}
