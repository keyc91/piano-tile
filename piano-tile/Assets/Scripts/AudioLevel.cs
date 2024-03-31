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

        // naètení cesty k audio souoru skrz poèet hvìzd
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");

        // naètení audio souboru
        audioSource.clip = Resources.Load<AudioClip>(path);

        // spuštìní souboru
        audioSource.Play();
    }
}
