using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;         
    public float volume = 0.5f;         

    private AudioSource audioSource;

    private void Awake()
    {
        
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        
        DontDestroyOnLoad(gameObject);

        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = true;

        PlayBackgroundMusic();
    }

    
    public void PlayBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    
    public void StopBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

