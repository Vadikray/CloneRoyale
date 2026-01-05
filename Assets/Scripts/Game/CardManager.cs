using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private CardController[] _cardControllers;
    [SerializeField] private Image _nexCardImage;
    [SerializeField] private int _layerIndex = 6;
    private CardsInGame _cardsInGame;
    private string[] _ids;
    private Camera _camera;
    private List<string> _freeCardsID;
    private string _nexCardID;

    private void Start()
    {
        _ids = new string[_cardControllers.Length];
        _camera = Camera.main;
        _cardsInGame = CardsInGame.Instance;
        _freeCardsID = _cardsInGame.GetAllID();

        MixList(_freeCardsID);

        for (int i = 0; i < _cardControllers.Length; i++)
        {
            string cardID = _freeCardsID[0];
            _freeCardsID.RemoveAt(0);
            _ids[i] = cardID;
            _cardControllers[i].Init(this, i, _cardsInGame._playerDeck[cardID].sprite);
        }

        SetNexRandom();
    }

    private void MixList(List<string> ids)
    {
        int length = ids.Count;
        int[] arr = new int[length];
        for (int i = 0; i < length; i++) arr[i] = i;

        System.Random rand = new System.Random();
        arr = arr.OrderBy(x => rand.Next()).ToArray();

        string[] tempArr = new string[length];
        for (int i = 0; i < length; i++) tempArr[i] = ids[i];

        for (int i = 0; i < length; i++) ids[i] = tempArr[arr[i]];
    }

    public void Release(int controllerIndex, in Vector3 screenPointPosition)
    {
        if (TryGetSpawnPoint(screenPointPosition, out Vector3 spawnPoint) == false) return;

        string id = _ids[controllerIndex];

        _freeCardsID.Add(id);
        _ids[controllerIndex] = _nexCardID;

        _cardControllers[controllerIndex].SetSprite(_cardsInGame._playerDeck[_nexCardID].sprite);

        SetNexRandom();

        _spawner.SendSpawn(id, spawnPoint);
    }

    private bool TryGetSpawnPoint(Vector3 screenPointPosition, out Vector3 spawnPoint)
    {
        Ray ray = _camera.ScreenPointToRay(screenPointPosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.layer == _layerIndex)
        {
            spawnPoint = hit.point;
            spawnPoint.y = 0;
            return true;
        }

        spawnPoint = Vector3.zero;
        return false;
    }

    private void SetNexRandom()
    {
        int randomIndex = Random.Range(0, _freeCardsID.Count);
        _nexCardID = _freeCardsID[randomIndex];
        _freeCardsID.RemoveAt(randomIndex);
        _nexCardImage.sprite = _cardsInGame._playerDeck[_nexCardID].sprite;
    }
}