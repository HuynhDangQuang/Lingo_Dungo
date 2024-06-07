using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class
{
    #region Base Stat

    public string name;
    //public int level = 1;
    public int maxHp;
    public int maxMp;
    public int atk;
    public int ap;
    public Skill normalAttack;
    public Skill primarySkill;
    public Skill secondarySkill;

    #endregion
    public Class(int maxHp, int maxMp, int atk, int ap)
    {
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.atk = atk;
        this.ap = ap;
    }
}
