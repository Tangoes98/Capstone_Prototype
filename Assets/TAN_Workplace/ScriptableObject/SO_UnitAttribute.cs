using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/CreatNewUnitAttributeScriptableObject", order = 1)]
public class SO_UnitAttribute : ScriptableObject
{
    // !Should never modify variables during playmode! // 
    public string UnitName;
    public string UnitBindA;
    public int UnitBindALevel;
    public string UnitBindB;
    public int UnitBindBLevel;
    public float Health;
    public float Attack;
    public float Defence;
    public float AttackSpeed;
    public float CriticalHitRate;
    public float SkillStrength;
    public float MaxEnergy;
    public string ActiveSkill;
    public float SkillDuration;
    public float Range;
    public float EnergyAutoRecovery;
    public float EnergyDamageRecovery;

    // public string UnitName { get; private set; }
    // public string UnitBindA { get; private set; }
    // public int UnitBindALevel { get; private set; }
    // public string UnitBindB { get; private set; }
    // public int UnitBindBLevel { get; private set; }
    // public float Health { get; private set; }
    // public float Attack { get; private set; }
    // public float Defence { get; private set; }
    // public float AttackSpeed { get; private set; }
    // public float CriticalHitRate { get; private set; }
    // public float SkillStrength { get; private set; }
    // public float MaxEnergy { get; private set; }
    // public string ActiveSkill { get; private set; }
    // public float SkillDuration { get; private set; }
    // public float Range { get; private set; }
    // public float EnergyAutoRecovery { get; private set; }
    // public float EnergyDamageRecovery { get; private set; }


    // public TextAsset jsonFile;
    // public SO_UnitAttribute so_unitAttribute;

    // private void OnEnable()
    // {
    //     so_unitAttribute = JsonUtility.FromJson<SO_UnitAttribute>(jsonFile.text);
    // }

}
