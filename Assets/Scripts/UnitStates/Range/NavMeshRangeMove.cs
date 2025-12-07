using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshRangeMove", menuName = "UnitState/NavMeshRangeMove")]
public class NavMeshRangeMove : UnitStateNavMeshMove
{
    protected override bool TryFindTarget(out UnitStateType changeType)
    {
        if (TryAttackTower())
        {
            changeType = UnitStateType.Attack;
            return true;
        }

        if (TryChaseUnit())
        {
            changeType = UnitStateType.Chase;
            return true;
        }

        changeType = UnitStateType.None;
        return false;
    }

    private bool TryAttackTower()
    {
        float distanceToTarget = _nearestTower.GetDistance(_unit.transform.position);
        if (distanceToTarget <= _unit.parameters.startAttackDistance)
        {
            return true;
        }

        return false;
    }

    private bool TryChaseUnit()
    {
        bool hasEnemy = MapInfo.Instance.TryGetNearestAnyUnit(_unit.transform.position, _targetIsEnemy, out Unit enemy,
            out float distance);
        if (hasEnemy == false) return false;
        if (_unit.parameters.startChaseDistance >= distance)
        {
            _unit.SetState(UnitStateType.Chase);
            return true;
        }

        return false;
    }
}