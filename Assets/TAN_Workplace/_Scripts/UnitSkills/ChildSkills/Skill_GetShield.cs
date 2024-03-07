using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_GetShield : T_SkillBase
{


    public override void TakeAction(T_UnitStats self)
    {
        _selfUnit = self;
        Debug.Log("Skill_GetShield");
        GetShield();
    }

    void GetShield()
    {

    }

}
