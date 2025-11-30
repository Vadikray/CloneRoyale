using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float max { get; private set; } = 10f;
    private float _current;

    private void Start()
    {
        _current = max;
    }

    public void ApplyDamage(float value)
    {
        _current -= value;
        if (_current <= 0) _current = 0;
        
        Debug.Log($"Обьект {name}: ,было - {_current + value}, стало {_current}");
    }
}

interface IHealth
{
    Health health { get; }
}