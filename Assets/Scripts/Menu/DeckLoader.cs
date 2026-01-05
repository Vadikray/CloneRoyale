using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private DeckManager _manager;
    [SerializeField]private List<int> _availableCards = new List<int>();
    [SerializeField]private int[] _selectedCards = new int[5];

    private void Start()
    {
        StartLoad();
    }

    private void StartLoad()
    {
        Network.Instance.Post(
            URLLibrary.MAIN + URLLibrary.GETDECKINFO,
            new Dictionary<string, string> {{"userID", UserInfo.Instance.ID.ToString()}},
            SuccessLoad,
            ErrorLoad);
    }

    private void ErrorLoad(string error)
    {
        Debug.LogError(error);
        StartLoad();
    }
    

    private void SuccessLoad(string data)
    {
        DeckData deckData = JsonUtility.FromJson<DeckData>(data);
        _selectedCards = new int[deckData.selectedIDs.Length];
        for (int i = 0; i < _selectedCards.Length; i++)
        {
            int.TryParse(deckData.selectedIDs[i], out _selectedCards[i]);
        }

        for (int i = 0; i < deckData.availableCards.Length; i++)
        {
            int.TryParse(deckData.availableCards[i].id, out int id);
            _availableCards.Add(id);
        }
        
        _manager.Init(_availableCards, _selectedCards);
    }
}
    
[System.Serializable]
public class DeckData
{
    public Availablecard[] availableCards;
    public string[] selectedIDs;
}

[System.Serializable]
public class Availablecard
{
    public string name;
    public string id;
}