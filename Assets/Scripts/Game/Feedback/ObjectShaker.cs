using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ObjectShaker : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _duration = 0f;
    [SerializeField] private float _magnitude = 0f;
    private float _actualDuration = 0f;
    private float _actualMagnitude = 0f;
    private Vector3 _original;

    private Coroutine _shaking;

    private void Start()
    {
        _original = _transform.position;
    }

    public void Shake(float duration = 0f, float magnitude = 0f)
    {
        _actualDuration = _duration;
        _actualMagnitude = _magnitude;

        if (duration != 0f)
            _actualDuration *= duration;
        if (duration != 0f)
            _actualMagnitude *= magnitude;

        Debug.Log("Set actuals as dur: " + _actualDuration + " and mag: " + _actualMagnitude);

        if (_shaking != null)
            StopCoroutine(_shaking);
        _shaking = StartCoroutine(StartShaker());
    }

    private IEnumerator StartShaker()
    {
        float x;
        float y;

        do
        {
            _transform.position = _original;

            x = UnityEngine.Random.Range(-1f, 1f) * _actualMagnitude * _actualDuration;
            y = UnityEngine.Random.Range(-1f, 1f) * _actualMagnitude * _actualDuration;

            _transform.position += new Vector3(x, y, 0f);

            yield return null;

            _actualDuration -= Time.deltaTime;
        }
        while (_actualDuration > 0);

        _actualDuration = 0f;
        _actualMagnitude = 0f;
    }
}
