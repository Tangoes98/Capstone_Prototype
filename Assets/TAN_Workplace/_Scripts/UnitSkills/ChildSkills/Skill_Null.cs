using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Null : T_SkillBase
{


    public override void TakeAction(T_UnitStats self)
    {
        _selfUnit = self;
        Debug.Log("SKILL_NULL");
        DEBUG_SKILL();
    }

    void DEBUG_SKILL()
    {

    }

}
