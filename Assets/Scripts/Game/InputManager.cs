using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction shake;
    [SerializeField] private InputAction pour;
    private static readonly Key[] DigitKeys = {
        Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
        Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9,
        Key.Q, Key.W, Key.E, Key.R, Key.T,
        Key.Y, Key.U, Key.I, Key.O, Key.P
    };

    private static InputManager _instance;

    private static List<Ingredient> _ingredients;
    private static List<Drink> _drinks;

    private void Awake()
    {
        _instance = this;

        _ingredients = Resources.LoadAll<Ingredient>("").ToList();
        _drinks = Resources.LoadAll<Drink>("").ToList();

        shake = new InputAction(name: "Shake", type: InputActionType.Value, expectedControlType: "Axis");
        var comp = shake.AddCompositeBinding("1DAxis");
        comp.With("negative", "<Keyboard>/w");
        comp.With("positive", "<Keyboard>/s");

        pour = new InputAction(name: "Pour", type: InputActionType.Button);
        pour.AddBinding("<Keyboard>/x").WithInteraction("press");
    }

    private void OnEnable()
    {
        if (shake != null) shake.Enable();
        if (pour  != null) pour.Enable();
    }

    private void OnDisable()
    {
        if (shake != null) shake.Disable();
        if (pour  != null) pour.Disable();
    }

    private static float _previous = 0f;
    public static bool Shaking(out float dif)
    {
        dif = 0f;

        float current = 0f;
        if (_instance?.shake != null)
            current = _instance.shake.ReadValue<float>();
        
        bool flipped = (current > 0f && _previous <= 0f) || (current < 0f && _previous >= 0f);

        if (flipped && current != 0f)
        {
            dif = Mathf.Abs(current) + Mathf.Abs(_previous);
            _previous = current;
            return true;
        }

        _previous = current;
        return false;
    }


    /// <summary>
    /// you can access each ingredient's color in their scriptable objects
    /// </summary>
    public static bool PouringIngredient(out Ingredient ingredient)
    {
        ingredient = null;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return false;
        
        // Map number keys to ingredient indices: 0..9
        for (int i = 0; i < _ingredients.Count && i < 10; i++)
        {
            bool pressedRow = keyboard[DigitKeys[i]].wasPressedThisFrame;
            if (pressedRow)
            {
                ingredient = _ingredients[i];
                return true;
            }
        }

        return false;
    }

    public static bool Pouring()
    {
        if (_instance?.pour == null)
            return false;

        if (_instance.pour.ReadValue<float>() > 0f)
            return true;
        // if not tilting
        return false;
    }

    public static Drink CurrentDrink()
    {
        // somewhere in color map and custom inputs getting all ingredient colors using links, and mapping them from that
        return _drinks[Random.Range(0, _drinks.Count)];
    }
}
