using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#nullable enable

public class WebRequestHandler : MonoBehaviour
{

    public void Get(string url, Action<string> onSuccess, Action<string> onError, Action onComplete )  
    {
        StartCoroutine(GetRequest(url, onSuccess, onError, onComplete));
    }

    private IEnumerator GetRequest(string url, Action<string> onSuccess, Action<string> onError, Action onComplete)
    {
        UnityWebRequest request;

        request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();


        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            onError?.Invoke(request.error);
        }
        else if(request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }

        if(request.isDone)
            onComplete?.Invoke();
    }



    #region Downloading Files
    public void DownloadFile(string url, string savePath, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(DownloadFileRequest(url, savePath, onSuccess, onError));
    }

    private IEnumerator DownloadFileRequest(string url, string savePath, Action onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            onError?.Invoke(request.error);
        }
        else if(request.result == UnityWebRequest.Result.Success)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(savePath));
            System.IO.File.WriteAllBytes(savePath, request.downloadHandler.data);
            onSuccess?.Invoke();
        }
    }
    #endregion
}
