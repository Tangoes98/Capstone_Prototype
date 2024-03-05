using System;
using System.Collections.Generic;
using UnityEngine;



public enum UnitCombatState
{
    Idle, ReadyToCombat, Combat, WaitingSkillAction, ActionCoolDown
}

public class T_UnitCombatManager : MonoBehaviour
{

    #region ============= Variables =============
    //* Instances
    T_LevelManager _LevelManager;
    T_UIManager _UIManager;
    T_UnitMovement _UnitMovement;
    T_UnitStats _UnitStats;
    UnitAttribute _unitAttributes;


    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] bool _attackAvaliable;
    [SerializeField] UnitCombatState _unitCombatStates;
    [SerializeField] T_UnitCombatManager _attackTarget;
    [SerializeField] bool _isEnemy;


    [Header("DEBUG: Attack")]
    [SerializeField] float _attackRange;
    [SerializeField] float _attackDamageValue;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _attack;
    [SerializeField] float _defence;
    [SerializeField] float _criticalHitRate;
    float _attackTimer;

    #endregion
    #region =================== public =========================
    public bool G_IsEnemyUnit() => _isEnemy;
    public float G_GetAttackCD_UIFillAmount() => _attackTimer / _attackSpeed;

    public T_UnitCombatManager G_GetAttackTarget() => _attackTarget;
    public event Action Event_DealDamage;

    public void G_LookingForOpponents() => LookingForOpponents();

    #endregion
    #region ================= MonoBehaviour ======================
    void Start()
    {

        _isActive = false;

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _UnitMovement = GetComponent<T_UnitMovement>();
        _UnitStats = GetComponent<T_UnitStats>();
        _unitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_unitAttributes);

        _attackTimer = _attackSpeed;
        _unitCombatStates = UnitCombatState.Idle;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;

    }

    void Update()
    {
        if (!_isActive) return;

        TargetValidation();
        IsAbleToCombat();

        switch (_unitCombatStates)
        {
            case UnitCombatState.Idle:

                break;

            case UnitCombatState.ReadyToCombat:
                if (_attackAvaliable) SwitchCombatState(UnitCombatState.Combat);
                else SwitchCombatState(UnitCombatState.ActionCoolDown);
                break;

            case UnitCombatState.Combat:
                AttackAction(_attackTarget);
                break;

            case UnitCombatState.WaitingSkillAction:
                break;

            case UnitCombatState.ActionCoolDown:
                StartAttackCD();
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
        _attackDamageValue = _UnitStats._damageValue;

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
    void ValidCurrentTargetUnit(T_UnitCombatManager targetUnit, List<T_UnitCombatManager> units)
    {
        if (!targetUnit)
        {
            //Debug.Log("No enemies insight");
            _UnitMovement.G_SwitchMovementState(UnitMovementState.OnMoving);
            return;
        }


        if (!targetUnit.gameObject.activeSelf)
        {
            units.Remove(targetUnit);
            _attackTarget = null;
        }
    }

    //* Check if the unit is able to combat
    void IsAbleToCombat()
    {
        if (_attackTimer == _attackSpeed) _attackAvaliable = true;
        else _attackAvaliable = false;

    }
    #endregion
    #region ---------------------- Combat ------------------------
    void AttackAction(T_UnitCombatManager target)
    {
        if (!target) return;

        if (target.TryGetComponent(out T_UnitHealth health))
        {
            health.G_DealDamage(_attackDamageValue);
            //Debug.Log($"Deal {_attackValue} damage to {target}");
            Event_DealDamage?.Invoke();

            SwitchCombatState(UnitCombatState.ActionCoolDown);
        }
    }
    void StartAttackCD()
    {
        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0)
        {
            _attackTimer = _attackSpeed;
            SwitchCombatState(UnitCombatState.ReadyToCombat);
        }
    }
    #endregion
    #region -------------- Scouting ------------------
    void LookingForOpponents()
    {
        if (_isEnemy) SearchEnemyInRange(_LevelManager.G_GetFriendList(), _attackRange);
        else SearchEnemyInRange(_LevelManager.G_GetEnemyList(), _attackRange);
    }
    void SearchEnemyInRange(List<T_UnitCombatManager> opponents, float range)
    {
        foreach (var unit in opponents)
        {
            if (Vector3.Distance(unit.transform.position, this.transform.position) > range) continue;

            _attackTarget = unit;
            //Debug.Log("Found enemy");
            _UnitMovement.G_SwitchMovementState(UnitMovementState.StopMoving);
            SwitchCombatState(UnitCombatState.ReadyToCombat);
        }
    }
    #endregion
    #endregion






}
