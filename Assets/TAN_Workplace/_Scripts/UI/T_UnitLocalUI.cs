using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class T_UnitLocalUI : MonoBehaviour
{
    [Header("Field_value_required")]
    [SerializeField] Canvas _unitCanvas;
    [SerializeField] Image _healthImage;
    [SerializeField] Image _attackCDImage;


    T_Unit t_Unit;
    T_UnitHealth t_UnitHealth;






    private void Start()
    {
        t_Unit = GetComponentInParent<T_Unit>();
        t_UnitHealth = GetComponentInParent<T_UnitHealth>();
    }

    private void Update()
    {
        UpdateCanvasFacingDirection();
        UpdateHealthBarFillAmount();
        UpdateUnitAttckCDImage();
    }


    #region =============== Methods =================================

    void UpdateCanvasFacingDirection()
    {
        _unitCanvas.transform.forward = Camera.main.transform.forward;
    }

    void UpdateHealthBarFillAmount()
    {
        _healthImage.fillAmount = t_UnitHealth.G_HealthImageFillAmount();
    }

    void UpdateUnitAttckCDImage()
    {
        _attackCDImage.fillAmount = 1 - t_Unit.G_GetAttackCD_UIFillAmount();
    }





    #endregion





}
