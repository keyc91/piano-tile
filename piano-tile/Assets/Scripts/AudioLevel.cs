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

        // výpoèet odložení audia
        float delay = MidiFileInfo.shortestNoteSec * 3;

        // spuštìní audia se zpoždìním
        StartCoroutine(DelayedAudio(delay));
    }

    IEnumerator DelayedAudio(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // spuštìní mp3 souboru daného levelu
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }
}
