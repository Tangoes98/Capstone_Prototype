using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class T_UnitLocalUI : MonoBehaviour
{
    [Header("Field_value_required")]
    [SerializeField] Canvas _unitCanvas;
    [SerializeField] Image _healthImage;
    [SerializeField] Image _attackCDImage;

    [SerializeField] Transform _damagePopup_parent;
    [SerializeField] Transform _damagePopup;
    // [SerializeField] TextMeshProUGUI _regularDamageText;
    // [SerializeField] Animator _dmg_animator;

    [SerializeField] TextMeshProUGUI _skillDurationText;
    [SerializeField] TextMeshProUGUI _combatDurationText;
    [SerializeField] Image _energyBarImage;
    [SerializeField] Image _shieldImage;


    T_UnitCombat _UnitCombatMgr;
    T_UnitHealth _UnitHealth;
    T_UnitSkillAction _UnitSkillAction;
    T_UnitStats _UnitStats;

    #region ======================== Public =========================

    #endregion =======================================================

    private void Start()
    {
        _UnitCombatMgr = GetComponentInParent<T_UnitCombat>();
        _UnitHealth = GetComponentInParent<T_UnitHealth>();
        _UnitSkillAction = GetComponentInParent<T_UnitSkillAction>();
        _UnitStats = GetComponentInParent<T_UnitStats>();

        _UnitHealth.Take_Damage_Event += RegularDamagePopUp;
    }

    private void Update()
    {
        UpdateCanvasFacingDirection();
        UpdateHealthBarFillAmount();
        UpdateUnitAttckCDImage();
        UpdateEnergyBarFillAmount();
        UpdateSkillDurationText();
        UpdateCombatDurationText();
        UpdateShieldFillAmount();
    }


    #region =============== Methods =================================

    void UpdateCanvasFacingDirection()
    {
        _unitCanvas.transform.forward = Camera.main.transform.forward;
    }

    void UpdateHealthBarFillAmount()
    {
        _healthImage.fillAmount = _UnitHealth.G_HealthImageFillAmount();
    }

    void UpdateUnitAttckCDImage()
    {
        _attackCDImage.fillAmount = 1 - _UnitCombatMgr.G_GetAttackCD_UIFillAmount();
    }

    // void UpdateRegularDamagePopupsValue(float f)
    // {
    //     _dmg_animator.SetTrigger("Popup_anim");
    //     if (f == 0) _regularDamageText.text = "Miss";
    //     else _regularDamageText.text = f.ToString();
    //     if (_UnitCombatMgr.G_GetIsUnitDead()) return;
    //     StartCoroutine(Wait(1.5f));
    // }

    void RegularDamagePopUp(float d)
    {
        GameObject dmg_obj = Instantiate(_damagePopup, _damagePopup_parent).gameObject;
        dmg_obj.GetComponent<Animator>().SetTrigger("Popup_anim");

        if (d < 1)
            dmg_obj.GetComponent<TextMeshProUGUI>().text = "MISS";
        else
            dmg_obj.GetComponent<TextMeshProUGUI>().text = d.ToString();

        if (_UnitStats.G_GetIsUnitDead()) return;
        StartCoroutine(Wait(1.5f, dmg_obj));
    }

    IEnumerator Wait(float time, GameObject obj)
    {
        var waitTime = new WaitForSeconds(time);
        yield return waitTime;
        Destroy(obj);
    }
    // IEnumerator Wait(float time)
    // {
    //     var waitTime = new WaitForSeconds(time);
    //     yield return waitTime;
    //     _dmg_animator.ResetTrigger("Popup_anim");
    //     _regularDamageText.text = " ";
    // }

    void UpdateEnergyBarFillAmount()
    {
        _energyBarImage.fillAmount = _UnitSkillAction.G_GetEnergyFillAmount();
    }

    void UpdateSkillDurationText()
    {
        if (_UnitSkillAction.G_GetState() == UnitSkillActionState.SkillDuration)
            _skillDurationText.text = "SKILL_ACTING";
        else _skillDurationText.text = " ";
    }
    void UpdateCombatDurationText()
    {
        if (_UnitCombatMgr.G_GetState() == UnitCombatState.CombatDuration)
            _combatDurationText.text = "COMBT_ACTING";
        else _combatDurationText.text = " ";
    }

    void UpdateShieldFillAmount()
    {
        _shieldImage.fillAmount = _UnitHealth.G_ShieldImageFillAmount();
    }




    #endregion





}
