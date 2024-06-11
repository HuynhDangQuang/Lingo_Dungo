using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Resource.Dungeon
{
    public class DungeonRoom
    {
        public Enemy[] enemies = new Enemy[4];

        public bool up, down, left, right;
        public bool revealed = false;
        public bool isExit = false;

        public void ReleaseEnemies()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = null;
            }
        }

        public bool CanStartCombat()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
