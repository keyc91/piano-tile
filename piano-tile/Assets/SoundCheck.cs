using UnityEngine;

public class SoundCheck : MonoBehaviour
{
    void Update()
    {
        // Find all GameObjects with AudioSource components
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Loop through each AudioSource to check if it's playing
        foreach (AudioSource audioSource in audioSources)
        {
            // Check if the AudioSource is playing
            if (audioSource.isPlaying)
            {
                // Output the name of the GameObject with the playing AudioSource
                Debug.Log("GameObject with AudioSource playing sound: " + audioSource.gameObject.name);
            }
        }
    }
}