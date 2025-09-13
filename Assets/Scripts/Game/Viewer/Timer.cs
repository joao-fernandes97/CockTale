using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] private TMP_Text _timerTMP;
    private string _format;
    [SerializeField] private float _baseTimer = 66.6f;
    private float _timer;
    public float Time => _timer;

    private void Start()
    {
        _format = _timerTMP.text;
    }

    public void OnEnable()
    {
        _timer = _baseTimer;
    }

    private void Update()
    {
        _timer -= UnityEngine.Time.deltaTime;
        _timerTMP.text = _timer.ToString(_format);
    }
    
}