using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ObjectShaker : MonoBehaviour
{
    private RectTransform _transform;
    private float _actualDuration = 0f;
    private float _actualMagnitude = 0f;
    private Vector3 _original;

    private Coroutine _shaking;
    private System.Random _rng;

    private void Start()
    {
        _transform = GetComponent<RectTransform>();

        _original = _transform.anchoredPosition;

        int seed = unchecked(Environment.TickCount ^ GetInstanceID());
        _rng = new System.Random(seed);
    }

    public void Shake(float duration = 0f, float magnitude = 0f)
    {
        _actualDuration = duration;
        _actualMagnitude = magnitude;

        Debug.Log("Set actuals as dur: " + _actualDuration + " and mag: " + _actualMagnitude);

        if (_shaking != null)
            StopCoroutine(_shaking);
        _shaking = StartCoroutine(StartShaker());
    }

    private IEnumerator StartShaker()
    {
        if (_transform == null)
            yield break; 

        float x;
        float y;

        do
        {
            _transform.anchoredPosition = _original;

            float rx = (float)(_rng.NextDouble() * 2.0 - 1.0);
            float ry = (float)(_rng.NextDouble() * 2.0 - 1.0);

            x = rx * _actualMagnitude * _actualDuration;
            y = ry * _actualMagnitude * _actualDuration;

            _transform.anchoredPosition += new Vector2(x, y);

            yield return null;

            _actualDuration -= Time.deltaTime;
        }
        while (_actualDuration > 0);

        _actualDuration = 0f;
        _actualMagnitude = 0f;

        _transform.anchoredPosition = _original;
    }
}
