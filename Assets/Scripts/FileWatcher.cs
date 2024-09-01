using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileWatcher
{
    private FileSystemWatcher fileSystemWatcher;

    private string watchedPath;

    public Action onFileChanged;

    public Action onFileAdded;

    public void Init()
    {
        fileSystemWatcher = new FileSystemWatcher();

        fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.Attributes;

        fileSystemWatcher.Filter = "*.png";

        fileSystemWatcher.Changed += OnFileChanged;

        fileSystemWatcher.EnableRaisingEvents = true;
    }

    private void OnFileChanged(object source, FileSystemEventArgs e)
    {        
        // This event is triggered on a background thread.
        // Because Unity API code cant be called from a background thread, we need to use the MainThreadExecutor to execute the code on the main thread.
        MainThreadExecutor.ExecuteOnMainThread(() =>
        {
            onFileChanged?.Invoke();

        });
    }

    public void SetWatchedFile()
    {
        watchedPath = BrowseFile();


        if (watchedPath != null)
        {
            fileSystemWatcher.Path = Path.GetDirectoryName(watchedPath);
            onFileAdded?.Invoke();
        }
    }

    public string GetWatchedPath()
    {
        return watchedPath;
    }

    private string BrowseFile()
    {
        // Open file browser to select a path
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", Application.persistentDataPath + "/Skins/", "png", false);

        if (paths.Length == 0)
        {
            // No file was selected
            return null;
        }

        // Return the directory of the selected file
        return paths[0];      
    }


}
