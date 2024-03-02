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
    [SerializeField] List<T_Unit> _enemyList;
    [SerializeField] List<T_Unit> _friendList;











    #endregion
    #region =================== Public ============================

    public List<T_Unit> G_GetEnemyList() => _enemyList;
    public List<T_Unit> G_GetFriendList() => _friendList;










    #endregion
    #region ================ MonoBehaviour =======================
    private void Start()
    {
        GetAllEnemies();

    }
    private void Update()
    {

    }
    #endregion
    #region =============== Methods =======================

    void GetAllEnemies()
    {
        T_Unit[] units = _unitSpawner.GetComponentsInChildren<T_Unit>();
        foreach (var unit in units)
        {
            if (unit.G_IsEnemyUnit()) _enemyList.Add(unit);
            else _friendList.Add(unit);
        }






    }















    #endregion

}
