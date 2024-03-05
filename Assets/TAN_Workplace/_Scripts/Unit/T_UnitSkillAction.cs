using UnityEngine;

public enum UnitSkillActionState
{
    WaitingSkillAnimation, SkillActing, SavingEnergy, Holding
}
public class T_UnitSkillAction : MonoBehaviour
{
    #region ============= Private =================



    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] UnitSkillActionState _unitSkillActionStates;

    T_LevelManager _LevelManager;
    T_UIManager _UIManager;
    T_UnitCombatManager _unitCombatMgr;
    T_UnitHealth _health;
    T_UnitStats _UnitStats;
    UnitAttribute _unitAttributes;



    [Header("DEBUG: Skill Action")]
    [SerializeField] float _skillPower;
    [SerializeField] bool _isSkillActionReady;
    [SerializeField] float _maxEnergy;
    [SerializeField] float _currentEnergy;
    [SerializeField] float _skillDuration;
    [SerializeField] float _energyAutoRecovery;
    [SerializeField] float _energyPerDamageRecovery;



    #endregion
    #region ================== Public =====================
    public float G_GetEnergyFillAmount() => _currentEnergy / _maxEnergy;
    public bool G_IsSkillActionReady() => _isSkillActionReady;

    #endregion
    #region ============= MonoBehaviour =================
    //! START
    private void Start()
    {
        _isActive = false;

        _UnitStats = GetComponent<T_UnitStats>();
        _unitCombatMgr = GetComponentInParent<T_UnitCombatManager>();
        _health = GetComponentInParent<T_UnitHealth>();
        _unitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_unitAttributes);

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _unitSkillActionStates = UnitSkillActionState.SavingEnergy;
        _currentEnergy = 0;

        _health.Take_Damage_Event += OnTakingDamageEvent;
        _unitCombatMgr.Event_DealDamage += OnDealDamageEvent;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;

    }

    //! UPDATE
    private void Update()
    {
        if (!_isActive) return;

        SkillActionValidation();

        switch (_unitSkillActionStates)
        {
            case UnitSkillActionState.WaitingSkillAnimation:
                break;
            case UnitSkillActionState.SkillActing:
                SkillActionDamage(_skillPower);
                break;
            case UnitSkillActionState.SavingEnergy:
                RegularEnergyAccumulation();

                break;
            case UnitSkillActionState.Holding:
                WaitingForTarget();
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
        SwitchSkillActionState(UnitSkillActionState.Holding);
    }

    #region ---------------- Event Methods ---------------
    void OnTakingDamageEvent(float f) => _currentEnergy += _energyPerDamageRecovery;
    void OnDealDamageEvent() => _currentEnergy += _energyPerDamageRecovery;

    void OnBattleStartEvent()
    {
        _isActive = true;
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
    #region ------------------------ Skill Action ------------------------
    void SkillActionDamage(float damageValue)
    {

        T_UnitCombatManager target = _unitCombatMgr.G_GetAttackTarget();
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

        SwitchSkillActionState(UnitSkillActionState.SavingEnergy);
    }

    #endregion
    #region ----------------------- Holding -------------------------
    void WaitingForTarget()
    {
        _currentEnergy = _maxEnergy;

        T_UnitCombatManager target = _unitCombatMgr.G_GetAttackTarget();
        if (!target) return;

        _currentEnergy = 0f;

        SwitchSkillActionState(UnitSkillActionState.SkillActing);
    }
    #endregion
    #endregion
}
