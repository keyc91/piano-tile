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

        // na�ten� cesty k audio souoru skrz po�et hv�zd
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");

        // na�ten� audio souboru
        audioSource.clip = Resources.Load<AudioClip>(path);

        // spu�t�n� souboru
        audioSource.Play();
    }
}
