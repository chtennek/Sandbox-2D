using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;

    [Range(0, 1)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    private AudioSource m_source;
    public AudioSource Source { set { m_source = value; } }

    public void Play()
    {
        if (m_source == null)
        {
            Debug.LogWarning("Sound.Play(): No AudioSource assigned!");
            return;
        }
        m_source.clip = GetClip();
        m_source.volume = volume;
        m_source.pitch = pitch;
        m_source.Play();
    }

    public AudioClip GetClip()
    {
        if (clips.Length == 0)
            return null;

        return clips[Random.Range(0, clips.Length)];
    }
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private Dictionary<string, Sound> m_sounds = new Dictionary<string, Sound>();

    public static AudioManager singleton;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogWarning("More than one AudioManager in scene!");
            Destroy(singleton);
        }

        singleton = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        foreach (Sound s in sounds)
        {
            GameObject go = new GameObject("Sound_" + s.name);
            go.transform.parent = transform;
            AudioSource source = go.AddComponent<AudioSource>();

            s.Source = source;
            m_sounds[s.name] = s;
        }
    }

    public static void PlaySound(string name)
    {
        if (singleton == null)
            Debug.LogWarning("PlaySound(): No AudioManager in scene!");

        if (singleton.m_sounds.ContainsKey(name))
            singleton.m_sounds[name].Play();
    }
}
