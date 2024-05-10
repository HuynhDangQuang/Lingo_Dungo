using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float TimeRate = 0f;
    public string Answer;
    public Combatant User;
    public List<Combatant> Targets;
    public Skill Skill;
    public SkillType type = SkillType.NormalAttack;

    public Action(float time, string answer, Combatant user, List<Combatant> targets, Skill skill, SkillType type)
    {
        this.TimeRate = time;
        this.Answer = answer;
        this.User = user;
        this.Targets = targets;
        this.Skill = skill;
        this.type = type;
    }

    public enum SkillType
    {
        NormalAttack,
        PrimarySkill,
        SecondarySkill
    }
}
