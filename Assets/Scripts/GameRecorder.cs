using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TimerManager;
using static MatchmakingManager;

public class GameRecorder : MonoBehaviour
{
    [System.Serializable]
    public class Game
    {
        public string jsonDeck;
        public string startTick;
        public List<Spawnlog> spawnlogs = new List<Spawnlog>();
    }
    [System.Serializable]
    public class Spawnlog
    {
        public Spawnlog(string json, bool isEnemy)
        {
            this.json = json;
            this.isEnemy = isEnemy;
        }

        public string json;
        public bool isEnemy;
    }
    private Game _game = new Game();
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        MultiplayerManager.Instance.StartGame += StartGame;
        MultiplayerManager.Instance.StartTick += StartTick;
        MultiplayerManager.Instance.SpawnPlayer += SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy += SpawnEnemy;
    }
    private void OnDestroy()
    {
        MultiplayerManager.Instance.StartGame -= StartGame;
        MultiplayerManager.Instance.StartTick -= StartTick;
        MultiplayerManager.Instance.SpawnPlayer -= SpawnPlayer;
        MultiplayerManager.Instance.SpawnEnemy -= SpawnEnemy;
    }
    private void SpawnPlayer(string obj)
    {
        _game.spawnlogs.Add(new Spawnlog(obj, false));
    }
    private void SpawnEnemy(string obj)
    {
        _game.spawnlogs.Add(new Spawnlog(obj, true));
    }
    private void StartTick(string jsonTick)
    {
        Tick tick = JsonUtility.FromJson<Tick>(jsonTick);
        if (tick.tick < 10) return;
        _game.startTick = jsonTick;
    }
    private void StartGame(string jsonDeck)
    {
        _game.jsonDeck = jsonDeck;
    }

    public void SaveGame()
    {
        string path = Path.Combine(Application.dataPath, "../FILES", "Game.txt");
        string json = JsonUtility.ToJson(_game);
        using (StreamWriter sw = new StreamWriter(path, true))
            sw.Write(json);
    }

    public bool isPlay = false;
    public void LoadGame()
    {
        isPlay = true;
        string path = Path.Combine(Application.dataPath, "../FILES", "Game.txt");
        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();
            _game = JsonUtility.FromJson<Game>(json);
        }
        Decks decks = JsonUtility.FromJson<Decks>(_game.jsonDeck);
        string[] playerDeck = decks.player1;;
        string[] enemyDeck = decks.player2;;
        CardsInGame.Instance.SetDecks(playerDeck, enemyDeck);
        SceneManager.LoadScene("SampleScene");
    }

    public void ContinueLoadGame()
    {
        FindFirstObjectByType<TimerManager>().StartTick(_game.startTick);

        var spawner = FindFirstObjectByType<Spawner>();
        for (int i = 0; i < _game.spawnlogs.Count; i++)
        {
            var log = _game.spawnlogs[i];
            spawner.StartCoroutine(spawner.Spawn(log.json, log.isEnemy));
        }
    }
}