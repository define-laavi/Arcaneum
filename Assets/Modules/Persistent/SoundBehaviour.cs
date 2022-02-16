using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundBehaviour : MonoBehaviour
{
    private static SoundBehaviour _instance;
    public List<SoundClip> sounds;

    // Start is called before the first frame update
    private void Start()
    {
        _instance = this;
        foreach(var soundClip in sounds)
        {
            soundClip.source = gameObject.AddComponent<AudioSource>();
            soundClip.source.clip = soundClip.clip;
            soundClip.source.volume = soundClip.volume;
            soundClip.source.pitch = soundClip.pitch;
        }
    }

    public static void Play(string name)
    {
        var soundClip = _instance.sounds.FirstOrDefault(clip => clip.name.Equals(name));
        soundClip?.source.Play();
    }
}

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}