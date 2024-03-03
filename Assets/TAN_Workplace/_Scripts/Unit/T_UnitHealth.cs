using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_UnitHealth : MonoBehaviour
{

    #region ============= Variables =================

    [Header("Field_value_required")]
    [SerializeField] float _health;

    [Header("DEBUG_VIEW")]
    [SerializeField] bool _isDead;
    float _maxHealth;

    #endregion
    #region ================== Public =====================
    public void G_DealDamage(float damage) => TakeDamage(damage);
    public bool G_IsDead() => _isDead;
    public float G_HealthImageFillAmount() => GetHealthValueNormalized();

    public event Action<float> Take_Damage_Event;




    #endregion
    #region ============= MonoBehaviour =================

    void Start()
    {
        _maxHealth = _health;
        _isDead = false;
    }


    void Update()
    {

    }
    #endregion
    #region ============== Functions =================
    void TakeDamage(float damage)
    {
        _health -= damage;
        Take_Damage_Event?.Invoke(damage);
        if (_health <= 0) _health = 0;
        if (_health == 0) UnitDied();
    }

    void UnitDied()
    {
        _health = 0;
        _isDead = true;
        this.gameObject.SetActive(false);
    }

    float GetHealthValueNormalized() => _health / _maxHealth;



    #endregion











}
