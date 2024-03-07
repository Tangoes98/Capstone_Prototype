using UnityEngine;

public class T_UnitStats : MonoBehaviour
{

    #region ==================== Private =============================

    [Header("REFERENCE NEEDED")]
    [SerializeField] SO_UnitAttribute _unitAttributeSO;
    [SerializeField] UnitAttribute _unitAttribute;

    [Header("FIELD VALUE REQUIRED")]
    [SerializeField] bool _isEnemy;

    [Header("TEMPORARY VALUE REQUIRED")]
    public float _DamageValue;
    public float _UnitMoveSpeed;
    public float _AttackDuration;

    [Header("DEBUG ONLY")]
    [SerializeField] bool _isDead;
    [SerializeField] int _skillIndex;


    #endregion
    #region ======================== Public ==============================
    public T_UnitCombat UnitCombat { get; private set; }
    public T_UnitSkillAction UnitSkillAction { get; private set; }
    public T_UnitHealth UnitHealth { get; private set; }
    public T_UnitMovement UnitMovement { get; private set; }
    public T_UnitLocalUI UnitLocalUI { get; private set; }
    public T_UnitSignifier UnitSignifier { get; private set; }


    public UnitAttribute G_GetUnitAttributes() => _unitAttribute;
    public bool G_IsEnemyUnit() => _isEnemy;

    public bool G_GetIsUnitDead() => _isDead;
    public void G_SetIsUnitDead(bool bvalue) => _isDead = bvalue;

    public int G_GetSkillIndex() => _skillIndex;

    #endregion =======================================================
    private void Awake()
    {
        _unitAttribute = new UnitAttribute
    (
        _unitAttributeSO.UnitName,
        _unitAttributeSO.UnitBindA,
        _unitAttributeSO.UnitBindALevel,
        _unitAttributeSO.UnitBindB,
        _unitAttributeSO.UnitBindBLevel,

        _unitAttributeSO.Health,
        _unitAttributeSO.Shield,
        _unitAttributeSO.Attack,
        _unitAttributeSO.Defence,
        _unitAttributeSO.AttackSpeed,
        _unitAttributeSO.CriticalHitRate,

        _unitAttributeSO.SkillPower,
        _unitAttributeSO.MaxEnergy,
        _unitAttributeSO.SkillAutoName,
        _unitAttributeSO.SkillDuration,
        _unitAttributeSO.Range,
        _unitAttributeSO.EnergyAutoRecovery,
        _unitAttributeSO.EnergyPerDamageRecovery
    );

        UnitCombat = GetComponent<T_UnitCombat>();
        UnitSkillAction = GetComponent<T_UnitSkillAction>();
        UnitHealth = GetComponent<T_UnitHealth>();
        UnitMovement = GetComponent<T_UnitMovement>();
        UnitLocalUI = GetComponentInChildren<T_UnitLocalUI>();
        UnitSignifier = GetComponentInChildren<T_UnitSignifier>();
    }









}
