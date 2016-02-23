using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip MenuMusic;
    public AudioClip GameMusic;

    public AudioClip[] HitTileSounds;
    public AudioClip HitPaddleSound;
    public AudioClip KittyFallSound;
    public AudioClip VictorySound;
    public AudioClip GameOverSound;

    [SerializeField]
    AudioSource m_AudioSourceSFX;
    [SerializeField]
    AudioSource m_AudioSourceMusic;

    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            AudioManager am = GameObject.FindObjectOfType(typeof(AudioManager)) as AudioManager;
            if (am != null)
            {
                m_Instance = am;
            }
            else
            {
                GameObject newManager = new GameObject("Audio Manager");
                //DontDestroyOnLoad(newManager);
                m_Instance = newManager.AddComponent<AudioManager>();
            }
            return m_Instance;
        }
    }


    public void PlaySFX(AudioClip sf)
    {
        m_AudioSourceSFX.clip = sf;
        m_AudioSourceSFX.Play();
    }

    public void PlayMusic(AudioClip music)
    {
        if (m_AudioSourceMusic.clip == music && m_AudioSourceMusic.isPlaying)
            return;

        m_AudioSourceMusic.clip = music;
        m_AudioSourceMusic.Play();
    }

    public void StopSFX()
    {
        m_AudioSourceSFX.Stop();
    }

    public void StopMusic()
    {
        m_AudioSourceMusic.Stop();
    }

    public bool IsMusicPlaying()
    {
        return m_AudioSourceMusic.isPlaying;
    }
}
