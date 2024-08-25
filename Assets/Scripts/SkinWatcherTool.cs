using SFB;
using System;
using System.Collections.Concurrent;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinWatcherTool : MonoBehaviour
{
    [Header("UI Elements")]
    public Button testButton;
    [SerializeField] private Button watchSkinButton;
    [SerializeField] private TMP_Text trackedFileInfoText;

    private FileWatcher fileWatcher;

    [SerializeField] private Texture2D currentSkinTexture;

    private void Awake()
    {
        fileWatcher = new FileWatcher();

        fileWatcher.Init();

        watchSkinButton.onClick.AddListener(fileWatcher.SetWatchedFile);

        fileWatcher.onFileChanged += OnSkinFileUpdated;

        fileWatcher.onFileAdded += OnSkinFileAdded;
    }

    private void OnSkinFileAdded()
    {
        currentSkinTexture = LoadTextureFromPath(fileWatcher.GetWatchedPath());

        PlayerModelHandler.Instance.ApplySkin(currentSkinTexture);

        Debug.Log("Skin file added");
    }

    private void OnSkinFileUpdated()
    {
        currentSkinTexture = LoadTextureFromPath(fileWatcher.GetWatchedPath());

        PlayerModelHandler.Instance.ApplySkin(currentSkinTexture);

        Debug.Log("Skin file updated");
    }


    private Texture2D LoadTextureFromPath(string path)
    {
        if(File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);

            Texture2D skinTexture = new Texture2D(2, 2);

            if (skinTexture.LoadImage(fileData))
            {
                skinTexture.filterMode = FilterMode.Point;
                Debug.Log("Texture loaded successfully");
                return skinTexture;

            }
            else
            {
                Debug.LogError("Failed to load texture");
                return null;

            }
        }
        else
        {
            Debug.LogError("Texture file does not exist");
            return null;
        }   
    }
}
