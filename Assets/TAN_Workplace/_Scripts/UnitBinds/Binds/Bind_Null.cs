using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bind_Null : T_BindBase
{

    protected override void Start()
    {
        _BindName = "Null";
    }

    public override void BindEffect()
    {
        Debug.Log("Null BindEffect");
    }
}
