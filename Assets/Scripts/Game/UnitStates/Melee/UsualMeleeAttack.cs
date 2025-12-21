using UnityEngine;

[CreateAssetMenu(fileName = "_UsualMeleeAttack", menuName = "UnitState/UsualMeleeAttack")]
public class UsualMeleeAttack : UnitStateAttack
{
    protected override bool TryFindTarget(out float stopAttackDistance)
    {
        Vector3 unitPosition = _unit.transform.position;

        bool hasEnemy = MapInfo.Instance.TryGetNearestWalkingUnit(unitPosition, _targetIsEnemy, out Unit enemy,
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
}