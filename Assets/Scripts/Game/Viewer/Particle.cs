using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    [field:SerializeField] public ParticleSystem System { get; private set; }

    private ParticleSystemRenderer _renderer;

    private void Awake()
    {
        _renderer = System.GetComponent<ParticleSystemRenderer>();
    }

    private void Start()
    {
        System.gameObject.SetActive(true);
        
        // no auto emission
        System.Stop();
    }

    public void EmitIngredient(Ingredient ingredient)
    {
        if (ingredient == null) return;

        // System.gameObject.SetActive(true);
        System.Play();

        //_main.startColor = ingredient.Color;
        Material mat = _renderer.material; ;

        // URP uses _BaseMap, and built in uses _MainTex
        if (mat.HasProperty("_BaseMap"))
            mat.SetTexture("_BaseMap", ingredient.ParticleSprite.texture);
        else
            mat.mainTexture = ingredient.ParticleSprite.texture;

        StartCoroutine(Emit());

        Debug.Log("Emiting. ");
    }

    private IEnumerator Emit()
    {
        yield return _waitForSeconds0_5;
        System.Stop();
    }
}
