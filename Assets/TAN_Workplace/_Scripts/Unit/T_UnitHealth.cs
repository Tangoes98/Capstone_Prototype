using System;
using UnityEngine;

public class T_UnitHealth : MonoBehaviour
{

    #region ============= Private =================
    T_UnitStats _UnitStats;
    UnitAttribute _UnitAttributes;
    T_UnitCombat _UnitCombatManager;



    [SerializeField] bool _isActive;
    [Header("DEBUG_VIEW")]
    [SerializeField] bool _isDead;

    [Header("DEBUG: Health")]
    [SerializeField] float _health;
    float _maxHealth;
    [SerializeField] float _shield;
    float _maxShield;

    #endregion
    #region ================== Public =====================
    public void G_DealDamage(float damage) => TakeDamage(damage);
    public float G_HealthImageFillAmount() => GetHealthValueNormalized();
    public float G_ShieldImageFillAmount() => GetShieldValueNormalized();

    public event Action<float> Take_Damage_Event;




    #endregion
    #region ============= MonoBehaviour =================

    void Start()
    {
        _UnitStats = GetComponent<T_UnitStats>();
        _UnitAttributes = _UnitStats.G_GetUnitAttributes();
        InitializeUnitAttributes(_UnitAttributes);

        _UnitCombatManager = GetComponent<T_UnitCombat>();

        _maxHealth = _health;
        _maxShield = _shield;
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
        if (!IsAllDamageSheilded(damage, out float leftOverDamage))
            _health -= leftOverDamage;

        Take_Damage_Event?.Invoke(damage);
        if (_health <= 0) _health = 0;
        if (_health == 0) UnitDied();
    }

    bool IsAllDamageSheilded(float damage, out float damage_leftover)
    {
        _shield -= damage;
        float dmg = _shield - damage;
        //* Shielded all damage
        if (dmg >= 0)
        {
            damage_leftover = 0f;
            return true;
        }
        else
        {
            _shield = 0;
            damage_leftover = Mathf.Abs(dmg);
            return false;
        }
    }


    void UnitDied()
    {
        _health = 0;
        _isDead = true;
        _UnitCombatManager.G_SetIsUnitDead(_isDead);

        this.gameObject.SetActive(false);
    }

    float GetHealthValueNormalized() => _health / _maxHealth;
    float GetShieldValueNormalized() => _shield / _maxShield;



    #endregion











}
