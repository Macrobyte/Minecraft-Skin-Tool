using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
struct Audio
{
    public string name;
    public PlayerSounds soundType;
    public AudioClip audioFile;
    [Range(0,1)] public float volume;
    [Range(0, 3)] public float maxPitch;
    [Range(-3, 0)] public float minPitch;
}

public enum PlayerSounds
{
    Fart,
    Jump,
    Land,
    Death,
    Hit,
    Attack,
    Collect,
    LevelUp,
    PowerUp,
}


public class PlayerAudioManager : MonoBehaviour
{
    public static PlayerAudioManager instance;


    public AudioSource playerAudioSource;

    [SerializeField]
    private List<Audio> playerAudioClips = new List<Audio>();

    [SerializeField]
    private List<Audio> environmentSounds = new List<Audio>();

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void PlayPlayerSound(PlayerSounds soundToPlay)
    {
        foreach (Audio audio in playerAudioClips)
        {
            if (audio.soundType == soundToPlay)
            {
                playerAudioSource.clip = audio.audioFile;
                playerAudioSource.volume = audio.volume;
                
                // Random pitch
                playerAudioSource.pitch = UnityEngine.Random.Range(audio.minPitch, audio.maxPitch);

                playerAudioSource.Play();
            }
        }
       
    }


}
