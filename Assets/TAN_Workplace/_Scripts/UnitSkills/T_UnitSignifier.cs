using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_UnitSignifier : MonoBehaviour
{

    [SerializeField] GameObject _tauntSignifier;






    public void G_SetTauntSignifier(bool b_value) => SetTauntSignifier(b_value);


    private void Start()
    {
        SetTauntSignifier(false);
    }
    

    void SetTauntSignifier(bool b_value)
    {
        _tauntSignifier.SetActive(b_value);
    }



}
