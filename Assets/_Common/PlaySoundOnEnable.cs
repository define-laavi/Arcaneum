using UnityEngine;

namespace Arcadeum.Common
{
    [RequireComponent(typeof(AudioSource))]
    public class PlaySoundOnEnable : MonoBehaviour
    {
        private AudioSource _source;
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _source.Play();
        }
    }
}
