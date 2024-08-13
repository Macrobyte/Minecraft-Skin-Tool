using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

#region Profile Struct
[System.Serializable]
public struct MinecraftProfile
{
    [Header("Profile Info")]
    public string profileId;
    public string profileName;

    [Header("Textures")]
    public Texture2D skinTexture;
    public Texture2D capeTexture;

    [Header("Other")]
    public Model model;
    public bool hasCape;

    public void ClearProfile()
    {
        profileId = string.Empty;
        profileName = string.Empty;
        skinTexture = null;
        capeTexture = null;
        model = Model.Steve;
        hasCape = false;
    }
}
#endregion

[RequireComponent(typeof(WebRequestHandler))]
public class MojangAPIHandler : MonoBehaviour
{
    public static MojangAPIHandler Instance { get; private set; }

    [Header("Minecraft Profile")]
    [SerializeField]
    private MinecraftProfile currentAccountProfile;

    private WebRequestHandler WebRequestHandler;

    public delegate void OnFetchEvent(string fetchMessage);

    public event OnFetchEvent OnFetch;

    public delegate void OnProfileDownloadedEvent(MinecraftProfile minecraftProfile);

    public event OnProfileDownloadedEvent OnProfileDownloaded;

    public enum APIType
    {
        UUID,
        Profile,
    }

    protected Dictionary<string, string> apiURLs = new Dictionary<string, string>()
    {
        { "uuid", "https://api.mojang.com/users/profiles/minecraft/{username}" },
        { "profile", "https://sessionserver.mojang.com/session/minecraft/profile/{uuid}" },
        { "login", "https://login.live.com/oauth20_authorize.srf?client_id=000000004C12AE6F&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL&display=touch&response_type=token&locale=en" }
    };


    private void Awake()
    {
        WebRequestHandler = GetComponent<WebRequestHandler>();

        if (WebRequestHandler)
            Debug.Log("WebRequestHandler is valid!");

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }


