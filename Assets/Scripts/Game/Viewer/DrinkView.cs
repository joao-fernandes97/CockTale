using System.Collections;
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
    [SerializeField] private float _characterY = 0f;
    [SerializeField] private RectTransform _table;
    [SerializeField] private float _horizontalScale = 1f;

    [SerializeField] private Animator _animator;

    private void Start()
    {
        // order it at beginning do later table logic works
        _character.rectTransform.SetSiblingIndex( 0 );
    }

    public void SetUp(Drink current)
    {
        StartCoroutine(SetUpRoutine(current));
        _animator.SetTrigger("OrderDrink"); // character showing up and ordering drink, label coming down
    }

    private IEnumerator SetUpRoutine(Drink current)
    {
        AnimatorStateInfo s;
        do
        {
            s = _animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        } while ( !s.IsTag("OrderDrink") );

        _drink.sprite = current.Sprite;
        _drink.color = current.Color;
        _drinkTMP.text = current.name;
        
        Character car = current.Character;

        if (car != null)
        {
            RectTransform rect = _character.rectTransform;

            _character.sprite = car.Sprite;
            Sprite sprite = car.Sprite;
            

            // choose a width from sprite aspect
            float w = sprite.rect.width * _horizontalScale;
            float h = w * (sprite.rect.height / sprite.rect.width);

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   h);

            // align sprite pivot to a baseline using the final height
            float spritePivotNormY = sprite.pivot.y / sprite.rect.height;
            float y = _characterY - (spritePivotNormY - rect.pivot.y) * h;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, y);

            _table.SetSiblingIndex( car.Order );

            _characterLine.text = car.name + ": " +
                ((car.FavoriteDrink == current) ? car.FavoriteLine : car.Line);
        }

        Debug.Log("Ordering Drink." );
    }
    
    public void End(bool won)
    {
        if (!won && _messedUp != null)
            _drink.sprite = _messedUp;

        Debug.Log("Rolling up.");
        _animator.SetTrigger("RollUp"); // Drink has been ordered, roll it on the table and make character/label disappear, resets to default state
    }
}