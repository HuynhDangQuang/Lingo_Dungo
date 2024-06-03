using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Resource.EnemyLogic
{
    public class EnemyLogic_Default : EnemyLogicBase
    {
        public EnemyLogic_Default(Combatant owner)
        {
            this.owner = owner;
        }

        public override Action.SkillType? ChooseAction()
        {
            return Action.SkillType.NormalAttack;
        }
    }
}
