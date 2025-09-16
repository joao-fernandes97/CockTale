using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkView : MonoBehaviour
{
    [SerializeField] private TMP_Text _drinkTMP;
    [SerializeField] private Image _drink;
    [SerializeField] private Sprite _messedUp;

    [SerializeField] private Image _character;
    [SerializeField] private TMP_Text _characterLine;

    [SerializeField] private Animator _animator;

    public void SetUp(Drink current)
    {
        _drink.sprite = current.Sprite;
        _drinkTMP.text = current.name;
        
        Character car = current.Character;

        if (car != null)
        {
            _character.sprite = car.Sprite;
            _character.rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal, car.Sprite.rect.width
                );

            _characterLine.text = car.name + ": ";

            if (car.FavoriteDrink == current)
                _characterLine.text += car.FavoriteLine;
            else
                _characterLine.text += car.Line;
        }

        Debug.Log("Ordering Drink." );
        _animator.SetTrigger("OrderDrink"); // character showing up and ordering drink, label coming down
    }
    public void End(bool won)
    {
        if (!won && _messedUp != null)
            _drink.sprite = _messedUp;

        Debug.Log("Rolling up." );
        _animator.SetTrigger("RollUp"); // Drink has been ordered, roll it on the table and make character/label disappear, resets to default state
    }
}