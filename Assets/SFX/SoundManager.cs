using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public int NumAudioSources;

    public List<MusicAudioEntry> Music;
    public List<SFXAudioEntry> SFX;

    private Dictionary<string, MusicAudioEntry> _musicDict;
    private Dictionary<string, SFXAudioEntry> _sfxDict;

    private AudioSource[] _audioSources;
    private int _audioSourceIndex;
    private AudioSource _musicSource;

    private void Awake()
    {
        instance = this;

        GlobalEvents.WinLevel.AddListener(handleWinLevel);
        GlobalEvents.LoseLevel.AddListener(handleLoseLevel);
        GlobalEvents.NextLevelEvent.AddListener(handleNextLevel);
        GlobalEvents.RetryLevelEvent.AddListener(handleNextLevel);
    }

    private void handleWinLevel()
    {
        PlaySFX("win");
        StopMove();
        PauseMusic();
    }

    private void handleLoseLevel()
    {
        PlaySFX("lose");
        StopMove();
        PauseMusic();
    }

    private void handleNextLevel()
    {
        Button();
    }

    public static void Button()
    {
        SoundManager.instance.PlaySFX("button");
    }

    public static void Move()
    {
        SoundManager.instance.PlaySFX("move", 0, true);
    }

    public static void StopMove()
    {
        SoundManager.instance.StopSFX("move");
    }

    /// <summary>
    /// Create dictionaries for music and sfx for fast lookup
    /// Create audio sources
    /// </summary>
    private void Start()
    {
        _musicDict = new Dictionary<string, MusicAudioEntry>();
        foreach (var musicClip in Music)
        {
            _musicDict.Add(musicClip.Name, musicClip);
        }

        _sfxDict = new Dictionary<string, SFXAudioEntry>();
        foreach (var sfxClip in SFX)
        {
            _sfxDict.Add(sfxClip.Name, sfxClip);
        }

        _musicSource = createAudioSource(true);
        _audioSources = new AudioSource[NumAudioSources];
        for (int i = 0; i < NumAudioSources; i++)
        {
            _audioSources[i] = createAudioSource();
        }
    }

    private AudioSource createAudioSource(bool music = false)
    {
        GameObject audioSourceObj = new GameObject();
        AudioSource newAudioSource = audioSourceObj.AddComponent<AudioSource>();
        audioSourceObj.transform.parent = transform;
        audioSourceObj.name = "AudioSource";
        newAudioSource.playOnAwake = false;
        return newAudioSource;
    }

    public SFXAudioEntry GetSFX(string name)
    {
        SFXAudioEntry entry = null;
        if (_sfxDict != null && _sfxDict.ContainsKey(name))
        {
            entry = _sfxDict[name];
        }
        return entry;
    }

    public MusicAudioEntry GetMusic(string name)
    {
        MusicAudioEntry entry = null;
        if (_musicDict != null && _musicDict.ContainsKey(name))
        {
            entry = _musicDict[name];
        }
        return entry;
    }

    public AudioSource GetMusicSource()
    {
        return _musicSource;
    }

    public void PlaySFX(string name, float delay = 0f, bool loop = false)
    {
        SFXAudioEntry entry = GetSFX(name);
        if (entry != null && entry.AudioClip != null)
        {
            AudioSource source = getAudioSource();
            if (source != null)
            {
                source.Stop();
                source.gameObject.name = "SFX-" + name;
                source.volume = entry.Volume;
                source.clip = entry.GetAudioClip();
                source.pitch = Time.timeScale;
                source.loop = loop;

                if (delay > 0)
                {
                    source.PlayDelayed(delay);
                }
                else
                {
                    source.Play();
                }
            }
        }
    }

    /// <summary>
    /// Stop sfx which contains name
    /// </summary>
    /// <param name="name"></param>
    public void StopSFX(string name)
    {
        foreach (var source in _audioSources)
        {
            if (source.gameObject.name.Contains(name))
            {
                source.Stop();
            }
        }
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Pause();
        }
    }

    public void MuteMusic(bool mute = true)
    {
        _musicSource.mute = mute;
    }

    /// <summary>
    /// Mute all sfx
    /// </summary>
    /// <param name="mute"></param>
    public void MuteSFX(bool mute = true)
    {
        foreach (var source in _audioSources)
        {
            source.mute = mute;
        }
    }

    private void playMusicForEntry(MusicAudioEntry entry, bool playIntro, bool randomStart)
    {
        _musicSource.Stop();
        _musicSource.gameObject.name = "Music-" + entry.Name;

        _musicSource.clip = entry.AudioClip;
        _musicSource.pitch = Time.timeScale;
        _musicSource.loop = true;
        _musicSource.volume = entry.Volume;
        _musicSource.timeSamples = 0;
        if (randomStart)
        {
            _musicSource.timeSamples = Random.Range(0, _musicSource.clip.samples);
        }
        _musicSource.Play();
    }

    public void PlayMusic(string name, float fadeTime = 0, bool playIntro = false, bool randomStart = false)
    {
        MusicAudioEntry entry = GetMusic(name);
        if (entry != null && entry.AudioClip != null)
        {
            if (fadeTime > 0f)
            {
                _musicSource.volume = 0f;
                playMusicForEntry(entry, playIntro, randomStart);
                _musicSource.DOFade(entry.Volume, fadeTime);
            }
            else
            {
                if (_musicSource != null)
                {
                    playMusicForEntry(entry, playIntro, randomStart);
                }
            }
        }
    }

    private AudioSource getAudioSource()
    {
        AudioSource source = null;
        for (int i = 0; i < NumAudioSources; i++)
        {
            source = _audioSources[_audioSourceIndex];
            _audioSourceIndex = (_audioSourceIndex + 1) % NumAudioSources;
            if (source != null && !source.isPlaying)
            {
                break;
            }
        }

        return source;
    }
}
