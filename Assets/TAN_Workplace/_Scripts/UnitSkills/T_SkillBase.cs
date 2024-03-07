using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class T_SkillBase : MonoBehaviour
{

    protected T_UnitStats _selfUnit;

    public abstract void TakeAction(T_UnitStats self);

    public T GetSkill<T>() where T : T_SkillBase => (T)this;


}
