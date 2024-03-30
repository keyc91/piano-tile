using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGameOver : MonoBehaviour
{
    public AudioSource audioSource;
    private int currentStars;

    void Awake()
    {
        PlayAudio();
    }

    void PlayAudio()
    {
        // na�ten� audia z po�tu dosa�en�ch hv�zd
        currentStars = PlayerPrefs.GetInt("CurrentStars");
        string path;
        if (currentStars == 0) path = "Audio/0starsover" + Random.Range(1, 3);
        else path = "Audio/" + currentStars + "starsover";
        audioSource.clip = Resources.Load<AudioClip>(path);

        audioSource.Play();
    }

}
