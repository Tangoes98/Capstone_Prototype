using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum UnitCombatState
{
    Idle,
    CombatHolding,
    CombatValidation,
    CombatActing,
    CombatDuration,
    CombatCoolDown
}

public class T_UnitCombat : MonoBehaviour
{

    #region ============= Variables =============
    //* Instances
    T_LevelManager _LevelManager;
    T_UIManager _UIManager;
    UnitAttribute _UnitAttributes;
    T_UnitMovement _UnitMovement;
    T_UnitStats _UnitStats;
    T_UnitSkillAction _UnitSkillAction;


    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] bool _attackAvaliable;
    [SerializeField] UnitCombatState _unitCombatStates;
    [SerializeField] T_UnitCombat _attackTarget;
    [SerializeField] bool _isEnemy;
    [SerializeField] bool _isDead;

    [SerializeField] bool _stopCombat;


    [Header("DEBUG: Attack")]
    [SerializeField] float _attackRange;
    [SerializeField] float _attackDamageValue;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _attackTimer;
    [SerializeField] float _attack;
    [SerializeField] float _defence;
    [SerializeField] float _criticalHitRate;

    [SerializeField] float _attackDuration;
    [SerializeField] float _attackDurationTimer;



    #endregion
    #region =================== public =========================
    public bool G_IsEnemyUnit() => _isEnemy;
    public float G_GetAttackCD_UIFillAmount() => _attackTimer / _attackSpeed;

    public T_UnitCombat G_GetAttackTarget() => _attackTarget;
    public event Action Event_DealDamage;

    public void G_LookingForOpponents() => LookingForOpponents();

    public bool G_GetIsUnitDead() => _isDead;
    public void G_SetIsUnitDead(bool bvalue) => _isDead = bvalue;

    public UnitCombatState G_GetState() => _unitCombatStates;

    #endregion
    #region ================= MonoBehaviour ======================
    void Start()
    {

        _isActive = false;

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _UnitStats = GetComponent<T_UnitStats>();
        _UnitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_UnitAttributes);
        _UnitMovement = GetComponent<T_UnitMovement>();
        _UnitSkillAction = GetComponent<T_UnitSkillAction>();

        _isDead = false;

        _attackTimer = _attackSpeed;
        _attackDurationTimer = _attackDuration;
        _unitCombatStates = UnitCombatState.Idle;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;

    }

    void Update()
    {
        if (!_isActive) return;

        //!Debug only
        if (_stopCombat) SwitchCombatState(UnitCombatState.Idle);
        //!------------


        TargetValidation();
        //WaitingSkillActionValidation();


        switch (_unitCombatStates)
        {
            case UnitCombatState.Idle:

                break;

            case UnitCombatState.CombatValidation:
                AttackCDValidation();
                if (!_attackAvaliable)
                {
                    SwitchCombatState(UnitCombatState.CombatCoolDown);
                    return;
                }


                SwitchCombatState(UnitCombatState.CombatHolding);
                break;

            case UnitCombatState.CombatHolding:
                if (_UnitSkillAction.G_GetState() == UnitSkillActionState.SkillDuration) return;

                SwitchCombatState(UnitCombatState.CombatActing);
                break;

            case UnitCombatState.CombatActing:
                AttackAction(_attackTarget);
                break;

            case UnitCombatState.CombatDuration:
                _UnitMovement.G_SwitchMovementState(UnitMovementState.StopMoving);
                CombatDuration();
                break;

            case UnitCombatState.CombatCoolDown:
                WaitingAttackCD();
                break;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }
    #endregion
    #region ============= Method ===============
    void InitializeUnitAttributes(UnitAttribute uatb)
    {
        // implement initialization here only if too much code under Start()
        //* Initialize data from UnitAttribute from T_UnitStats class.
        _isEnemy = _UnitStats.G_IsEnemyUnit();
        _attackRange = uatb.Range;
        _attackSpeed = uatb.AttackSpeed;
        _attackDamageValue = _UnitStats._DamageValue;
        _attackDuration = _UnitStats._AttackDuration;

        _attack = uatb.Attack;
        _defence = uatb.Defence;
        _criticalHitRate = uatb.CriticalHitRate;
    }

    // *----- StateMachine related ------
    void SwitchCombatState(UnitCombatState st) => _unitCombatStates = st;

    #region -------------- Event methods --------------
    void OnBattleStartEvent()
    {
        _isActive = true;
    }

    void OnGameOverEvent()
    {
        _isActive = false;
    }
    #endregion
    #region ------------------- Utilities --------------------------------
    void TargetValidation()
    {
        if (_isEnemy) ValidCurrentTargetUnit(_attackTarget, _LevelManager.G_GetFriendList());
        else ValidCurrentTargetUnit(_attackTarget, _LevelManager.G_GetEnemyList());
    }

    //* Check if the current target unit is active, else remove from unitList.
    void ValidCurrentTargetUnit(T_UnitCombat targetUnit, List<T_UnitCombat> units)
    {
        if (!targetUnit)
        {
            //Debug.Log("No enemies insight");
            _UnitMovement.G_SwitchMovementState(UnitMovementState.OnMoving);
            return;
        }


        if (targetUnit.G_GetIsUnitDead())
        {
            units.Remove(targetUnit);
            _attackTarget = null;
        }
    }

    //* Check if the unit is able to combat
    void AttackCDValidation()
    {
        if (_attackTimer == _attackSpeed) _attackAvaliable = true;
        else _attackAvaliable = false;
    }

    // void WaitingSkillActionValidation()
    // {
    //     if (_UnitSkillAction.G_GetState() != UnitSkillActionState.SkillDuration) return;
    //     if (!_attackAvaliable) return;

    //     SwitchCombatState(UnitCombatState.CombatWaiting);
    // }
    #endregion
    #region ---------------------- Combat ------------------------
    void AttackAction(T_UnitCombat target)
    {
        if (!target) return;

        if (target.TryGetComponent(out T_UnitHealth health))
        {
            // health.G_DealDamage(_attackDamageValue);
            int dmg = Mathf.RoundToInt(UnityEngine.Random.Range(0, _attackDamageValue));
            health.G_DealDamage((float)dmg);
            //Debug.Log($"Deal {_attackValue} damage to {target}");
            Event_DealDamage?.Invoke();
            SwitchCombatState(UnitCombatState.CombatDuration);
        }
    }
    void WaitingAttackCD()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0)
        {
            _attackTimer = _attackSpeed;
            SwitchCombatState(UnitCombatState.CombatValidation);
        }
    }
    #endregion
    #region --------------------- Combat Duration ------------------------
    void CombatDuration()
    {
        _attackDurationTimer -= Time.deltaTime;
        if (_attackDurationTimer < 0)
        {
            _attackDurationTimer = _attackDuration;
            SwitchCombatState(UnitCombatState.CombatCoolDown);
        }
    }







    #endregion
    #region -------------- Scouting ------------------
    void LookingForOpponents()
    {
        if (_isEnemy) SearchEnemyInRange(_LevelManager.G_GetFriendList(), _attackRange);
        else SearchEnemyInRange(_LevelManager.G_GetEnemyList(), _attackRange);
    }
    void SearchEnemyInRange(List<T_UnitCombat> opponents, float range)
    {
        foreach (var unit in opponents)
        {
            if (Vector3.Distance(unit.transform.position, this.transform.position) > range) continue;

            _attackTarget = unit;
            //Debug.Log("Found enemy");
            _UnitMovement.G_SwitchMovementState(UnitMovementState.StopMoving);
            SwitchCombatState(UnitCombatState.CombatValidation);
        }
    }
    #endregion
    #endregion






}
