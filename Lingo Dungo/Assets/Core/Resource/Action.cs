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
    public string userAnimationId;
    public string targetAnimationId;

    public Action(float time, string answer, Combatant user, List<Combatant> targets, Skill skill, SkillType type, string userAnimationId, string targetAnimationId)
    {
        this.TimeRate = time;
        this.Answer = answer;
        this.User = user;
        this.Targets = targets;
        this.Skill = skill;
        this.type = type;
        this.userAnimationId = userAnimationId;
        this.targetAnimationId = targetAnimationId;
    }

    public enum SkillType
    {
        NormalAttack,
        PrimarySkill,
        SecondarySkill
    }
}
