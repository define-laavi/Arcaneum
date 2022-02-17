using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcadeum.Common
{
    public class SoundPool : MonoBehaviour
    {
        private static SoundPool _instance;
        [SerializeField] private List<SoundClip> _sounds;

        private void Start()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this;
            foreach (var soundClip in _sounds)
            {
                soundClip.source = gameObject.AddComponent<AudioSource>();
                soundClip.source.clip = soundClip.clip;
            }
        }
        
        public static void ChangeVolumeAndPitch(AudioClip clip, float volume = 0.2f, float pitch = 1, bool loops = true)
        {
            if (_instance == null)
                throw new System.Exception("There is no SoundPool added to the scene!");

            var soundClip = _instance._sounds.FirstOrDefault(aclip => aclip.clip == clip);
            var source = default(AudioSource);
            if (soundClip != null)
            {
                source = soundClip.source;
            }
            else
            {
                source = _instance.AddSoundClip(clip).source;
            }

            source.volume = volume;
            source.pitch = pitch;
            source.loop = loops;
        }


        public static void Play(AudioClip clip, float volume = 0.2f, float pitch = 1)
        {
            if (_instance == null)
                throw new System.Exception("There is no SoundPool added to the scene!");

            var soundClip = _instance._sounds.FirstOrDefault(aclip => aclip.clip == clip);
            var source = default(AudioSource);
            if (soundClip != null)
            {
                source = soundClip.source;
            }
            else
            {
                source = _instance.AddSoundClip(clip).source;
            }

            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }

        private SoundClip AddSoundClip(AudioClip clip)
        {
            var soundClip = new SoundClip()
            {
                clip = clip,
                name = clip.name,
                source = gameObject.AddComponent<AudioSource>()
            };

            soundClip.source.clip = soundClip.clip;

            _sounds.Add(soundClip);

            return soundClip;
        }
    }

    [System.Serializable]
    public class SoundClip
    {
        public string name;
        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;
    }
}