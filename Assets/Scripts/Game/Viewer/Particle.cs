using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    [SerializeField] private ParticleSystem _particleSystem;

    private ParticleSystem.MainModule _main;
    private ParticleSystemRenderer _renderer;

    private void Awake()
    {
        _renderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
        _main = _particleSystem.main;
        _main.stopAction = ParticleSystemStopAction.None;
    }

    private void Start()
    {
        // no auto emission
        _particleSystem.Stop();
    }

    public void EmitIngredient(Ingredient ingredient)
    {
        if (ingredient == null) return;

        // _particleSystem.gameObject.SetActive(true);
        _particleSystem.Play();

        _main.startColor = ingredient.Color;
        Material mat = _renderer.material; ;

        // URP uses _BaseMap, and built in uses _MainTex
        if (mat.HasProperty("_BaseMap"))
            mat.SetTexture("_BaseMap", ingredient.Sprite.texture);
        else
            mat.mainTexture = ingredient.Sprite.texture;

        StartCoroutine(Emit());

        Debug.Log("Emiting. ");
    }

    private IEnumerator Emit()
    {
        yield return _waitForSeconds0_5;
        _particleSystem.Stop();
    }
}
