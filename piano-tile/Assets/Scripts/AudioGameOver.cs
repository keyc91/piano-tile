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
        // naètení audia z poètu dosažených hvìzd
        currentStars = PlayerPrefs.GetInt("CurrentStars");
        string path = "Audio/" + currentStars + "starsover";
        audioSource.clip = Resources.Load<AudioClip>(path);

        audioSource.Play();
    }

}
