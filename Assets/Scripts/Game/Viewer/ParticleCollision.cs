using UnityEngine;
using UnityEngine.Events;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private Particle _particle;
    [SerializeField] private Manager _manager;
    [SerializeField] private ParticleSystem _splash;
    [SerializeField] private UnityEvent _plop;
    private ParticleSystem.MainModule _main;
    public static Color Color { get; private set; }

    private void Start()
    {
        _splash.gameObject.SetActive(true);
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponentInChildren<ParticleSystem>(true);

        if (ps != null && ps == _splash)
        {
            _main = ps.main;
            _main.startColor = GetColor();
            ps.Play();
            _plop.Invoke();
        }
    }

    public Color GetColor()
    {
        if (_manager.Mix.Length <= 0)
            return Color.white;
        
        Color color = Color.black;

        foreach (Ingredient ing in _manager.Mix)
            if (ing != null)
                color += ing.Color;
        
        color /= _manager.Mix.Length;

        //Debug.Log("Color: " + color);

        Color = color;

        return color;
    }
}
