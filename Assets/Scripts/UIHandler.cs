using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [Header("Skin File Watcher")]
    [SerializeField]
    private Button watchSkinButton;

  


    private void Start()
    {
        

        watchSkinButton.onClick.AddListener(OpenFileBrowser);

    }


    private void OpenFileBrowser()
    {
        //string path = EditorUtility.OpenFilePanel("Open a File", "", "txt");
        //if (!string.IsNullOrEmpty(path))
        //{
        //    Debug.Log("Selected file: " + path);
        //    // Do something with the selected file
        //}
    }

}
