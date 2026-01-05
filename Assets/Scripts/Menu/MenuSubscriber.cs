using System;
using UnityEngine;

public class MenuSubscriber : MonoBehaviour
{
    [SerializeField] private DeckManager _deckManager;
    [SerializeField] private SelectedDeckUI _selectedDeckUI;
    [SerializeField] private SelectedDeckUI _selectedDeckUI2;
    [SerializeField] private SelectedDeckUI _selectedDeckUIMatchmaking;
    [SerializeField] private AvailableDeckUI _availableDeckUI;
    [SerializeField] private MatchmakingManager _matchmakingManager;

    private void Start()
    {
        _deckManager.UpdateSelected += _selectedDeckUI.UpdateCardsList;
        _deckManager.UpdateSelected += _selectedDeckUI2.UpdateCardsList;
        _deckManager.UpdateSelected += _selectedDeckUIMatchmaking.UpdateCardsList;
        _deckManager.UpdateAvailable += _availableDeckUI.UpdateCardsList;

        _matchmakingManager.Subscribe();
    }

    private void OnDestroy()
    {
        _deckManager.UpdateSelected -= _selectedDeckUI.UpdateCardsList;
        _deckManager.UpdateSelected -= _selectedDeckUI2.UpdateCardsList;
        _deckManager.UpdateSelected -= _selectedDeckUIMatchmaking.UpdateCardsList;
        _deckManager.UpdateAvailable -= _availableDeckUI.UpdateCardsList;
        
        _matchmakingManager.Unsubscribe();
    }
}
