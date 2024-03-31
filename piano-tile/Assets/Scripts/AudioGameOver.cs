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
        // po�et dosa�en�ch hv�zd
        currentStars = PlayerPrefs.GetInt("CurrentStars");

        // cesta k audiu, random v�b�r ze dvou soubor�
        string path = "Audio/" + currentStars + "starsover" + Random.Range(1,3);

        // na�ten� souboru
        audioSource.clip = Resources.Load<AudioClip>(path);

        // spu�t�n�
        audioSource.Play();
    }

}
