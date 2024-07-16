using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class Authenticator : MonoBehaviour
{
    private const string loginUrl = "https://login.live.com/oauth20_authorize.srf?client_id=000000004C12AE6F&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL&display=touch&response_type=token&locale=en";

    public void StartAuthentication()
    {
        StartCoroutine(GetLoginPage());
    }

    private IEnumerator GetLoginPage()
    {
        UnityWebRequest request = UnityWebRequest.Get(loginUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Login Page Received: " + responseText);
            ExtractValues(responseText);
        }
    }

    private void ExtractValues(string pageSource)
    {
        string sFTTagPattern = @"sFTTag:'<input type=""hidden"" name=""PPFT"" id=""i0327"" value=""(.+?)""/>";
        string urlPostPattern = @"urlPost:'(.+?)'";

        Regex sFTTagRegex = new Regex(sFTTagPattern);
        Regex urlPostRegex = new Regex(urlPostPattern);

        Match sFTTagMatch = sFTTagRegex.Match(pageSource);
        Match urlPostMatch = urlPostRegex.Match(pageSource);

        if (sFTTagMatch.Success && urlPostMatch.Success)
        {
            string sFTTagValue = sFTTagMatch.Groups[1].Value;
            string urlPostValue = urlPostMatch.Groups[1].Value;

            //Debug.Log("sFTTag Value: " + sFTTagValue);
            //Debug.Log("urlPost Value: " + urlPostValue);

            // Save these values and proceed to the next step
        }
        else
        {
            //Debug.LogError("Failed to extract sFTTag or urlPost values.");
        }
    }
}