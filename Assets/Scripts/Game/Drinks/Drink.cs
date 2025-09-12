using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Scriptable Objects/Drink")]
public class Drink : ScriptableObject
{
    [field: SerializeField] public Ingredient[] Recipe { get; private set; }
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public Sprite Sprite { get; private set; }

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