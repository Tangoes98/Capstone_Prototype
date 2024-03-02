using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class T_Unit : MonoBehaviour
{
    enum UnitCombatState
    {
        Idle, ReadyToCombat, Combat, SkillAction, ActionCoolDown
    }
    enum UnitMovementState
    {
        OnMoving, StopMoving
    }

    #region ============= Variables =============
    //* Instances
    T_LevelManager _LevelManager;
    T_UIManager _UIManager;


    [Header("DEBUG_VIEW")]
    [SerializeField] bool _attackAvaliable;
    [SerializeField] UnitCombatState _unitCombatStates;
    [SerializeField] UnitMovementState _uniMovementStates;
    [SerializeField] T_Unit _attackTarget;

    [Header("====Field_value_required=====")]
    [SerializeField] bool _isEnemy;


    [Header("Movement")]
    [SerializeField] float _unitMoveSpeed;

    [Header("Attack")]
    [SerializeField] float _attackRange;
    [SerializeField] float _attackValue;
    [SerializeField] float _attackSpeed;
    float _attackTimer;


    NavMeshAgent _agent;
    #endregion
    #region =================== public =========================
    public bool G_IsEnemyUnit() => _isEnemy;
    public float G_GetAttackCD_UIFillAmount() => _attackTimer / _attackSpeed;





    #endregion
    #region ================= MonoBehaviour ======================
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _attackTimer = _attackSpeed;

        _unitCombatStates = UnitCombatState.Idle;
        _uniMovementStates = UnitMovementState.StopMoving;

        _UIManager.Event_BattleStart += OnBattleStartEvent;


    }

    void Update()
    {
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

            case UnitCombatState.SkillAction:
                break;

            case UnitCombatState.ActionCoolDown:
                StartAttackCD();
                break;
        }


        switch (_uniMovementStates)
        {
            case UnitMovementState.OnMoving:
                LookingForOpponents();
                UnitMovement();
                break;

            case UnitMovementState.StopMoving:
                StopMovement();
                break;
        }

        // switch (_unitReconStates)
        // {
        //     case UnitReconState.Idle:

        //         break;
        //     case UnitReconState.Recon:
        //         LookingForOpponents();
        //         break;
        // }





    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }
    #endregion
    #region ============= Method ===============
    void Init()
    {
        //todo: implement initialization here
    }

    void UpdateUnitState()
    {

    }

    // ----- StateMachine related ------
    void SwitchCombatState(UnitCombatState st) => _unitCombatStates = st;
    void SwitchMovementState(UnitMovementState st) => _uniMovementStates = st;
    // void SwitchReconState(UnitReconState st) => _unitReconStates = st;



    #region -------------- Event methods --------------
    void OnBattleStartEvent()
    {

    }
    #endregion
    #region ------------------- Utilities --------------------------------
    void TargetValidation()
    {
        if (_isEnemy) ValidCurrentTargetUnit(_attackTarget, _LevelManager.G_GetFriendList());
        else ValidCurrentTargetUnit(_attackTarget, _LevelManager.G_GetEnemyList());
    }
    // Check if the current target unit is active, else remove from unitList.
    void ValidCurrentTargetUnit(T_Unit targetUnit, List<T_Unit> units)
    {
        if (!targetUnit)
        {
            Debug.Log("No enemies insight");
            SwitchMovementState(UnitMovementState.OnMoving);
            return;
        }


        if (!targetUnit.gameObject.activeSelf)
        {
            units.Remove(targetUnit);
            _attackTarget = null;
        }
    }

    // Based on the unit list, look for the closest unit returned as target
    T_Unit GetClosestOpponentUnit(List<T_Unit> units)
    {
        if (units.Count < 1) return null;

        List<float> distances = new();
        Dictionary<float, T_Unit> unitDic = new();
        foreach (var unit in units)
        {
            float dis = Vector3.Distance(unit.transform.position, this.transform.position);
            distances.Add(dis);
            unitDic.Add(dis, unit);

        }
        float targetDistance = Mathf.Min(distances.ToArray());
        return unitDic[targetDistance];
    }

    // Check if the unit is able to combat
    void IsAbleToCombat()
    {
        if (_attackTimer == _attackSpeed) _attackAvaliable = true;
        else _attackAvaliable = false;

    }
    #endregion
    #region ---------------------- Combat ------------------------
    void AttackAction(T_Unit target)
    {
        if (!target) return;

        if (target.TryGetComponent(out T_UnitHealth health))
        {
            health.G_DealDamage(_attackValue);
            Debug.Log($"Deal {_attackValue} damage to {target}");

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
    void SearchEnemyInRange(List<T_Unit> opponents, float range)
    {
        foreach (var unit in opponents)
        {
            if (Vector3.Distance(unit.transform.position, this.transform.position) > range) continue;

            _attackTarget = unit;
            Debug.Log("Found enemy");
            SwitchMovementState(UnitMovementState.StopMoving);
            SwitchCombatState(UnitCombatState.ReadyToCombat);
            //SwitchCombatState(UnitCombatState.Combat);
        }
    }
    #endregion
    #region -------------------- Movement ----------------------
    void UnitMovement()
    {
        if (_isEnemy) MoveToTarget(_LevelManager.G_GetFriendList());
        else MoveToTarget(_LevelManager.G_GetEnemyList());
    }
    void MoveToTarget(List<T_Unit> opponents)
    {
        _agent.speed = _unitMoveSpeed;
        // Check if still opponent unit left
        if (opponents.Count < 1)
        {
            SwitchMovementState(UnitMovementState.StopMoving);
            return;
        }

        T_Unit target = GetClosestOpponentUnit(opponents);

        // Move to opponent
        Vector3 targetPosition = target.transform.position;
        _agent.destination = targetPosition;
    }
    void StopMovement()
    {
        //_agent.destination = transform.position;
        _agent.speed = 0f;
    }
    #endregion




    #endregion






}
