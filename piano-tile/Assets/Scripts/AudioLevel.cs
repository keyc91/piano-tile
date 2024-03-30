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

        // spu�t�n� mp3 souboru dan�ho levelu
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }
}
