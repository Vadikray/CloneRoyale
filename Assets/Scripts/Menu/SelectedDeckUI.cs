using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedDeckUI : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager; 
    [SerializeField] private Image[] _images;
    
    public void UpdateCardsList(IReadOnlyList<Card> cards)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            if (i < cards.Count)
            {
                _images[i].sprite = cards[i].sprite;
                _images[i].enabled = true;
            }
            else
            {
                _images[i].sprite = null;
                _images[i].enabled = false;
            }
        } 
    }
}
