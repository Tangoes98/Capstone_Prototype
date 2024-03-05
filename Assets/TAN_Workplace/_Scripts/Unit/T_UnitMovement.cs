using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitMovementState
{
    OnMoving, StopMoving
}

public class T_UnitMovement : MonoBehaviour
{
    #region ================== Private =================

    T_LevelManager _LevelManager;
    T_UIManager _UIManager;
    T_UnitCombat _UnitCombatMgr;
    NavMeshAgent _agent;
    T_UnitStats _UnitStats;
    UnitAttribute _UnitAttributes;



    [SerializeField] bool _isActive;
    [Header("DEBUG")]
    [SerializeField] UnitMovementState _uniMovementStates;
    [SerializeField] T_UnitCombat _moveToTarget;

    [Header("DEBUG: Movement")]
    [SerializeField] float _unitMoveSpeed;



    #endregion
    #region ===================== Public ===================
    public void G_SwitchMovementState(UnitMovementState st) => SwitchMovementState(st);





    #endregion
    #region ====================== MonoBehaviour ==================
    private void Start()
    {
        _isActive = false;
        _agent = this.GetComponent<NavMeshAgent>();

        _LevelManager = T_LevelManager.Instance;
        _UIManager = T_UIManager.Instance;

        _UnitCombatMgr = GetComponent<T_UnitCombat>();
        _UnitStats = GetComponent<T_UnitStats>();
        _UnitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_UnitAttributes);

        _uniMovementStates = UnitMovementState.StopMoving;

        _UIManager.Event_BattleStart += OnBattleStartEvent;
        _LevelManager.Event_GameOver += OnGameOverEvent;
    }
    private void Update()
    {
        if (!_isActive) return;


        switch (_uniMovementStates)
        {
            case UnitMovementState.OnMoving:
                _UnitCombatMgr.G_LookingForOpponents();
                UnitMovement(_unitMoveSpeed);
                break;

            case UnitMovementState.StopMoving:
                UnitMovement(0f);
                break;
        }

    }
    #endregion
    #region ========================= Methods =================
    void SwitchMovementState(UnitMovementState st) => _uniMovementStates = st;

    void InitializeUnitAttributes(UnitAttribute uatb)
    {
        _unitMoveSpeed = _UnitStats._UnitMoveSpeed;
    }

    #region -------------- Event Methods ---------------------------

    void OnBattleStartEvent()
    {
        _isActive = true;
    }

    void OnGameOverEvent()
    {
        _isActive = false;
    }
    #endregion
    #region -------------------- Movement ----------------------
    void UnitMovement(float moveSpeed)
    {
        if (_UnitStats.G_IsEnemyUnit()) MoveToTarget(_LevelManager.G_GetFriendList(), moveSpeed);
        else MoveToTarget(_LevelManager.G_GetEnemyList(), moveSpeed);
    }

    void MoveToTarget(List<T_UnitCombat> opponents, float moveSpeed)
    {
        _agent.speed = moveSpeed;
        //* Check if still opponent unit left
        if (opponents.Count < 1)
        {
            SwitchMovementState(UnitMovementState.StopMoving);
            return;
        }

        T_UnitCombat target = GetClosestOpponentUnit(opponents);

        // Move to opponent
        Vector3 targetPosition = target.transform.position;
        _agent.destination = targetPosition;
    }

    //* Based on the unit list, look for the closest unit returned as target
    T_UnitCombat GetClosestOpponentUnit(List<T_UnitCombat> units)
    {
        if (units.Count < 1) return null;

        List<float> distances = new();
        Dictionary<float, T_UnitCombat> unitDic = new();
        foreach (var unit in units)
        {
            float dis = Vector3.Distance(unit.transform.position, this.transform.position);
            distances.Add(dis);
            unitDic.Add(dis, unit);

        }
        float targetDistance = Mathf.Min(distances.ToArray());
        _moveToTarget = unitDic[targetDistance];
        return unitDic[targetDistance];
    }

    void StopMovement()
    {
        _agent.speed = 0f;
    }
    #endregion



    #endregion
}
