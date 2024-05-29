using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
    protected int playerID;

    protected int position;

    public GameObject Model;

    public Class ownerClass;

    #region base Stat

    protected int maxHp;
    protected int maxMp;
    protected int hp;
    protected int mp;
    protected int atk;
    protected int ap;

    public int MaxHP
    {
        get { return maxHp; }
        set
        {
            maxHp = Mathf.Max(value, 1);
        }
    }
    public int MaxMP
    {
        get { return maxMp; }
        set
        {
            maxMp = Mathf.Max(value, 1);
        }
    }
    public int HP
    {
        get { return hp; }
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
        }
    }
    public int MP
    {
        get { return mp; }
        set
        {
            mp = Mathf.Clamp(value, 0, maxMp);
        }
    }
    public int ATK { get { return atk; } }
    public int AP { get { return ap; } }
    #endregion

    #region Functions

    public void GainHp(int value)
    {
        hp += value;
        if (hp < 0)
        {
            hp = 0;
        }
        else if (hp > maxHp)
        {
            hp = maxHp;
        }
    }

    public void GainMp(int value)
    {
        mp += value;
        if (mp < 0)
        {
            mp = 0;
        }
        else if (mp > maxMp)
        {
            mp = maxMp;
        }
    }

    #region get set

    public Skill NormalAttack
    {
        get { return ownerClass.normalAttack; }
    }

    public Skill PrimarySkill
    {
        get {  return ownerClass.primarySkill; }
    }

    public Skill SecondarySkill
    {
        get { return ownerClass.secondarySkill; }
    }

    #endregion

    public void UpdateStat()
    {
        // Re-caculate the stat
        maxHp = ownerClass.maxHp;
        maxMp = ownerClass.maxMp;
        atk = ownerClass.atk;
        ap = ownerClass.ap;

        // fix the HP, MP 
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        if (mp > maxMp)
        {
            mp = maxMp;
        }    
    }

    public void AttachModel(GameObject model)
    {
        Model = model;
        model.GetComponent<Model>().owner = this;
    }
    #endregion
}
