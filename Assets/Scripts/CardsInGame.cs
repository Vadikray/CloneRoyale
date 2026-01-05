using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class CardsInGame : MonoBehaviour
{
    public static CardsInGame Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ReadOnlyDictionary<string, Card> _playerDeck { get; private set; }
    public ReadOnlyDictionary<string, Card> _enemyDeck { get; private set; }

    public void SetDecks(string[] playerCards, string[] enemyCards)
    {
        DeckManager deckManager = FindFirstObjectByType<DeckManager>();
        bool player = deckManager.TryGetDeck(playerCards, out Dictionary<string, Card> playerDeck);
        bool enemy = deckManager.TryGetDeck(enemyCards, out Dictionary<string, Card> enemyDeck);

        if (player == false || enemy == false)
        {
            Debug.Log($"Не удалось загрузить какую-то колоду player = {player} | enemy = {enemy}");
        }

        _playerDeck = new ReadOnlyDictionary<string, Card>(playerDeck);
        _enemyDeck = new ReadOnlyDictionary<string, Card>(enemyDeck);
    }

    public List<string> GetAllID() => _playerDeck.Keys.ToList();
}