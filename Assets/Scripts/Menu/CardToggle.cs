using UnityEngine;

public class CardToggle : MonoBehaviour
{
    [SerializeField] private CardSelecter _selecter;
    [SerializeField] private int _index;

    public void Click(bool value)
    {
        if(value == false) return;
        _selecter.SetSelectToggleIndex(_index);
    }
}