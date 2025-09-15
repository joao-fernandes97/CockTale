using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private ObjectShaker[] _shakingObject;
    [SerializeField] private RectTransform _shake;
    [SerializeField] private float _shakeStrength = 0.1f;

    public void OnEnable()
    {
        SetShaker(0f);
    }
    public void ModifyShaker(float dif, int count) // camera should actually shake when this is called
    {
        // Debug.Log("new local scale " + dif);

        float newDif = _shakeStrength * dif / (count + 1);

        foreach (ObjectShaker obj in _shakingObject)
            obj?.Shake(newDif, newDif);

        SetShaker(_shake.localScale.x +newDif);
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
