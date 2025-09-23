using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : Audio
{
    [SerializeField] private AudioClip[] _possibleClips;
    [SerializeField] private bool _waitForCompletion = false;

    private void Start()
    {
        _audioSource.loop = false;
    }

    public void Play()
    {
        if (_waitForCompletion && _audioSource.isPlaying) return;

        _audioSource.Stop();
        _audioSource.clip = _possibleClips[Random.Range(0, _possibleClips.Length)];
        _audioSource.Play();
    }

    public void Stop()
    {
        if (!_audioSource.isPlaying) return;

        _audioSource.Stop();
    }
}
