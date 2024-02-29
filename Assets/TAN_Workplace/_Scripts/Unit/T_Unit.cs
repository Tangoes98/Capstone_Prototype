using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class T_Unit : MonoBehaviour
{
    enum UnitState
    {
        Idle,
        Searching,
        Moving,
        Combat,
        SkillAction,
        Waiting
    }

    #region ============= Variables =============
    [Header("DEBUG_VIEW")]
    [SerializeField] UnitState _unitState;
    [SerializeField] int _health;
    [SerializeField] float _viewRange;
    [SerializeField] float _attackRange;

    T_LevelManager _LevelManager;

    [Header("Movement")]
    // [SerializeField] float _stopDistance;
    // [SerializeField] float _moveSpeed;
    // [SerializeField] float _rotateSpeed;
    NavMeshAgent _agent;




    #endregion
    #region ================= MonoBehaviour ======================
    void Start()
    {
        Init();
        _unitState = UnitState.Idle;
    }

    void Update()
    {

        switch (_unitState)
        {
            case UnitState.Idle:
                break;

            case UnitState.Searching:
                break;

            case UnitState.Moving:
                if (_LevelManager.G_GetEnemyList().Count < 1)
                    _unitState = UnitState.Waiting;

                UnitMovement(_LevelManager.G_GetEnemyList());
                break;

            case UnitState.Combat:
                break;

            case UnitState.SkillAction:
                break;
                
            case UnitState.Waiting:
                _agent.destination = transform.position;
                break;
        }
    }
    #endregion
    #region ============= Method ===============
    void Init()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _LevelManager = T_LevelManager.Instance;
    }

    void UpdateUnitState()
    {

    }
    #region -------------- Searching ------------------






    #endregion

    #region -------------------- Movement ----------------------

    // bool IsAbleToAttack()

    void UnitMovement(List<T_Unit> enemies)
    {
        // Get closest enemy target
        T_Unit target = GetClosestOpponentUnitPosition(enemies);

        // if no opponent target left, going to wait state
        if (!target) return;

        // Check if target is a valid target
        if (!target.gameObject.activeSelf)
            _LevelManager.G_GetEnemyList().Remove(target);

        // Move to target
        Vector3 targetPosition = target.transform.position;
        _agent.destination = targetPosition;


    }
    T_Unit GetClosestOpponentUnitPosition(List<T_Unit> units)
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


    //* To calculate the center point of all enemy units in the scene.
    // Vector3 CalculateMoveCenterDirection(T_Unit[] units)
    // {
    //     Vector3 centerVec = new(0f, 0f, 0f);

    //     float totalX = 0f;
    //     float totalZ = 0f;
    //     foreach (var unit in units)
    //     {
    //         totalX += unit.transform.position.x;
    //         totalZ += unit.transform.position.z;
    //     }

    //     centerVec = new(totalX / units.Length, 0f, totalZ / units.Length);

    //     return centerVec;
    // }

    #endregion






    #endregion






}
