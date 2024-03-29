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

        // v�po�et odlo�en� audia
        float delay = MidiFileInfo.shortestNoteSec * 3;

        // spu�t�n� audia se zpo�d�n�m
        StartCoroutine(DelayedAudio(delay));
    }

    IEnumerator DelayedAudio(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // spu�t�n� mp3 souboru dan�ho levelu
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }
}
