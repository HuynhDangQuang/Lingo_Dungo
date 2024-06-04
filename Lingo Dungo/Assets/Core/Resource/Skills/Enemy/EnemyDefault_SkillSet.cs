using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Resource.Skills.Enemy
{
    #region Normal Attack
    public class EnemyDefault_NormalAttack : Skill
    {
        public EnemyDefault_NormalAttack()
        {
            type = DamageType.Damage;
            cost = 0;
            TargetsAnimation = "Hit";
            TargetsHitSFX = "Blow1";
        }

        public override int GetDamage(Combatant user, Combatant target, float resultMultiplier)
        {
            float value = user.ATK * resultMultiplier;
            return Mathf.RoundToInt(value);
        }
    }
    #endregion
}
