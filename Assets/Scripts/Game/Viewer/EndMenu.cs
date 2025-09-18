using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private SceneAsset _Menu;
    [SerializeField] private Clock _clock;

    [SerializeField] private TMP_Text _scoreTMP;
    [SerializeField] private TMP_Text _correctServedTMP;
    [SerializeField] private TMP_Text _totalServedTMP;
    [SerializeField] private TMP_Text _kdTMP;
    [SerializeField] private TMP_Text _totalShakingTMP;
    [SerializeField] private TMP_Text _averageShakingTMP;
    [SerializeField] private TMP_Text _bonusPointsTMP;

    private int _drinksServed;
    private float _totalShakeTime;
    private int _correctDrinks;
    private int _score;
    private int _bonus;

    private float _shakeTime;

    private void Start()
    {
        _drinksServed = 0;
        _correctDrinks = 0;
        _totalShakeTime = 0f;
        _score = 0;
        _shakeTime = 0f;
        _bonus = 0;
    }

    public bool ServeDrink(Drink drink, Ingredient[] mix)
    {
        _drinksServed++;
        _totalShakeTime += _shakeTime;
        _shakeTime = 0f;
        int correct = 0;

        for (int i = 0; i < mix.Length; i++)
        {
            if (drink.Recipe.Length > i) break;

            if (mix[i] == drink.Recipe[i])
                correct++;
            else
                break;
        }

        bool won = false;

        if (correct == drink.Recipe.Length && mix.Length == drink.Recipe.Length)
        {
            _correctDrinks++;
            _score += correct;

            won = true;
        }
        _score += correct / 3;

        if (drink.Character.FavoriteDrink == drink)
        {
            _bonus += 1;
            _score *= 2;
        }

        return won;
    }

    public void Shake()
    {
        _shakeTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        _scoreTMP.text = "Final Score: " + _score.ToString();
        _correctServedTMP.text = "Correct Drinks Served: " + _correctDrinks.ToString();
        _totalServedTMP.text = "Total Drinks Served: " + _drinksServed.ToString();
        _kdTMP.text = "Served K/D: " + (_drinksServed / (_correctDrinks +1)).ToString();
        _totalShakingTMP.text = "Total Shaking Time: " + _totalShakeTime.ToString();
        _averageShakingTMP.text = "Average Shaking Time: " + (_totalShakeTime / _drinksServed +1).ToString();
        _bonusPointsTMP.text = "Bonus Times: " + _bonus.ToString();

        _clock.StopTime(true);
    }
    private void OnDisable()
    {
        _clock.StopTime(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_Menu.name);
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
