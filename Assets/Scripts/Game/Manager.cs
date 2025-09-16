using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // debugs
    [SerializeField] private Image _debugIngredient;

    [SerializeField] private DrinkView _drink;
    [SerializeField] private EndMenu _end;
    [SerializeField] private Timer _timer;
    [SerializeField] private Shaker _shaker;
    [SerializeField] private Clock _clock;
    [SerializeField] private Particle _particles;

    [SerializeField] private float _ingredientInterval = 0.2f;
    [SerializeField] private float _ingredientFlashTme = 0.8f;

    private Drink _currentDrink;
    private Queue<Ingredient> _currentMix;
    public Ingredient[] Mix => _currentMix.ToArray();

    private bool _completedShaking = false;

    private float _pour = 0f;

    private void Start()
    {
        _end.gameObject.SetActive(false);
    }
    
    public void OnEnable()
    {
        _shaker.OnEnable();

        _currentDrink = null;
        _currentMix = new();
        _completedShaking = false;
        _pour = 0f;
    }

    private void Update()
    {

        SpinWheel();
        MixDrink();

        if (_timer.Time < 0f)
        {
            // stop game and show end screen
            _end.gameObject.SetActive(true);
        }
    }

    private Coroutine _cor;
    public void SpinWheel()
    {
        if (_currentDrink != null) return;

        // call enumerator to wait until it has a stable color and then send signals back to arduino for each ingredient
        _cor ??= StartCoroutine(GenerateDrink());
    }

    private IEnumerator GenerateDrink()
    {
        _clock.StopTime(true);
        Debug.Log("Generating new drink. ");

        Drink drink;
        do
        {
            drink = InputManager.CurrentDrink();
            yield return null;
            // Debug.Log("Ahoo drink: " + drink?.name);
        }
        while (drink == null);

        drink.ChooseCharacter();
        _drink.SetUp(drink);

        WaitForSecondsRealtime interval = new(_ingredientInterval);
        WaitForSecondsRealtime flash = new(_ingredientFlashTme);

        // send signals to arduino to light up each ingredient
        foreach (Ingredient ingredient in drink.Recipe)
        {
            // Debug.Log("Ahoo drink: " + ingredient?.name);

            yield return interval;
            // turn on ingredient for arduino
            _debugIngredient.color = ingredient.Color;

            yield return flash;
            // turn off ingredient for arduino
            _debugIngredient.color = Color.clear;
        }

        // Debug.Log("Ahoo");
        _currentDrink = drink;
        _cor = null;

        _clock.StopTime(false);
    }

    private void MixDrink()
    {
        if (_currentDrink == null) return;

        // get pour inputs
        if (_completedShaking && InputManager.Pouring())
        {
            // Debug.Log("Pouring. ");
            // needs 1 second of pouring to complete
            _pour += Time.deltaTime;

            // when finished pouring
            if (_pour > 0.5f)
            {
                bool won = _end.ServeDrink(_currentDrink, _currentMix.ToArray());
                _drink.End(won);
                OnEnable();
            }
            return;
        }
        else
            _pour = 0f;

        // get shake inputs
        if (_currentMix.Count > 0 && InputManager.Shaking(out float dif))
        {
            // Debug.Log("Shaking. ");
            // shaking meter

            _end.Shake();
            _shaker.ModifyShaker(dif, _currentMix.Count);

            // when finished shaking
            if (_shaker.GetShaker() >= 1f)
            {
                _shaker.SetShaker(1f);
                _completedShaking = true;
            }
            return;
        }

        // get ingredient inputs
        if (InputManager.PouringIngredient(out Ingredient ingredient))
        {
            // Debug.Log("Pouring Ingredient. ");
            _currentMix.Enqueue(ingredient);
            _shaker.Remap(_currentMix.Count, _currentMix.Count + 1);
            _particles.EmitIngredient(ingredient);
        }
    }
}
