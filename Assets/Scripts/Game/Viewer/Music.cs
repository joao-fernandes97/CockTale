using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    public float Volume
    {
        get { return _volume; }
        set
        {
            _audioSource.volume = value;
            _volume = value;
        }
    }
    
    private float _volume;
}
