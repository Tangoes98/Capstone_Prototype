using System;
using UnityEngine;

public class T_UnitHealth : MonoBehaviour
{

    #region ============= Variables =================
    T_UnitStats _UnitStats;
    UnitAttribute _unitAttributes;



    [SerializeField] bool _isActive;
    [Header("DEBUG_VIEW")]
    [SerializeField] bool _isDead;

    [Header("DEBUG: Health")]
    [SerializeField] float _health;
    [SerializeField] float _shield;
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
        _UnitStats = GetComponent<T_UnitStats>();
        _unitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_unitAttributes);

        _maxHealth = _health;
        _isDead = false;
    }


    void Update()
    {

    }
    #endregion
    #region ============== Methods =================
    void InitializeUnitAttributes(UnitAttribute uatb)
    {
        _health = uatb.Health;
        _shield = uatb.Shield;

    }

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
