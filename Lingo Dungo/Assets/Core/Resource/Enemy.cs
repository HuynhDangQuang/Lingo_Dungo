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
            case EnemyTypes.AlphaMonster:
                ownerClass = new Class(100, 10, 50, 10)
                {
                    normalAttack = new EnemyDefault_NormalAttack()
                };
                mpRegen = 20;
                answerRate = 0.4f;
                answerVariance = 0.2f;
                logic = new EnemyLogic_Default(this);
                break;
        }
    }
}

public enum EnemyTypes
{
    AlphaMonster,
}