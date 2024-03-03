using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitSkillAction : MonoBehaviour
{
    #region ============= Variables =================

    enum UnitSkillActionState
    {
        Active, Saving, Holding
    }
    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] UnitSkillActionState _unitSkillActionStates;

    T_LevelManager _LevelManager;
    T_UIManager _UIManager;
    T_Unit _unit;
    T_UnitHealth _health;



    [Header("Skill Action")]
    [SerializeField] float _skillStrength;
    [SerializeField] bool _isSkillActionReady;
    [SerializeField] float _maxEnergy;
    [SerializeField] float _currentEnergy;
    [SerializeField] float _skillDuration;
    [SerializeField] float _energyAutoRecovery;
    [SerializeField] float _energyDamageRecovery;



    #endregion
    #region ================== Public =====================
    public float G_GetEnergyFillAmount() => _currentEnergy / _maxEnergy;

    #endregion
    #region ============= MonoBehaviour =================
    private void Start()
    {
        _isActive = true;

        _unit = GetComponentInParent<T_Unit>();
        _health = GetComponentInParent<T_UnitHealth>();

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _unitSkillActionStates = UnitSkillActionState.Saving;
        _currentEnergy = 0;

        _health.Take_Damage_Event += OnTakingDamageEvent;
        _unit.Event_DealDamage += OnDealDamageEvent;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;

    }
    private void Update()
    {
        if (!_isActive) return;

        SkillActionValidation();

        switch (_unitSkillActionStates)
        {
            case UnitSkillActionState.Active:
                SkillActionDamage(_skillStrength);

                break;
            case UnitSkillActionState.Saving:
                RegularEnergyAccumulation();

                break;
            case UnitSkillActionState.Holding:
                WaitingForTarget();
                break;
        }


    }




    #endregion
    #region ============== Event Methods =================
    void OnTakingDamageEvent(float f) => _currentEnergy += _energyDamageRecovery;
    void OnDealDamageEvent() => _currentEnergy += _energyDamageRecovery;

    void OnBattleStartEvent()
    {
        _isActive = true;
    }
    void OnGameOverEvent()
    {
        _isActive = false;
    }
    #endregion
    #region ============== Methods =================
    // ----- StateMachine related ------
    void SwitchSkillActionState(UnitSkillActionState st) => _unitSkillActionStates = st;

    // Energy value check
    void SkillActionValidation()
    {
        if (_currentEnergy < _maxEnergy) return;
        SwitchSkillActionState(UnitSkillActionState.Holding);
    }

    #region -------------------- Energy Saving -----------------------
    // Normally Accumulate Energy
    void RegularEnergyAccumulation()
    {
        _currentEnergy += _energyAutoRecovery * Time.deltaTime;
    }
    #endregion
    #region ------------------------ Skill Action ------------------------

    void SkillActionDamage(float damageValue)
    {

        T_Unit target = _unit.G_GetAttackTarget();
        if (!target)
        {
            Debug.Log("BUG OCCOURED");
            SwitchSkillActionState(UnitSkillActionState.Holding);
            return;
        }

        if (target.TryGetComponent(out T_UnitHealth health))
        {
            health.G_DealDamage(damageValue);
            
        }

        SwitchSkillActionState(UnitSkillActionState.Saving);
    }

    #region ----------------------- Holding -------------------------

    void WaitingForTarget()
    {
        _currentEnergy = _maxEnergy;

        T_Unit target = _unit.G_GetAttackTarget();
        if (!target) return;

        _currentEnergy = 0f;

        SwitchSkillActionState(UnitSkillActionState.Active);
    }
    #endregion









    #endregion

    #endregion
}
