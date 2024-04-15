using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill
{
    #region param

    protected Combatant owner;
    protected int cost;

    protected int Cost { get { return cost; } }

    #endregion

    public Skill(Combatant owner)
    {
        this.owner = owner;
    }
    abstract public double GetMultiplier();
    abstract public void executeAction(Combatant target, double resultMultiplier);

}
