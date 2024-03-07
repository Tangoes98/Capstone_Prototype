using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Taunt : T_SkillBase
{
    //* Skill description:                              //
    //* Draw the enemy within range to attack itself    //


    // public Skill_Taunt(T_UnitStats self)
    // {
    //     _UnitStats = self;
    // }

    public override void TakeAction(T_UnitStats self)
    {
        _selfUnit = self;
        Debug.Log("Skill_Taunt");
        TauntAction();
    }

    #region ============= Skill Methods =============

    public void TauntAction()
    {
        _selfUnit.UnitCombat.G_LookingForOpponents();
        var targets = _selfUnit.UnitCombat.G_GetAllTargetInSight();
        foreach (var unit in targets)
        {
            unit.UnitCombat.G_SetAttackTarget(_selfUnit);
            unit.UnitMovement.G_SwitchMovementState(UnitMovementState.MoveToTarget);
            unit.UnitCombat.G_SwitchCombattState(UnitCombatState.CombatValidation);
            unit.UnitSkillAction.G_SwitchSkillActionState(UnitSkillActionState.SavingEnergy);
            unit.UnitSignifier.G_SetTauntSignifier(true);
        }
    }

    #endregion





}
