using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    #region param

    protected int cost;
    protected DamageType type = DamageType.Damage;
    public string CastAnimation;
    public string TargetsAnimation;

    public int Cost { get { return cost; } }
    public DamageType DamageType { get { return type; } }

    #endregion

    public Skill()
    {
    }
    virtual public int GetDamage(Combatant user, Combatant target, float resultMultiplier)
    {
        return 0;
    }

    virtual public void ApplySelfEffect(Combatant user, float resultMultiplier)
    {

    }

    virtual public void ApplyTargetEffect(Combatant user, Combatant target, float resultMultiplier)
    {
        
    }

    virtual public string GetDescription()
    {
        return "";
    }
}

public enum DamageType
{
    Damage,
    Heal,
    None,
}