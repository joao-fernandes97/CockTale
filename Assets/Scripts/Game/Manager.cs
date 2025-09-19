using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    // debugs
    [SerializeField] private Transform _ingredients;

    [SerializeField] private DrinkView _drink;
    [SerializeField] private EndMenu _end;
    [SerializeField] private Timer _timer;
    [SerializeField] private Shaker _shaker;
    [SerializeField] private Clock _clock;
    [SerializeField] private Particle _particles;
    [SerializeField] private float _pourTime = 1f;

    [SerializeField] private float _ingredientInterval = 0.2f;
    [SerializeField] private float _ingredientFlashTme = 0.8f;
    [SerializeField] private float _waitTime = 1f;

    private Drink _currentDrink;
    private Queue<Ingredient> _currentMix;
    public Ingredient[] Mix => _currentMix.ToArray();

    private bool _completedShaking = false;

    private float _pour = 0f;

    private void Start()
    {
        _end.gameObject.SetActive(false);
        for (int i = 0; i < _ingredients.childCount; i++)
        {
            GameObject obj = _ingredients.GetChild(i).gameObject;
            obj.SetActive(false);
        }
    }

    public void OnEnable()
    {
        _shaker.OnEnable();

        _currentDrink = null;
        _currentMix = new();
        _completedShaking = false;

        Debug.Log("New Drink comp. ");
    }

    private void LateUpdate()
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
        InputManager.ResetDrink(); //forcibly reset drink back to null
        Drink drink;
        do
        {
            drink = InputManager.CurrentDrink();
            if (drink != null) Debug.Log(drink);
            yield return null;
            // Debug.Log("Ahoo drink: " + drink?.name);
        }
        while (drink == null);

        drink.ChooseCharacter();
        _drink.SetUp(drink);

        WaitForSecondsRealtime interval = new(_ingredientInterval);
        WaitForSecondsRealtime flash = new(_ingredientFlashTme);
        WaitForSecondsRealtime wait = new(_waitTime);

        // send signals to arduino to light up each ingredient
        for (int i = 0;  i < drink.Recipe.Length; i++)
        {
            GameObject obj = _ingredients.GetChild(i).gameObject;
            Image comp = obj.GetComponent<Image>();
            comp.sprite = drink.Recipe[i].Sprite;
            obj.SetActive(true);
            // Debug.Log("Ahoo drink: " + ingredient?.name);

            yield return interval;
        }

        yield return wait;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < _ingredients.childCount; i++)
            {
                Image img = _ingredients.GetChild(i).gameObject.GetComponent<Image>();
                img.color = Color.clear;
                Debug.Log("ing Clear color from: " + _ingredients.GetChild(i).gameObject.name);
            }

            yield return flash;

            for (int i = 0; i < _ingredients.childCount; i++)
            {
                Image img = _ingredients.GetChild(i).gameObject.GetComponent<Image>();
                img.color = Color.white;
                Debug.Log("ing White color from: " + _ingredients.GetChild(i).gameObject.name);
            }

            yield return flash;
            Debug.Log("ing Clear/White color from ing. ");
        }
        
        for (int i = 0; i < _ingredients.childCount; i++)
        {
            GameObject obj = _ingredients.GetChild(i).gameObject;
            obj.SetActive(false);
        }

        // Debug.Log("Ahoo");
        _currentDrink = drink;
        _cor = null;

        _clock.StopTime(false);
    }

    private void MixDrink()
    {
        if (_currentDrink == null)
        {
            if (_pour > 0f)
            {
                _pour -= Time.unscaledDeltaTime * 2;
                _pour = Mathf.Clamp(_pour, 0f, _pourTime);
                _shaker.Pour(_pour, _pourTime*2);
            }
            return;
        }

        // get pour inputs
            if (_completedShaking
                && (InputManager._instance.usingSensors ? InputManager.SensorPouring() : InputManager.Pouring()))
            {
                // Debug.Log("Pouring. ");
                // needs 1 second of pouring to complete
                _pour += Time.deltaTime;

                // when finished pouring
                if (_pour > _pourTime)
                {
                    bool won = _end.ServeDrink(_currentDrink, _currentMix.ToArray());
                    _drink.End(won);
                    _timer.End(won);
                    OnEnable();
                }

                _pour = Mathf.Clamp(_pour, 0f, _pourTime);
                _shaker.Pour(_pour, _pourTime);
                return;
            }
            else
            {
                _pour -= Time.deltaTime * 2;
            }

        _pour = Mathf.Clamp(_pour, 0f, _pourTime);
        _shaker.Pour(_pour, _pourTime);

        // get shake inputs
        if (_currentMix.Count > 0
            && (InputManager._instance.usingSensors ? InputManager.SensorShaking(out float dif) : InputManager.Shaking(out dif)))
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
                Debug.Log("Completed Shake. ");
            }
            return;
        }

        // get ingredient inputs
        if (InputManager._instance.usingSensors ? InputManager.CupTilted(out Ingredient ingredient)
                                                : InputManager.PouringIngredient(out ingredient))
            {
                Debug.Log("Pouring Ingredient: "+ ingredient.name);
                _currentMix.Enqueue(ingredient);
                _shaker.Remap(_currentMix.Count, _currentMix.Count + 1);
                _particles.EmitIngredient(ingredient);
            }
    }
}
