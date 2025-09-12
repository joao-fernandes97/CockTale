using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [field: SerializeField] public IngredientType Type { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public GameObject ParticleSytem { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Color.a != 1f)
        {
            Color current = Color;
            current.a = 1f;
            Color = current;
        }
    }
#endif

    public enum IngredientType
    {
        Spirit,
        Beverage,
        Complement,
        Syrup
    }
}
