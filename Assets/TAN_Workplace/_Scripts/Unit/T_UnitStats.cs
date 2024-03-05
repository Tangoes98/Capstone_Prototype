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
    public float _damageValue;
    public float _unitMoveSpeed;



    #endregion
    #region ======================== Public ==============================
    public UnitAttribute G_GetUnitAttributes() => _unitAttribute;
    public bool G_IsEnemyUnit() => _isEnemy;






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
    }







}
