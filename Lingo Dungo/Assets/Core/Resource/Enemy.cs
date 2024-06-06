using Assets.Core.Resource.EnemyLogic;
using Assets.Core.Resource.Skills.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Combatant
{
    public float answerRate;
    public float answerVariance;
    public int mpRegen = 10;
    public EnemyLogicBase logic;

    public Enemy(EnemyTypes enemyType)
    {
        SetupClass(enemyType);
        UpdateStat();
        hp = maxHp;
    }

    #region Functions

    public float GetFinalAnswerTimeRate()
    {
        return Mathf.Clamp(answerRate * (1 + Random.Range(-answerVariance, answerVariance)), 0, 1);
    }

    public override void GainHp(int value)
    {
        base.GainHp(value);
        if (value < 0)
        {
            mp = Mathf.Clamp(mp + value, 0, maxMp);
        }
    }

    public Action RegisterAction()
    {
        CombatManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CombatManager>();
        if (mp == maxMp)
        {
            Action.SkillType? result = logic.ChooseAction();
            switch (result)
            {
                case Action.SkillType.NormalAttack:
                {
                    return new Action(
                        GetFinalAnswerTimeRate(),
                        manager.RoundCorrectAnswer,
                        this,
                        new List<Combatant>() { manager.ThisPlayer },
                        NormalAttack,
                        Action.SkillType.NormalAttack,
                        NormalAttack.CastAnimation,
                        NormalAttack.TargetsAnimation
                    );
                }
                case Action.SkillType.PrimarySkill:
                {
                    return new Action(
                        GetFinalAnswerTimeRate(),
                        manager.RoundCorrectAnswer,
                        this,
                        new List<Combatant>() { manager.ThisPlayer },
                        PrimarySkill,
                        Action.SkillType.PrimarySkill,
                        PrimarySkill.CastAnimation,
                        PrimarySkill.TargetsAnimation
                    );
                }
                case Action.SkillType.SecondarySkill:
                {
                    return new Action(
                        GetFinalAnswerTimeRate(),
                        manager.RoundCorrectAnswer,
                        this,
                        new List<Combatant>() { manager.ThisPlayer },
                        SecondarySkill,
                        Action.SkillType.SecondarySkill,
                        SecondarySkill.CastAnimation,
                        SecondarySkill.TargetsAnimation
                    );
                }
            }
        }
        return null;
    }
    #endregion

    public void SetupClass(EnemyTypes enemyType)
    {
        switch (enemyType)
        {
            case EnemyTypes.MonsterA:
                ownerClass = new Class(250, 10, 50, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 20;
                answerRate = 0.4f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                modelId = "MonsterA";
                break;

            case EnemyTypes.MonsterB:
                ownerClass = new Class(350, 100, 100, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 150;
                answerRate = 0.3f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                modelId = "MonsterB";
                break;

            case EnemyTypes.MonsterC:
                ownerClass = new Class(300, 50, 60, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 100;
                answerRate = 0.5f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                modelId = "MonsterC";
                break;

            case EnemyTypes.MonsterI:
                ownerClass = new Class(150, 50, 40, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 120;
                answerRate = 0.6f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                modelId = "MonsterI";
                break;

            case EnemyTypes.MonsterO:
                ownerClass = new Class(250, 20, 100, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 50;
                answerRate = 0.6f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                modelId = "MonsterO";
                break;
        }
    }
}

public enum EnemyTypes
{
    MonsterA,
    MonsterB,
    MonsterC,
    MonsterI,
    MonsterO
}