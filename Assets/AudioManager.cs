using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;         // Assign the audio track in the Inspector
    public float volume = 0.5f;         // Volume level (0.0 to 1.0)

    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Make this object persist across scenes
        DontDestroyOnLoad(gameObject);

        // Set up the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;

        PlayBackgroundMusic();
    }

    // Play the background music
    public void PlayBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Stop the background music
    public void StopBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

