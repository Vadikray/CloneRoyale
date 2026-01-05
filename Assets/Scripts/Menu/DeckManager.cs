using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private GameObject _lockScreenCanvas;
    [SerializeField] private Card[] _cards;
    [SerializeField] private List<Card> _availableCards = new List<Card>();
    [SerializeField] private List<Card> _selectedCards = new List<Card>();
    public event Action<IReadOnlyList<Card>, IReadOnlyList<Card>> UpdateAvailable;
    public event Action<IReadOnlyList<Card>> UpdateSelected;

    public IReadOnlyList<Card> AvailableCards
    {
        get { return _availableCards; }
    }

    public IReadOnlyList<Card> SelectedCards
    {
        get { return _selectedCards; }
    }

    #region Editor

#if UNITY_EDITOR
    [SerializeField] private AvailableDeckUI _availableDeckUI;
    private void OnValidate()
    {
        _availableDeckUI.SetAllCardsCount(_cards);
    }
#endif

    #endregion


    public void Init(List<int> availableCardIndexes, int[] selectedCardIndexes)
    {
        for (int i = 0; i < availableCardIndexes.Count; i++)
        {
            _availableCards.Add(_cards[availableCardIndexes[i]]);
        }

        for (int i = 0; i < selectedCardIndexes.Length; i++)
        {
            _selectedCards.Add(_cards[selectedCardIndexes[i]]);
        }

        UpdateAvailable?.Invoke(AvailableCards, SelectedCards);
        UpdateSelected?.Invoke(SelectedCards);

        _lockScreenCanvas.SetActive(false);
    }

    public void ChangesDeck(IReadOnlyList<Card> selectedCards, Action success)
    {
        _lockScreenCanvas.SetActive(true);
        int[] IDs = new int[selectedCards.Count];
        for (int i = 0; i < selectedCards.Count; i++)
        {
            IDs[i] = selectedCards[i].id;
        }

        string json = JsonUtility.ToJson(new Wrapper(IDs));
        string uri = URLLibrary.MAIN + URLLibrary.SETSELECTDECK;
        Dictionary<string, string> data = new Dictionary<string, string>
        {
            {"userID", UserInfo.Instance.ID.ToString()},
            {"json", json}
        };

        success += () =>
        {
            for (int i = 0; i < _selectedCards.Count; i++)
            {
                _selectedCards[i] = selectedCards[i];
            }

            UpdateSelected?.Invoke(SelectedCards);
        };
        Network.Instance.Post(uri, data, (s) => SendSuccess(s, success), Error);
    }

    private void SendSuccess(string obj, Action success)
    {
        if (obj != "ok")
        {
            Error(obj);
            return;
        }

        success?.Invoke();
        _lockScreenCanvas.SetActive(false);
    }

    private void Error(string obj)
    {
        Debug.LogError("Неудачная попытка отправки новой колоды: " + obj);
        _lockScreenCanvas.SetActive(false);
    }


    [System.Serializable]
    private class Wrapper
    {
        public int[] IDs;

        public Wrapper(int[] IDs)
        {
            this.IDs = IDs;
        }
    }

    public bool TryGetDeck(string[] cardsIDs, out Dictionary<string, Card> deck)
    {
        deck = new Dictionary<string, Card>();
        for (int i = 0; i < cardsIDs.Length; i++)
        {
            if (int.TryParse(cardsIDs[i], out int id) == false || id == 0) return false;
            Card card = _cards.FirstOrDefault(c => c.id == id);
            if (card == null) return false;

            deck.Add(cardsIDs[i], card);
        }

        return true;
    }
}

[System.Serializable]
public class Card
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public int id { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }
    [field: SerializeField] public Unit unit { get; private set; }

    [field: SerializeField] public GameObject galagram { get; private set; }
}