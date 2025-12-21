using UnityEngine;

[CreateAssetMenu(fileName = "_UsualRangeAttack", menuName = "UnitState/UsualRangeAttack")]
public class UsualRangettack : UnitStateAttack
{
    [SerializeField] private Arrow _arrow;

    protected override bool TryFindTarget(out float stopAttackDistance)
    {
        Vector3 unitPosition = _unit.transform.position;

        bool hasEnemy = MapInfo.Instance.TryGetNearestAnyUnit(unitPosition, _targetIsEnemy, out Unit enemy,
            out float distance);

        if (hasEnemy && distance - enemy.parameters.modelRadius <= _unit.parameters.startAttackDistance)
        {
            _target = enemy.health;
            stopAttackDistance = _unit.parameters.stopAttackDistance + enemy.parameters.modelRadius;
            return true;
        }

        Tower targetTower = MapInfo.Instance.GetNearestTower(unitPosition, _targetIsEnemy);
        if (targetTower.GetDistance(unitPosition) <= _unit.parameters.startAttackDistance)
        {
            _target = targetTower.health;
            stopAttackDistance = _unit.parameters.stopAttackDistance + targetTower.radius;
            return true;
        }

        stopAttackDistance = 0;
        return false;
    }

    protected override void Attack()
    {
        Vector3 unitPosition = _unit.transform.position;
        Vector3 targetPosition = _target.transform.position;
        
        Arrow arrow = Instantiate(_arrow, unitPosition, Quaternion.identity);
        arrow.Init(targetPosition);
        
        float delay = Vector3.Distance(unitPosition, targetPosition) / arrow.speed;
        _target.ApplyDelayDamage(delay, _damage);
    }
}