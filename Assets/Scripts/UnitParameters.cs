using UnityEngine;

public class UnitParameters : MonoBehaviour
{
    [field: SerializeField] public float speed { get; private set; } = 4f;
    [field: SerializeField] public float modelRadius { get; private set; } = 1f;
    [field: SerializeField] public float startChaseDistance { get; private set; } = 5f;
    [field: SerializeField] public float stoptChaseDistance { get; private set; } = 7f;

    public float startAttackDistance { get { return modelRadius + _startAttackDistance; } }

    public float stopAttackDistance { get { return modelRadius + _stopAttackDistance; } }

    [SerializeField] public float _startAttackDistance = 1f;
    [SerializeField] public float _stopAttackDistance = 1.5f;
}