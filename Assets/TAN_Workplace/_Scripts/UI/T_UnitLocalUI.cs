using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class T_UnitLocalUI : MonoBehaviour
{
    [Header("Field_value_required")]
    [SerializeField] Canvas _unitCanvas;
    [SerializeField] Image _healthImage;
    [SerializeField] Image _attackCDImage;
    [SerializeField] TextMeshProUGUI _regularDamageText;
    [SerializeField] TextMeshProUGUI _skillActionText;
    [SerializeField] Image _energyBarImage;


    T_UnitCombatManager _Unit;
    T_UnitHealth _UnitHealth;
    T_UnitSkillAction _UnitSkillAction;

    #region ======================== Public =========================
    public void G_UpdateSkillActionText(float f) => UpdateSkillActionText(f);

    #endregion =======================================================

    private void Start()
    {
        _Unit = GetComponentInParent<T_UnitCombatManager>();
        _UnitHealth = GetComponentInParent<T_UnitHealth>();
        _UnitSkillAction = GetComponentInParent<T_UnitSkillAction>();

        _UnitHealth.Take_Damage_Event += UpdateRegularDamagePopupsValue;
    }

    private void Update()
    {
        UpdateCanvasFacingDirection();
        UpdateHealthBarFillAmount();
        UpdateUnitAttckCDImage();
        UpdateEnergyBarFillAmount();
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
        _attackCDImage.fillAmount = 1 - _Unit.G_GetAttackCD_UIFillAmount();
    }

    void UpdateRegularDamagePopupsValue(float f)
    {
        if (f == 0) _regularDamageText.text = "Miss";
        else _regularDamageText.text = f.ToString();
        StartCoroutine(Wait(1.5f));
    }
    void UpdateSkillActionText(float f)
    {
        if (f == 0) _skillActionText.text = "Miss";
        else _skillActionText.text = f.ToString();
        StartCoroutine(Wait(2f));
    }
    IEnumerator Wait(float time)
    {
        var waitTime = new WaitForSeconds(time);
        yield return waitTime;
        _regularDamageText.text = " ";
    }

    void UpdateEnergyBarFillAmount()
    {
        _energyBarImage.fillAmount = _UnitSkillAction.G_GetEnergyFillAmount();
    }




    #endregion





}
