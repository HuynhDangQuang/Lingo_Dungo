using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Resource.Skills.Player
{
    #region Normal Attack
    public class Knight_NormalAttack : Skill
    {

        public Knight_NormalAttack() {
            type = DamageType.Damage;
            cost = 0;
            TargetsAnimation = "Hit";
            TargetsHitSFX = "Slash1";
        }

        public override int GetDamage(Combatant user, Combatant target, float resultMultiplier)
        {
            float value = user.ATK * resultMultiplier;
            return Mathf.RoundToInt(value);
        }
    }
    #endregion

    #region Primary Skill
    public class Knight_Block : Skill
    {

        public Knight_Block()
        {
            type = DamageType.None;
            cost = 30;
            TargetsAnimation = "Buff";
            TargetsHitSFX = "Up1";
        }

        public override int GetDamage(Combatant user, Combatant target, float resultMultiplier)
        {
            return 0;
        }

        public override void ApplyTargetEffect(Combatant user, Combatant target, float resultMultiplier)
        {
            float value = user.AP * 5f * Mathf.Clamp(resultMultiplier, 0.3f, 1f);
            target.GainBarrier(Mathf.FloorToInt(value));
        }
    }
    #endregion

    #region Secondary Skill
    public class Knight_Cleave : Skill
    {

        public Knight_Cleave()
        {
            type = DamageType.Damage;
            cost = 40;
            TargetsAnimation = "Hit";
            TargetsHitSFX = "Slash1";
        }

        public override int GetDamage(Combatant user, Combatant target, float resultMultiplier)
        {
            float value = (user.ATK * 1.5f + user.AP) * resultMultiplier;
            return Mathf.RoundToInt(value);
        }
    }
    #endregion
}
