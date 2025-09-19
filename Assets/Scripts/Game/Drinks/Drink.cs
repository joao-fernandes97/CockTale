using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Scriptable Objects/Drink")]
public class Drink : ScriptableObject
{
    [field: SerializeField] public Ingredient[] Recipe { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public NamedColor namedColor { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] private List<Character> _likedBy;
    public Character Character { get; private set; }

    public static List<Character> _used;

    public void ChooseCharacter()
    {
        _likedBy ??= Resources.LoadAll<Character>("").ToList();
        _used ??= Resources.LoadAll<Character>("").ToList();

        _likedBy = _likedBy.OrderBy(b => _used.IndexOf(b)).ToList();

        if (_likedBy.Count > 0)
            Character = _likedBy[InverseCDF(_likedBy.Count)];

        // just for debug
        /*for (int i = 0; i < 12; i++)
        {
            _likedBy = _likedBy.OrderBy(b => _used.IndexOf(b)).ToList();

            int index = InverseCDF(_likedBy.Count);

            Character = _likedBy[index];

            Debug.Log("Chose character: " + Character.name + "   index: " + index + "   last: " + _used.ElementAt(_used.Count - 1) + "   first: " + _used.ElementAt(0));
            
            _used.Remove(Character);
            _used.Add(Character);
        }*/

        if (Character != null)
        {
            _used.Remove(Character);
            _used.Add(Character);
        }
    }

    public static int InverseCDF(int range)
    {
        float u = Random.Range(0f, 1f);
        return Mathf.FloorToInt(range - range * Mathf.Sqrt(1 - u));
    }

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
    private void OnDisable()
    {
        Recipe = Recipe.Where(x => x != null).ToArray();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Drink))]
public class DrinkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Drink drink = (Drink)target;
        if (drink.Sprite == null) return;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

        float inspectorWidth = EditorGUIUtility.currentViewWidth;

        // scale preview
        float previewSize = inspectorWidth / 2;
        Rect rect = GUILayoutUtility.GetRect(previewSize, previewSize, GUILayout.ExpandWidth(false));

        // center preview
        rect.x = (inspectorWidth - previewSize) * 0.5f;


        Color prev = GUI.color;

        // apply drink color and sprite
        GUI.color = drink.Color;
        GUI.DrawTexture(rect, AssetPreview.GetAssetPreview(drink.Sprite), ScaleMode.ScaleToFit);

        GUI.color = prev;
    }
}
#endif