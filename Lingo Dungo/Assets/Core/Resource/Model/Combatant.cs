using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
    protected int playerID;

    protected int position;

    public GameObject Model;
    public string modelId = "Actor";

    public Class ownerClass;

    #region base Stat

    protected int maxHp;
    protected int maxMp;
    protected int hp;
    protected int mp;
    protected int atk;
    protected int ap;
    protected int barrier;

    public bool JustGainBarrier = false;

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


    #region get set
    public Skill NormalAttack
    {
        get { return ownerClass.normalAttack; }
    }

    public Skill PrimarySkill
    {
        get { return ownerClass.primarySkill; }
    }

    public Skill SecondarySkill
    {
        get { return ownerClass.secondarySkill; }
    }

    public float DamageTakenRate
    {
        get
        {
            return 1;
        }
    }

    public float MinCriticalTimeRate
    {
        get
        {
            return 0.5f;
        }
    }

    public float CriticalDamageBonus
    {
        get
        {
            return 1.5f;
        }
    }

    public int Barrier
    {
        get
        {
            return barrier;
        }
        set
        {
            barrier = Mathf.Max(value, 0);
        }
    }

    public bool IsDied
    {
        get
        {
            return hp == 0;
        }
    }
    #endregion

    #region Functions

    virtual public void GainHp(int value)
    {
        hp = Mathf.Clamp(hp + value, 0, maxHp);
    }

    public void GainMp(int value)
    {
        mp = Mathf.Clamp(mp + value, 0, maxMp);
    }

    public void GainBarrier(int value)
    {
        value = Mathf.Max(0, value);
        JustGainBarrier = true;
        Barrier += value;
    }

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
