using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitBind : MonoBehaviour
{

    #region ================ Private ================

    T_UnitStats _UnitStats;
    T_BindBase _unitBindA;

    [Header("DEBUG: ")]
    [SerializeField] int _bindALevel;


    #endregion
    #region ================== Public ================
    public T_BindBase G_GetBindA() => _unitBindA;

    public void G_SetBindA(T_BindBase b) => _unitBindA = b;

    #endregion
    #region ================== MonoBehaviour ================
    private void Start()
    {
        _UnitStats = GetComponent<T_UnitStats>();




    }
    private void Update()
    {
        if (!_unitBindA) return;
        _bindALevel = _unitBindA._BindLevel;
    }





    #endregion
    #region ==================== Methods ================






    #endregion
}
