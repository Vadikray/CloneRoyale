using System;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private void Start()
    {
        if (FindFirstObjectByType<GameRecorder>().isPlay)
            FindFirstObjectByType<GameRecorder>().ContinueLoadGame();
    }

    public void SaveGame()
    {
        FindFirstObjectByType<GameRecorder>().SaveGame();
    }
}