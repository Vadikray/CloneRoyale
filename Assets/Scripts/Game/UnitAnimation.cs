using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    private const string State = "State";
    private const string AttackSpeed = "AttackSpeed";

    [SerializeField] private Animator _animator;

    public void Init(Unit unit)
    {
        float damageDelay = unit.parameters.damageDelay;
        _animator.SetFloat(AttackSpeed, 1 / damageDelay);
    }

    public void SetState(UnitStateType type)
    {
        _animator.SetInteger(State, (int) type);
    }
}