using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource2D;
    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource2D = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound2D(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip != null)
        {
            Instance.audioSource2D.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SoundManager: AudioClip '{clipName}' not found in Resources.");
        }
    }

    public static void PlaySound3D(string clipName, Vector3 position, float minDIstance = 1.0f, float maxDistance = 500.0f)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        if (clip != null)
        {
            GameObject audioObject = new GameObject("3DAudio_" + clipName);
            audioObject.transform.position = position;
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f;
            audioSource.minDistance = minDIstance;
            audioSource.maxDistance = maxDistance;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            audioSource.Play();

            Instance.activeAudioSources.Add(audioSource);

            Object.Destroy(audioObject, clip.length);
        }
        else
        {
            Debug.LogWarning($"SoundManager: AudioClip '{clipName}' not found in Resources.");
        }
    }

    public static void MuteAllSounds(bool mute)
    {
        Instance.audioSource2D.mute = mute;
        foreach (var audioSource in Instance.activeAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.mute = mute;
            }
        }
    }

    public static void MuteSound(string clipName, bool mute)
    {
        if (Instance.audioSource2D.clip != null && Instance.audioSource2D.clip.name == clipName)
        {
            Instance.audioSource2D.mute = mute;
        }

        foreach (var audioSource in Instance.activeAudioSources)
        {
            if (audioSource != null && audioSource.clip != null && audioSource.clip.name == clipName)
            {
                audioSource.mute = mute;
            }
        }
    }
}
