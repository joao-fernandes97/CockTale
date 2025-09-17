using System.Collections;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private ObjectShaker[] _shakingObject;
    [SerializeField] private RectTransform _shake;
    [SerializeField] private float _shakeStrength = 0.1f;

    [SerializeField] private float _duration = 0f;
    [SerializeField] private float _magnitude = 0f;

    [SerializeField] private Animator _animator;
    [SerializeField] private float _blendInterval = 0.1f;

    public void OnEnable()
    {
        SetShaker(0f);
    }
    
    public void ModifyShaker(float dif, int count) // camera should actually shake when this is called
    {
        // Debug.Log("new local scale " + dif);

        float newDif = _shakeStrength * dif / (count + 1);

        float d = _duration * Mathf.Min(0.01f, newDif) *10f;
        float m = _magnitude * Mathf.Min(0.01f, newDif) * 100f;

        foreach (ObjectShaker obj in _shakingObject)
            obj?.Shake(d, m);

        Shake(d);

        SetShaker(_shake.localScale.y + newDif);
    }

    private Coroutine _shaking;
    private float _timer;

    public void Shake(float duration = 0f)
    {
        _timer = duration + _blendInterval;

        _shaking ??= StartCoroutine(StartShaker());
    }

    private IEnumerator StartShaker()
    {
        float timer = _blendInterval/2;
        float value;

        do
        {
            value = Mathf.InverseLerp(_blendInterval, 0f, timer);
            _animator.SetFloat("IdleShake", value);
            yield return null;

            timer -= Time.deltaTime;
        }
        while (timer > 0);
        _animator.SetFloat("IdleShake", 1f);

        do
        {
            yield return null;
            _timer -= Time.deltaTime;
        } while (_timer > 0);

        timer = _blendInterval;
        do
        {
            value = Mathf.InverseLerp(0f, _blendInterval, timer);
            _animator.SetFloat("IdleShake", value);
            yield return null;

            timer -= Time.deltaTime;
        }
        while (timer > 0);

        _animator.SetFloat("IdleShake", 0f);
        _timer = 0f;
        _shaking = null;
    }

    public void SetShaker(float newValue)
    {
        Vector2 scale = _shake.localScale;
        scale.y = newValue;
        _shake.localScale = scale;
    }

    public float GetShaker() => _shake.localScale.y;

    public void Remap(int oldMax, int newMax)
    {
        float newValue = _shake.localScale.y;

        if (newMax > 0)
            newValue = _shake.localScale.y * oldMax / newMax;

        SetShaker(newValue);
    }
}
