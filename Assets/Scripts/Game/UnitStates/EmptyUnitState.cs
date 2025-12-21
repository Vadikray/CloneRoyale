using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "UnitState/Empty")]

public class EmptyUnitState : UnitState
{
    public override void Init()
    {
        _unit.SetState(UnitStateType.Default);
    }

    public override void Run()
    {
    }

    public override void Finish()
    {
        Debug.LogWarning($"Юнит {_unit.name} был в пустом состоянии, его перекинуло в дефолтное");
    }
}