using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkinWatcherTool : MonoBehaviour
{


    [SerializeField] private Button watchSkinButton;



    FileSystemWatcher fileWatcher = new FileSystemWatcher();


    private void Awake()
    {
        watchSkinButton.onClick.AddListener(BrowseFile);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeWatcher();
    }


    private void InitializeWatcher()
    {

        fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.Attributes;

        fileWatcher.Filter = "*.png";

        fileWatcher.Changed += OnChanged;

        fileWatcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Debug.Log("File: " + e.FullPath + " " + e.ChangeType);
    }


    public void BrowseFile()
    {
        
    }

    private void WatchFile(string path)
    {
        fileWatcher.Path = path;
    }


}
