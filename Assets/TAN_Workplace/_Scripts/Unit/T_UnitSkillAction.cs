using System;
using UnityEngine;

public enum UnitSkillActionState
{
    Debug,
    SkillHolding,
    SkillActing,
    SkillDuration,
    SavingEnergy,
}
public class T_UnitSkillAction : MonoBehaviour
{
    #region ============= Private =================



    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] UnitSkillActionState _unitSkillActionStates;

    T_LevelManager _LevelManager;
    T_SkillCollection _SkillCollection;
    T_UIManager _UIManager;
    T_UnitCombat _UnitCombatMgr;
    T_UnitHealth _Health;
    T_UnitMovement _UnitMovement;
    T_UnitStats _UnitStats;
    UnitAttribute _unitAttributes;



    [Header("DEBUG: Skill Action")]
    [SerializeField] float _skillPower;
    [SerializeField] bool _isSkillActionReady;
    [SerializeField] float _maxEnergy;
    [SerializeField] float _currentEnergy;
    [SerializeField] float _skillDuration;
    [SerializeField] float _skillDurationTimer;
    [SerializeField] float _energyAutoRecovery;
    [SerializeField] float _energyPerDamageRecovery;



    #endregion
    #region ================== Public =====================
    public float G_GetEnergyFillAmount() => _currentEnergy / _maxEnergy;
    //public bool G_IsSkillActionReady() => _isSkillActionReady;
    public UnitSkillActionState G_GetState() => _unitSkillActionStates;

    public void G_SwitchSkillActionState(UnitSkillActionState st) => SwitchSkillActionState(st);

    // public event Action Event_SkillActivated;

    #endregion
    #region ============= MonoBehaviour =================
    private void Start()
    {
        _isActive = false;

        _UnitStats = GetComponent<T_UnitStats>();
        _UnitCombatMgr = GetComponentInParent<T_UnitCombat>();
        _Health = GetComponentInParent<T_UnitHealth>();
        _UnitMovement = GetComponent<T_UnitMovement>();

        _unitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_unitAttributes);

        _isSkillActionReady = false;

        _LevelManager = T_LevelManager.Instance;
        _SkillCollection = T_SkillCollection.Instance;
        _UIManager = T_UIManager.Instance;

        _unitSkillActionStates = UnitSkillActionState.Debug;
        _currentEnergy = 0;

        _skillDurationTimer = _skillDuration;


        _UnitCombatMgr.Event_DealDamage += OnDealDamageEvent;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;

    }

    private void Update()
    {
        if (!_isActive) return;


        switch (_unitSkillActionStates)
        {
            case UnitSkillActionState.SavingEnergy:
                SkillActionValidation();
                RegularEnergyAccumulation();
                break;

            case UnitSkillActionState.SkillHolding:
                if (_UnitMovement.G_GetState() == UnitMovementState.MoveToTarget) return;
                if (_UnitCombatMgr.G_GetState() == UnitCombatState.CombatDuration) return;

                WaitingForTarget();
                break;

            case UnitSkillActionState.SkillActing:
                //SkillActionDamage(_skillPower);
                SkillAction(_UnitStats.G_GetSkillIndex());
                // Event_SkillActivated?.Invoke();
                break;

            case UnitSkillActionState.SkillDuration:
                _UnitMovement.G_SwitchMovementState(UnitMovementState.StopMoving);
                WaitingForSkillDuration();
                break;

            case UnitSkillActionState.Debug:
                break;
        }


    }
    #endregion
    #region ============== Methods =================
    void InitializeUnitAttributes(UnitAttribute uatb)
    {
        _skillPower = uatb.SkillPower;
        _maxEnergy = uatb.MaxEnergy;
        _skillDuration = uatb.SkillDuration;
        _energyAutoRecovery = uatb.EnergyAutoRecovery;
        _energyPerDamageRecovery = uatb.EnergyPerDamageRecovery;
    }
    void SwitchSkillActionState(UnitSkillActionState st) => _unitSkillActionStates = st;

    //* Energy value check
    void SkillActionValidation()
    {
        if (_currentEnergy < _maxEnergy) return;
        SwitchSkillActionState(UnitSkillActionState.SkillHolding);
    }

    #region ---------------- Event Methods ---------------
    //void OnTakingDamageEvent(float f) => _currentEnergy += _energyPerDamageRecovery;
    void OnDealDamageEvent() => _currentEnergy += _energyPerDamageRecovery;

    void OnBattleStartEvent()
    {
        _isActive = true;
        _unitSkillActionStates = UnitSkillActionState.SavingEnergy;
    }
    void OnGameOverEvent()
    {
        _isActive = false;
    }
    #endregion
    #region -------------------- Energy Saving -----------------------
    //* Normally Accumulate Energy
    void RegularEnergyAccumulation()
    {
        _currentEnergy += _energyAutoRecovery * Time.deltaTime;
    }
    #endregion
    #region --------------------- Waiting for skill duration -----------------------
    void WaitingForSkillDuration()
    {
        Debug.Log("Skill Duration");
        _skillDurationTimer -= Time.deltaTime;
        if (_skillDurationTimer < 0)
        {
            Debug.Log("Skill Duration Over");
            _skillDurationTimer = _skillDuration;
            SwitchSkillActionState(UnitSkillActionState.SavingEnergy);
        }
    }

    #endregion
    #region ------------------------ Skill Action ------------------------
    // void SkillActionDamage(float damageValue)
    // {
    //     _isSkillActionReady = false;

    //     T_UnitStats target = _UnitCombatMgr.G_GetAttackTarget();
    //     if (!target)
    //     {
    //         Debug.Log("NO Target Available");
    //         SwitchSkillActionState(UnitSkillActionState.SkillHolding);
    //         return;
    //     }

    //     if (target.TryGetComponent(out T_UnitHealth health))
    //     {
    //         health.G_DealDamage(damageValue);

    //     }

    //     SwitchSkillActionState(UnitSkillActionState.SkillDuration);
    // }

    void SkillAction(int skillIndex)
    {
        _isSkillActionReady = false;

        T_UnitStats target = _UnitCombatMgr.G_GetAttackTarget();
        if (!target)
        {
            Debug.Log("NO Target Available");
            SwitchSkillActionState(UnitSkillActionState.SkillHolding);
            return;
        }

        Debug.Log("Skill Action Activated");

        _SkillCollection.G_GetSkillDic()[skillIndex].TakeAction(_UnitStats);


        SwitchSkillActionState(UnitSkillActionState.SkillDuration);
    }

    #endregion
    #region ----------------------- Holding -------------------------
    void WaitingForTarget()
    {
        _currentEnergy = _maxEnergy;

        T_UnitStats target = _UnitCombatMgr.G_GetAttackTarget();
        if (!target) return;

        _isSkillActionReady = true;

        _currentEnergy = 0f;

        SwitchSkillActionState(UnitSkillActionState.SkillActing);
    }
    #endregion
    #endregion
}
