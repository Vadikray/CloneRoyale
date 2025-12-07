using UnityEngine;

[CreateAssetMenu(fileName = "_NavMeshMeleeChase", menuName = "UnitState/NavMeshMeleeChase")]
public class NavMeshMeleeChase : UnitStateNavMeshChase
{
    protected override void FindTargetUnit(out Unit targetUnit)
    {
        MapInfo.Instance.TryGetNearestWalkingUnit(_unit.transform.position, _targetIsEnemy, out targetUnit,
            out float distance);
    }
}