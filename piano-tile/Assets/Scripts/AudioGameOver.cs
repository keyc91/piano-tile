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
        // poèet dosažených hvìzd
        currentStars = PlayerPrefs.GetInt("CurrentStars");

        // cesta k audiu, random výbìr ze dvou souborù
        string path = "Audio/" + currentStars + "starsover" + Random.Range(1,3);

        // naètení souboru
        audioSource.clip = Resources.Load<AudioClip>(path);

        // spuštìní
        audioSource.Play();
    }

}
