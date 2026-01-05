using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public SpawnData(string id, Vector3 spawnPoint)
        {
            cardID = id;
            x = spawnPoint.x;
            y = spawnPoint.y;
            z = spawnPoint.z;
        }

        public string cardID;
        public float x;
        public float y;
        public float z;
        public uint serverTime;
    }

    [SerializeField] private TimerManager _timerManager;
    private Queue<GameObject> _galagrams = new Queue<GameObject>();

    private void Start()
    {
        MultiplayerManager.Instance.SpawnPlayer += SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy += SpawnEnemy;
        MultiplayerManager.Instance.Cheat += CancelSpawn;
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.SpawnPlayer -= SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy -= SpawnEnemy;
        MultiplayerManager.Instance.Cheat -= CancelSpawn;
    }

    private void SpawnEnemy(string json) => StartCoroutine(Spawn(json, true));

    private void SpawnPlayer(string json) => StartCoroutine(Spawn(json, false));


    public IEnumerator Spawn(string jsonSpawnData, bool isEnemy)
    {
        SpawnData data = JsonUtility.FromJson<SpawnData>(jsonSpawnData);
        string id = data.cardID;
        Vector3 spawnPoint = new Vector3(data.x, data.y, data.z);

        Unit unitPrefab;
        Quaternion rotation = Quaternion.identity;
        if (isEnemy)
        {
            unitPrefab = CardsInGame.Instance._enemyDeck[id].unit;
            rotation = Quaternion.Euler(0, 180, 0);
            spawnPoint *= -1;
        }
        else
        {
            unitPrefab = CardsInGame.Instance._playerDeck[id].unit;
        }

        float diff = _timerManager.GetConvertTime(data.serverTime) - Time.time;

        if (diff < 0)
        {
            Debug.LogError("Все пошло не по плану, надо что-то придумывать другое");
        }
        else
        {
            yield return new WaitForSeconds(diff);
        }

        if (isEnemy == false && _galagrams.Count > 0)
        {
            var galagram = _galagrams.Dequeue();
            Destroy(galagram);
        }

        Unit unit = Instantiate(unitPrefab, spawnPoint, rotation);
        unit.Init(isEnemy);
        MapInfo.Instance.AddUnit(unit);
    }

    private void CancelSpawn()
    {
        if (_galagrams.Count < 1) return;

        var galagram = _galagrams.Dequeue();
        Destroy(galagram);
    }

    public void SendSpawn(string id, in Vector3 spawnPoint)
    {
        var galagram = Instantiate(CardsInGame.Instance._playerDeck[id].galagram, spawnPoint, Quaternion.identity);
        _galagrams.Enqueue(galagram);

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"json", JsonUtility.ToJson(new SpawnData(id, spawnPoint))}
        };

        MultiplayerManager.Instance.SendMessage("Spawn", data);
    }
}