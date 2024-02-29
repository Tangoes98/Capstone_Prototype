using System.Collections;
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

    [SerializeField] GameObject _enemySpawner;
    [SerializeField] GameObject _playerSpawner;
    [SerializeField] T_Unit[] _enemyArray;
    [SerializeField] List<T_Unit> _enemyList;











    #endregion
    #region =================== Public ============================

    //public T_Unit[] G_GetEnemies() => _enemyArray;
    public List<T_Unit> G_GetEnemyList() => _enemyList;










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
        T_Unit[] enemies = _enemySpawner.GetComponentsInChildren<T_Unit>();
        _enemyList = new(enemies);
    }















    #endregion

}
