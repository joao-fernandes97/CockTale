using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private Particle _particle;
    [SerializeField] private Manager _manager;
    [SerializeField] private ParticleSystem _splash;
    private ParticleSystem.MainModule _main;

    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponentInChildren<ParticleSystem>(true);

        if (ps != null && ps == _splash)
        {
            _main = ps.main;
            _main.startColor = GetColor();
            ps.Play();
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

        Debug.Log("Color: " + color);

        return color;
    }
}
