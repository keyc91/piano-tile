using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGameOver : MonoBehaviour
{
    public AudioSource audioSource;
    private int currentStars;
    // Update is called once per frame
    void Awake()
    {
        PlayAudio();
    }

    void PlayAudio()
    {
        currentStars = PlayerPrefs.GetInt("CurrentStars");
        string path = "Audio/" + currentStars + "starsover";
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }

}
