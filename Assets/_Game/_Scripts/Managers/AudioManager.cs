using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    [SerializeField] Sound[] StaticSounds;
    [SerializeField] Sound[] DynamicSounds;

    public static AudioManager _singleton;
    public static AudioManager Singleton
    {
        get => _singleton;
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Destroy(value.gameObject);
                Debug.Log($"{nameof(AudioManager)} instance already exists, destroying duplicate");
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Singleton = this;

        foreach (var S in StaticSounds)
        {
            S.source = gameObject.AddComponent<AudioSource>();
            S.source.clip = S.clip;
            S.source.volume = S.volume;
            S.source.loop = S.loop;
        }
    }

    /// <summary>
    /// Play Static sound which are in the component
    /// </summary>
    /// <param name="name">Enter the name of the sound</param>
    public void Play(string name)
    {
        Sound s = Array.Find(StaticSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} not found");
            return;
        }
        s.source.Play();
    }

    /// <summary>
    /// Stop Static sound which are in the component
    /// </summary>
    /// <param name="name">Enter the name of the sound</param>
    public void Stop(string name)
    {
        Sound s = Array.Find(StaticSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} not found");
            return;
        }
        s.source.Stop();
    }

    // Dynamic Sound Functions
    public void PlayNewSound(string name, float destroyTime)
    {
        Sound s = Array.Find(DynamicSounds, sound => sound.name == name);
        if (s == null)
            return;
        s = AddAudioSource(s);
        s.source.Play();
        Destroy(s.source, destroyTime);
        print("Play new Sound");
    }

    public void PlayNewSound(string name)
    {
        Sound s = Array.Find(DynamicSounds, sound => sound.name == name);
        if (s == null)
            return;
        s = AddAudioSource(s);
        s.source.Play();
        Destroy(s.source, s.source.clip.length);
        print("Play new Sound");
    }

    Sound AddAudioSource(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.loop = s.loop;
        return s;
    }

    // Volume Setting Functions
    public void ChangeVolume(float _volume)
    {
        foreach (Sound sound in StaticSounds)
        {
            sound.source.volume = _volume;
        }
    }

    public void MuteSound()
    {
        foreach (Sound sound in StaticSounds)
        {
            sound.source.Stop();
            sound.source.volume = 0;
        }
    }

    public void UnMuteSoundFromMainMenu(float _volume, string _playSound)
    {
        foreach (Sound sound in StaticSounds)
        {
            sound.source.volume = _volume;
        }
        Play(_playSound);
    }
}
