using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public string Line { get; private set; }

    [field: SerializeField] public Drink FavoriteDrink { get; private set; }
    [field: SerializeField] public string FavoriteLine { get; private set; }
}
