using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatant
{
    protected int playerID;
    
    protected int position;

    #region base Stat

    protected int maxHp;
    protected int maxMp;
    protected int hp;
    protected int mp;
    protected int atk;
    protected int ap;

    public int MaxHP { get { return maxHp; } }
    public int MaxMP { get { return maxMp; } }
    public int HP { get { return hp; } }
    public int MP { get { return mp; } }
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

    #endregion
}
