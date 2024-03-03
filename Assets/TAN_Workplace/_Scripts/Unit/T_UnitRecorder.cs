using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitRecorder : MonoBehaviour
{
    #region ================= Variables ============================= 

    T_Unit t_Unit;
    T_UnitHealth t_UnitHealth;

    List<float> _totalDamageTaken;




    #endregion ===============================================
    private void Start()
    {
        t_Unit = GetComponentInParent<T_Unit>();
        t_UnitHealth = GetComponentInParent<T_UnitHealth>();
        t_UnitHealth.Take_Damage_Event += UpdateUnitTakenDamage;

        T_LevelManager.Instance.Event_GameOver += OnGameOverEvent;
    }

    private void Update()
    {

    }

    #region ==================== Event Methods =============================

    void OnGameOverEvent()
    {
        Debug.Log(CalculateTotalDamge(_totalDamageTaken));
    }




    #endregion
    #region ================= Methods =================================

    void UpdateUnitTakenDamage(float f)
    {
        _totalDamageTaken.Add(f);
    }




    float CalculateTotalDamge(List<float> floats)
    {
        float sum = 0f;
        for (int i = 0; i < floats.Count; i++)
        {
            sum += i;
        }
        return sum;
    }





    #endregion






}
