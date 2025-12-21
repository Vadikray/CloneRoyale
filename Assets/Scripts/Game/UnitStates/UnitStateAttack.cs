using UnityEngine;

public abstract class UnitStateAttack : UnitState
{
    [SerializeField] protected float _damage = 1.5f;
    private float _delay = 1f;
    private float _time = 0f;
    private float _stopAttackDistance = 0;
    protected bool _targetIsEnemy;
    protected Health _target;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = _unit.isEnemy == false;
        _delay = unit.parameters.damageDelay;
    }

    public override void Init()
    {
        if (TryFindTarget(out _stopAttackDistance) == false)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        _time = 0;
        _unit.transform.LookAt(_target.transform.position);
    }

    public override void Run()
    {
        if (_target == false)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        _time += Time.deltaTime;
        if (_time < _delay) return;
        _time -= _delay;


        float distanceToTarget = Vector3.Distance(_target.transform.position, _unit.transform.position);
        if (distanceToTarget > _stopAttackDistance) _unit.SetState(UnitStateType.Chase);

        Attack();
    }

    protected virtual void Attack()
    {
        _target.ApplyDamage(_damage);
    }

    public override void Finish()
    {
    }

    protected abstract bool TryFindTarget(out float stopAttackDistance);
}