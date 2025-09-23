using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : Audio
{
    private void Start()
    {
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public float Volume
    {
        get { return _volume; }
        set
        {
            Debug.Log("Set music volume at: " + value);
            _audioSource.volume = value;
            _volume = value;
        }
    }

    private float _volume;
}
