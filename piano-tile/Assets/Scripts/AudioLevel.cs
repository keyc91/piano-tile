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
        // vypocet zdrzeni audia
        float delay = MidiFileInfo.shortestNoteSec * 4;

        // spusteni audia se zpozdenim
        StartCoroutine(DelayedAudio(delay));
    }

    IEnumerator DelayedAudio(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        // spusteni audio filu
        string path = "Audio/" + PlayerPrefs.GetString("CurrentLevel");
        audioSource.clip = Resources.Load<AudioClip>(path);
        audioSource.Play();
    }
}
