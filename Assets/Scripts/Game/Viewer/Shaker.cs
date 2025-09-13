using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private RectTransform _shake;
    [SerializeField] private float _shakeStrength = 0.1f;

    public void OnEnable()
    {
        SetShaker(0f);
    }
    public void ModifyShaker(float dif, int count) // camera should actually shake when this is called
    {
        Debug.Log("new local scale " + dif);

        float newValue = _shake.localScale.x + _shakeStrength * dif / (count + 1);

        SetShaker(newValue);
    }

    public void SetShaker(float newValue)
    {
        Vector2 scale = _shake.localScale;
        scale.x = newValue;
        _shake.localScale = scale;
    }
    public float GetShaker() => _shake.localScale.x;

    public void Remap(int oldMax, int newMax)
    {
        float newValue = _shake.localScale.x;

        if (newMax > 0)
            newValue = _shake.localScale.x * oldMax / newMax;

        SetShaker(newValue);
    }
}
