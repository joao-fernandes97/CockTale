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
    private System.Random _rng;

    private void Start()
    {
        _original = _transform.localPosition;

        int seed = unchecked(Environment.TickCount ^ GetInstanceID());
        _rng = new System.Random(seed);
    }

    public void Shake(float duration = 0f, float magnitude = 0f)
    {
        _actualDuration = _duration;
        _actualMagnitude = _magnitude;

        if (duration != 0f)
            _actualDuration *= duration;
        if (magnitude != 0f)
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
            _transform.localPosition = _original;

            float rx = (float)(_rng.NextDouble() * 2.0 - 1.0);
            float ry = (float)(_rng.NextDouble() * 2.0 - 1.0);

            x = rx * _actualMagnitude * _actualDuration;
            y = ry * _actualMagnitude * _actualDuration;

            _transform.localPosition += new Vector3(x, y, 0f);

            yield return null;

            _actualDuration -= Time.deltaTime;
        }
        while (_actualDuration > 0);

        _actualDuration = 0f;
        _actualMagnitude = 0f;

        _transform.localPosition = _original;
    }
}
