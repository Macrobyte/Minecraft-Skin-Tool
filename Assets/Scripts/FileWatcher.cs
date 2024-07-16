using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileWatcher : MonoBehaviour
{
    FileSystemWatcher fileWatcher = new FileSystemWatcher();

    // Start is called before the first frame update
    void Start()
    {
        fileWatcher.Path = Application.persistentDataPath + "/Skins"; ;

        Debug.Log(fileWatcher.Path);

        fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.Attributes;

        fileWatcher.Filter = "*.png";

        fileWatcher.Changed += OnChanged;

        fileWatcher.EnableRaisingEvents = true;
    }


    private void InitializeWatcher()
    {

    }

    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Debug.Log("File: " + e.FullPath + " " + e.ChangeType);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
