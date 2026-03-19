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

    readonly Dictionary<string, AudioClip> bgmDict = new();
    readonly Dictionary<string, AudioClip> sfxDict = new();

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

    void Start()
    {
        Observer.AddObserver(EventMessage.ON_COMPLETE_LEVEL, PlayCompleteLevelSFX);
        Observer.AddObserver(EventMessage.ON_MERGE_FOOD, PlayMinusFoodSFX);
        Observer.AddObserver(EventMessage.ON_USE_BOOSTER_SHUFFLE, (object[] data) => PlaySFX("SHUFFLE"));
    }

    void OnDestroy()
    {
        Observer.RemoveObserver(EventMessage.ON_COMPLETE_LEVEL, PlayCompleteLevelSFX);
        Observer.RemoveObserver(EventMessage.ON_MERGE_FOOD, PlayMinusFoodSFX);
        Observer.RemoveObserver(EventMessage.ON_USE_BOOSTER_SHUFFLE, (object[] data) => PlaySFX("SHUFFLE"));
    }

    void InitAudioSources()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = .5f;

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

    public void PlayBGM(string key, float volume = 0.5f, bool isLoop = true)
    {
        if (bgmDict.TryGetValue(key, out var clip))
        {
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;

            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.loop = isLoop;
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

    public void PlaySFX(string key, float volume = 1f, bool isLoop = false)
    {
        if (sfxDict.TryGetValue(key, out var clip))
        {
            sfxSource.volume = volume;
            sfxSource.loop = isLoop;
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

    public void PlayMinusFoodSFX(object[] data)
    {
        int randomKey = UnityEngine.Random.Range(1, 9);
        PlaySFX($"{randomKey}");
    }

    void PlayCompleteLevelSFX(object[] data)
    {
        PlaySFX("COMPLETE_LEVEL");
    }
}
