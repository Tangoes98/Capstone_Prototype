using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bind_IronGuard : T_BindBase
{

    protected override void Start()
    {
        base.Start();
        _BindName = "IronGuard";
    }



    #region ========== Bind Effection ===============
    public override void BindEffect()
    {
        Debug.Log("IronGuard_BindEffect_Called");

        switch (_bindEffectLevel)
        {
            case BindEffectLevel.A:
                Debug.Log("IronGuard_BindEffect_A");

                break;
            case BindEffectLevel.B:
                Debug.Log("IronGuard_BindEffect_B");

                break;
            case BindEffectLevel.C:
                Debug.Log("IronGuard_BindEffect_C");

                break;
        }

    }






    #endregion


}
