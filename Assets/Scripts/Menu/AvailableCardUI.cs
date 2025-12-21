using UnityEngine;
using UnityEngine.UI;

public class AvailableCardUI : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _lockedColor;
    private CardStateType _currentState = CardStateType.None;

    [SerializeField] private CardSelecter _selecter;
    [SerializeField] private int _id;

    #region Editor

#if UNITY_EDITOR
    [SerializeField] private Image _image;

    public void Create(CardSelecter selecter, Card card, int id)
    {
        _selecter = selecter;
        _id = id;
        _image.sprite = card.sprite;
        _text.text = card.name;
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    #endregion

    public void SetState(CardStateType state)
    {
        _currentState = state;
        switch (state)
        {
            case CardStateType.None:
                break;
            case CardStateType.Available:
                _text.color = _availableColor;
                break;
            case CardStateType.Selected:
                _text.color = _selectedColor;
                break;
            case CardStateType.Locked:
                _text.color = _lockedColor;
                break;
            default:
                break;
        }
    }

    public void Click()
    {
        switch (_currentState)
        {
            case CardStateType.Available:
                _selecter.SelectCard(_id);
                SetState(CardStateType.Selected);
                break;
            case CardStateType.Selected:
                break;
            case CardStateType.Locked:
                break;
            default:
                break;
        }
    }

    public enum CardStateType
    {
        None = 0,
        Available = 1,
        Selected = 2,
        Locked = 3
    }
}