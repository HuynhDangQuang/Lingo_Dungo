using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
    protected int playerID;
    
    protected int position;

    public GameObject Model;

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

    public void gainHp(int value)
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

    public void gainMp(int value)
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

    public void AttachModel(GameObject model)
    {
        Model = model;
        model.GetComponent<Model>().owner = this;
    }
    #endregion
}
