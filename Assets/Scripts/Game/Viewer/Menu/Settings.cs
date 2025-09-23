using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : Menu
{
    [SerializeField] private GameObject _settingsObject;
    [SerializeField] private Slider _brightness;
    [SerializeField] private TMP_Text _brightnessText;
    [SerializeField] private Slider _volume;
    [SerializeField] private TMP_Text _volumeText;

    private Music _music;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private TMP_Text _musicVolumeText;

    [SerializeField] private float _maxVolume = 3f;
    [SerializeField] private float _clampBrightness = 2.5f;

    [SerializeField] private Volume volume;
    private ColorAdjustments _postExposure;

    private void Awake()
    {
        volume.profile.TryGet(out _postExposure);

        _brightness.onValueChanged.AddListener(ChangeBrightness);
        _volume.onValueChanged.AddListener(ChangeVolume);
        _musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
    }

    private void OnEnable()
    {
        _volume.value = 1f;
        _musicVolume.value = 1f;
        _brightness.value = 0.5f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New Scene Loaded: " + scene.name);
        Start();
    }
    private void Start()
    {
        ChangeBrightness(_brightness.value);
        ChangeVolume(_volume.value);
        ChangeMusicVolume(_musicVolume.value);

        TurnOffSettings();
    }

    public void TurnOnSettings()
    {
        if ( _settingsObject != null )
            _settingsObject.SetActive(true);
    }

    public void TurnOffSettings()
    {
        if ( _settingsObject != null )
            _settingsObject.SetActive(false);
    }

    public bool GetActive()
    {
        return _settingsObject.activeSelf;
    }

    public void ChangeBrightness(float value)
    {
        if ( _postExposure == null ) return;
        
        float final =  _clampBrightness * (value - _brightness.minValue)
            / (_brightness.maxValue - _brightness.minValue);

        _postExposure.postExposure.value =  final;
        
        if (_volumeText != null)
            _volumeText.text = FormatShort(final);
    }

    public void ChangeVolume(float value)
    {
        float final = _maxVolume * (value - _volume.minValue)
            / (_volume.maxValue - _volume.minValue);
            
        AudioListener.volume = final;
        
        if (_volumeText != null)
            _volumeText.text = FormatShort(final);
    }
    public void ChangeMusicVolume(float value)
    {
        float final = _maxVolume * (value - _musicVolume.minValue)
            / (_musicVolume.maxValue - _musicVolume.minValue);

        if (_music == null)
            _music = FindFirstObjectByType<Music>();

        if (_music != null)
            _music.Volume = final;
        
        if (_musicVolumeText != null)
            _musicVolumeText.text = FormatShort(final);
    }

    private string FormatShort(float value)
    {
        if (value >= 10f)
            return Mathf.RoundToInt(value).ToString();
        else
            return value.ToString("0.0");
    }

    public void OnDestroy()
    {
        _brightness.onValueChanged.RemoveListener(ChangeBrightness);
        _volume.onValueChanged.RemoveListener(ChangeVolume);
        _musicVolume.onValueChanged.RemoveListener(ChangeMusicVolume);
    }
}
