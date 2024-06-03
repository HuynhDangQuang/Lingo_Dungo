using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Resource.EnemyLogic
{
    public abstract class EnemyLogicBase
    {
        public Combatant owner;
        public EnemyLogicBase() { }
        abstract public Action.SkillType? ChooseAction();
       
    }
}
