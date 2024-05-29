using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    #region param

    protected Combatant owner;
    protected int cost;
    protected DamageType type = DamageType.Damage;
    public readonly string CastAnimation;
    public readonly string TargetsAnimation;

    public int Cost { get { return cost; } }

    #endregion

    public Skill(Combatant owner)
    {
        this.owner = owner;
    }
    abstract public double GetDamage(Combatant target);

    abstract public void executeAction(Combatant target, double resultMultiplier);
}

public enum DamageType
{
    Damage,
    Heal
}