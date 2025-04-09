using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;

    public AudioClip bgm;

    private void Awake()
    {
        if (Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
