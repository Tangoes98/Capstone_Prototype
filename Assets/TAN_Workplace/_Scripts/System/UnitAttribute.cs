using System;

[Serializable]
public struct UnitAttribute
{
    // string UnitName;
    // string UnitBindA;
    // int UnitBindALevel;
    // string UnitBindB;
    // int UnitBindBLevel;
    // float Health;
    // float Attack;
    // float Defence;
    // float AttackSpeed;
    // float CriticalHitRate;
    // float SkillStrength;
    // float MaxEnergy;
    // string ActiveSkill;
    // float SkillDuration;
    // float Range;
    // float EnergyAutoRecovery;
    // float EnergyDamageRecovery;

    public string UnitName;
    public string UnitBindA;
    public int UnitBindALevel;
    public string UnitBindB;
    public int UnitBindBLevel;

    public float Health;
    public float Shield;
    public float Attack;
    public float Defence;
    public float AttackSpeed;
    public float CriticalHitRate;

    public float SkillPower;
    public float MaxEnergy;
    public string SkillAutoName;
    public float SkillDuration;
    public float Range;
    public float EnergyAutoRecovery;
    public float EnergyPerDamageRecovery;

    public UnitAttribute(
        string UnitName,
        string UnitBindA,
        int UnitBindALevel,
        string UnitBindB,
        int UnitBindBLevel,

        float Health,
        float Shield,
        float Attack,
        float Defence,
        float AttackSpeed,
        float CriticalHitRate,

        float SkillPower,
        float MaxEnergy,
        string SkillAutoName,
        float SkillDuration,
        float Range,
        float EnergyAutoRecovery,
        float EnergyPerDamageRecovery
        )
    {
        this.UnitName = UnitName;
        this.UnitBindA = UnitBindA;
        this.UnitBindALevel = UnitBindALevel;
        this.UnitBindB = UnitBindB;
        this.UnitBindBLevel = UnitBindBLevel;

        this.Health = Health;
        this.Shield = Shield;
        this.Attack = Attack;
        this.Defence = Defence;
        this.AttackSpeed = AttackSpeed;
        this.CriticalHitRate = CriticalHitRate;

        this.SkillPower = SkillPower;
        this.MaxEnergy = MaxEnergy;
        this.SkillAutoName = SkillAutoName;
        this.SkillDuration = SkillDuration;
        this.Range = Range;
        this.EnergyAutoRecovery = EnergyAutoRecovery;
        this.EnergyPerDamageRecovery = EnergyPerDamageRecovery;
    }
}
