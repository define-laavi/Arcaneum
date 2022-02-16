using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOnEnable : MonoBehaviour
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