        DontDestroyOnLoad(this);
    }

    public void FindProfile(string username, OnFetchEvent errorMessage = null, OnProfileDownloadedEvent onProfileDownloaded = null)
    {
        FetchUUID(username);

        // Assign the error message to the OnFetchError event
        // This is an optional parameter if you want to display an error message through a callback method.
        if (errorMessage != null)
            OnFetch += errorMessage;

        if(onProfileDownloaded != null)
            OnProfileDownloaded += onProfileDownloaded;
    }

    public MinecraftProfile GetCurrentProfile()
    {
        return currentAccountProfile;
    }

    public static bool IsProfileValid(MinecraftProfile profile)
    {
        return profile.profileId.Length > 0;
    }

    #region UUID Fetching
    private void FetchUUID(string username)
    {
        currentAccountProfile.ClearProfile();

        string uuidUrl = apiURLs["uuid"].Replace("{username}", username);

        WebRequestHandler.Get(uuidUrl, OnUUIDFound, (error) => OnFetchFailed(error, APIType.Profile), () => OnFetchComplete(APIType.UUID) );
    }

    private void OnUUIDFound(string successText)
    {
        Debug.Log("Successfuly obtained player UUID");

        // Regex pattern to find the UUID
        string pattern = "\"id\"\\s*:\\s*\"([^\"]+)\"";
        Match match = Regex.Match(successText, pattern);

        if (match.Success)
        {
            string id = match.Groups[1].Value; // The first group captures the UUID
            currentAccountProfile.profileId = id;
        }
        else
        {
            Debug.LogError("UUID not found in the response.");
        }
    }
    #endregion

    #region Profile Fetching
    private void FetchProfile(string uuid)
    {
        string profileUrl = apiURLs["profile"].Replace("{uuid}", uuid);

        WebRequestHandler.Get(profileUrl, OnProfileFound, (error) => OnFetchFailed(error, APIType.Profile), () => OnFetchComplete(APIType.Profile));
    }

    private void OnProfileFound(string successText)
    {
        Debug.Log("Successfully obtained player profile");

        // Regex pattern to find the base64 encoded "value"
        string pattern = "\"value\"\\s*:\\s*\"([^\"]+)\"";
        MatchCollection matches = Regex.Matches(successText, pattern);

        if (matches.Count > 0)
        {
            // Only one match is expected
            string base64Value = matches[0].Groups[1].Value;
            
            // Using custom method to decode the base64 value and return it as a string
            string decodedString = DecodeBase64Value(base64Value);

            if(decodedString.Length <= 0)
            {
                Debug.LogWarning("Decoded base64 value is empty. Aborting profile fetch.");
                return;
            }
            else
            {
                SetProfileValues(decodedString);
            }
        }
        else
        {
            Debug.LogError("Base64 encoded 'value' not found in the response.");
        }

    }
    #endregion

    #region Setting Profile Values
    private void SetProfileValues(string decodedString)
    {
        MinecraftProfile profile = new MinecraftProfile();

        // Regex for 'profileName'
        string profileNamePattern = "\"profileName\"\\s*:\\s*\"([^\"]+)\"";
        Match profileNameMatch = Regex.Match(decodedString, profileNamePattern);

        if (profileNameMatch.Success)
        {
            profile.profileName = profileNameMatch.Groups[1].Value;
        }

        // Regex for 'profileId'
        string profileIdPattern = "\"profileId\"\\s*:\\s*\"([^\"]+)\"";
        Match profileIdMatch = Regex.Match(decodedString, profileIdPattern);

        if (profileIdMatch.Success)
        {
            profile.profileId = profileIdMatch.Groups[1].Value;
        }

        // Regex pattern for 'model'
        string modelPattern = "\"model\"\\s*:\\s*\"([^\"]+)\"";
        Match modelMatch = Regex.Match(decodedString, modelPattern);

        // The json file doesn't contain the 'model' value if it's Steve.
        // This future-proofs the code in case Mojang decides to add the 'model' value for players with the Steve skin.
        if (modelMatch.Success)
        {
            if(modelMatch.Groups[1].Value == "slim")
            {
                profile.model = Model.Alex;
            }
            else
            {
                profile.model = Model.Steve;
            }
        }

        // Regex pattern for 'url' value.
        string urlPattern = "\"url\"\\s*:\\s*\"([^\"]+)\"";
        MatchCollection urlMatches = Regex.Matches(decodedString, urlPattern);

        // First 'url' match is the skin texture.
        string skinDownloadUrl = urlMatches[0].Groups[1].Value;

        //Both skin and cape texture are assigned to the profile through the 'OnSkinDownloaded' and 'OnCapeDownloaded' methods.

        // Web request to download the skin texture
        WebRequestHandler.DownloadFile(skinDownloadUrl, Application.persistentDataPath + "/Skins/tempSkin.png", OnSkinDownloaded, (error) => Debug.LogError("Error downloading texture: " + error));

        // If there is more than one 'url' match, the second one is the cape texture.
        if (urlMatches.Count > 1)
        {
            string capeDownloadUrl = urlMatches[1].Groups[1].Value;
          
            // Web request to download the cape texture
            WebRequestHandler.DownloadFile(capeDownloadUrl, Application.persistentDataPath + "/Capes/tempSkin.png", OnCapeDownloaded, (error) => Debug.LogError("Error downloading texture: " + error));

            profile.hasCape = true;
        }  

        // Assign the profile to the current account profile
        currentAccountProfile = profile;

        
    }

    private void OnSkinDownloaded()
    {
        string filePath = Application.persistentDataPath + "/Skins/tempSkin.png";
        if (System.IO.File.Exists(filePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D skinTexture = new Texture2D(2, 2);
            if (skinTexture.LoadImage(fileData))
            {
                skinTexture.filterMode = FilterMode.Point;
                currentAccountProfile.skinTexture = skinTexture;

                OnProfileDownloaded?.Invoke(currentAccountProfile);
            }
            else
            {
                Debug.LogError("Failed to load skin texture from file.");
            }
        }
        else
        {
            Debug.LogError("Skin file does not exist at the specified path.");
        }

        
    }

    private void OnCapeDownloaded()
    { 
        string filePath = Application.persistentDataPath + "/Capes/tempSkin.png";
        if (System.IO.File.Exists(filePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            Texture2D capeTexture = new Texture2D(2, 2);
            if (capeTexture.LoadImage(fileData))
            {
                capeTexture.filterMode = FilterMode.Point;
                currentAccountProfile.capeTexture = capeTexture;

                Debug.Log("Cape texture successfully loaded and assigned.");
            }
            else
            {
                Debug.LogError("Failed to load cape texture from file.");
            }
        }
        else
        {
            Debug.LogError("Cape file does not exist at the specified path.");
        }
      
    }

    #endregion

    #region Fetching Status
    // Method to be called when a fetch is complete, depending on the API type we can perform different actions.
    private void OnFetchComplete(APIType type)
    {
        switch(type)
        {
            case APIType.UUID:    
                
                if(currentAccountProfile.profileId.Length <= 0)         
                    return;

                FetchProfile(currentAccountProfile.profileId);

                break;
            case APIType.Profile:

                OnFetch?.Invoke("");

                break;
            default:
                break;
        }
    }

    private void OnFetchFailed(string error, APIType type)
    {
        switch(type)
        {
            case APIType.UUID:
                OnFetch?.Invoke(error);
                break;
            case APIType.Profile:
                OnFetch?.Invoke(error);
                break;
            default:
                break;
        }
    }

    #endregion

    // Helper method to decode base64 string
    private string DecodeBase64Value(string base64string)
    {
        try
        {
            // Decode the base64 string
            byte[] data = Convert.FromBase64String(base64string);

            // Convert the byte array to a string and return it
            return Encoding.UTF8.GetString(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Error decoding base64 string: " + e.Message);
            return string.Empty;
        }
    }

}

