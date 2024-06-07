using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Resource.Model
{
    [Serializable]
    public class Achievement
    {
        public const string iconPath = "";

        public string name;
        public string description;
        public Sprite icon;
        public GoalFlag flag;
        public int goalValue;

        [NonSerialized]
        public int progressValue;

        public void ReloadProgress()
        {
            AchievementManager manager = AchievementManager.Instance;
            switch (flag)
            {
                case GoalFlag.FindWord:
                    progressValue = manager.GetTotalSeenWords();
                    break;
                case GoalFlag.ClearTopic:
                    progressValue = manager.ClearTopicCount;
                    break;
                case GoalFlag.DealDamage:
                    progressValue = manager.DealDamageCount;
                    break;
                case GoalFlag.TravelDungeon:
                    progressValue = manager.TravelDungeonCount;
                    break;
            }
        }

        public float GetProgressRate()
        {
            return Math.Min(progressValue / goalValue, 1f);
        }

        public int GetProgressPercent()
        {
            return Math.Min(Mathf.FloorToInt(progressValue * 100f / goalValue), 100);
        }
    }
}
