using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class T_BindBase : MonoBehaviour
{
    public enum BindEffectLevel
    {
        A, B, C
    };

    //[SerializeField] protected string _bindName;
    public string _BindName;
    [SerializeField] protected BindEffectLevel _bindEffectLevel;
    //[SerializeField] protected int _bindLevel;
    public int _BindLevel;
    [SerializeField] protected int _requiredLevelToA;
    [SerializeField] protected int _requiredLevelToB;
    [SerializeField] protected int _requiredLevelToC;

    [Header("Debug: Unit related")]
    [SerializeField] protected T_UnitStats _unit;

    public abstract void BindEffect();

    protected virtual void Start()
    {
        _BindLevel = 0;
    }
    protected virtual void Update()
    {
        if (_BindLevel == _requiredLevelToA) _bindEffectLevel = BindEffectLevel.A;
        if (_BindLevel == _requiredLevelToB) _bindEffectLevel = BindEffectLevel.B;
        if (_BindLevel == _requiredLevelToC) _bindEffectLevel = BindEffectLevel.C;
    }


}
