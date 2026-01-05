using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private void Start()
    {
        MultiplayerManager.Instance.StartTick += StartTick;
    }

    private void OnDestroy()
    {
        MultiplayerManager.Instance.StartTick -= StartTick;
    }

    private float _offset = 0;

    public void StartTick(string jsonTick)
    {
        Tick tick = JsonUtility.FromJson<Tick>(jsonTick);
        if (tick.tick < 10) return;

        float gameTime = Time.time;
        float serverTime = tick.time / 1000;

        _offset = gameTime - serverTime;
    }

    public float GetConvertTime(uint serverTime) => serverTime / 1000 + _offset;

    [System.Serializable]
    public class Tick
    {
        public int tick;
        public uint time;
    }
}