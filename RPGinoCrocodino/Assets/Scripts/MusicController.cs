using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // чтобы не было дублей
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}