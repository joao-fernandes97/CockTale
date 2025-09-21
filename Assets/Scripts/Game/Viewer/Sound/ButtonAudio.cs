using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Component must be in same game object as ui object to use hover sound
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ButtonAudio : Audio, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioClip _press;
    [SerializeField] private AudioClip _hover;
    [SerializeField] private AudioClip _unHover;

    private void Start()
    {
        _audioSource.loop = false;

        if (_button != null)
            _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        PlayClip(_press);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayClip(_hover);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PlayClip(_unHover);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null)
            return;

        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(OnButtonClicked);
    }
}
