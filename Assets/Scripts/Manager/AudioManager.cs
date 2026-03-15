using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SoundConfig
{
    public string key;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Configurations")]
    [SerializeField] SoundConfig[] bgmConfigs;
    [SerializeField] SoundConfig[] sfxConfigs;

    Dictionary<string, AudioClip> bgmDict = new();
    Dictionary<string, AudioClip> sfxDict = new();

    AudioSource bgmSource;
    AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitAudioSources();
        InitSoundDicts();
    }

    void InitAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
    }

    void InitSoundDicts()
    {
        foreach (var bgm in bgmConfigs)
        {
            if (!bgmDict.ContainsKey(bgm.key))
                bgmDict.Add(bgm.key, bgm.clip);
        }

        foreach (var sfx in sfxConfigs)
        {
            if (!sfxDict.ContainsKey(sfx.key))
                sfxDict.Add(sfx.key, sfx.clip);
        }
    }

    public void PlayBGM(string key)
    {
        if (bgmDict.TryGetValue(key, out var clip))
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM with key '{key}' not found!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string key)
    {
        if (sfxDict.TryGetValue(key, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX with key '{key}' not found!");
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void PlayMinusFoodSFX()
    {
        int randomKey = UnityEngine.Random.Range(1, 9);
        PlaySFX($"{randomKey}");
    }
}
