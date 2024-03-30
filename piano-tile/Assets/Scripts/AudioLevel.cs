using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevel : MonoBehaviour
{
    public AudioSource audioSource;
    public static AudioLevel Instance;
    void Start()
    {
        Instance = this;

        // spuštìní mp3 souboru daného levelu
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }
}
