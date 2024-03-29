using System;
using System.Collections.Generic;
using UnityEngine;

public class T_LevelManager : MonoBehaviour
{
    #region =============== Instance =======================
    public static T_LevelManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances occured");
            Destroy(Instance);
        }
        Instance = this;

    }
    #endregion

    #region =============== Variables =======================

    [SerializeField] GameObject _unitSpawner;
    [SerializeField] List<T_UnitStats> _allUnits;
    [SerializeField] List<T_UnitStats> _enemyList;
    [SerializeField] List<T_UnitStats> _friendList;











    #endregion
    #region =================== Public ============================

    public List<T_UnitStats> G_GetEnemyList() => _enemyList;
    public List<T_UnitStats> G_GetFriendList() => _friendList;
    public List<T_UnitStats> G_GetAllUnitList() => _allUnits;


    public event Action Event_GameOver;







    #endregion
    #region ================ MonoBehaviour =======================
    private void Start()
    {
        GetAllUnitIntoUnitList();

    }
    private void Update()
    {
        CheckUnitListEmptiness();
    }
    #endregion
    #region =============== Methods =======================

    void GetAllUnitIntoUnitList()
    {
        T_UnitStats[] units = _unitSpawner.GetComponentsInChildren<T_UnitStats>();
        _allUnits = new(units);
        foreach (var unit in units)
        {
            if (unit.G_IsEnemyUnit()) _enemyList.Add(unit);
            else _friendList.Add(unit);
        }
    }
    void CheckUnitListEmptiness()
    {
        if (IsUnitListEmpty(_enemyList) || IsUnitListEmpty(_friendList))
        {
            // Game Over
            Event_GameOver?.Invoke();
        }
        if (IsUnitListEmpty(_enemyList))
        {
            // Player wins

        }
        if (IsUnitListEmpty(_friendList))
        {
            // Enemy wins

        }


    }


    bool IsUnitListEmpty(List<T_UnitStats> untiList)
    {
        if (untiList.Count < 1) return true;
        else return false;
    }















    #endregion

}
