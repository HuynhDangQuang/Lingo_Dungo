using Assets.Core.Resource.Skills.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Combatant
{
    public Enemy(EnemyTypes enemyType)
    {
        SetupClass(enemyType);
        UpdateStat();
        hp = maxHp;
    }

    public void SetupClass(EnemyTypes enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypes.AlphaMonster:
                ownerClass = new Class(100, 100, 10, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                break;
        }
    }
}

public enum EnemyTypes
{
    AlphaMonster,
}