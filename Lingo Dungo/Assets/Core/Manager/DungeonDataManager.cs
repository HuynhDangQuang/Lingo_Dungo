using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Core.Manager
{
    public class DungeonDataManager
    {
        #region Constant
        public const int DEFAULT_SEGMENT_AMOUNT = 3;
        #endregion

        static private DungeonDataManager instance;
        static public DungeonDataManager Instance
        {
            get
            {
                instance ??= new DungeonDataManager();
                return instance;
            }
        }

        private DungeonDataManager() { }

        private List<string> wordsList = new List<string>();
        private List<string> saveTopics = new List<string>();

        public void SaveTopics(List<string> topics)
        {
            saveTopics = topics;
        }

        public void LoadTopics(int segmentAmount)
        {
            wordsList.Clear();
            if (saveTopics.Count == 0)
            {
                return;
            }

            WordManager wordManager = WordManager.Instance;
            AchievementManager achievementManager = AchievementManager.Instance;

            List<string> words = new List<string>();
            
            // Get all words first
            foreach (string topic in saveTopics)
            {
                words.AddRange(wordManager.GetWordsInTopic(topic));
            }

            // Sort the array base on the seen frequency of the words 

            words = words.OrderBy(x => achievementManager.GetSeenWordCount(x)).ToList();
            
            // Separate the array to small segment, then shuffle the word in segment and add them to list again

            int segmentLength = words.Count / segmentAmount;
            if (segmentLength == 0)
            {
                segmentLength = words.Count;
                segmentAmount = 1;
            }

            int currentIndex = 0;
            for (int i = 0; i < segmentAmount; i++)
            {
                List<string> segment = words.GetRange(
                    currentIndex,
                    currentIndex + segmentLength > words.Count ? words.Count - currentIndex - segmentLength : segmentLength
                );
                currentIndex += segmentLength; 
                Utilities.ShuffleList(segment);
                wordsList.AddRange(segment);
            }
        }

        public string GetWord()
        {
            if (wordsList.Count == 0)
            {
                LoadTopics(DEFAULT_SEGMENT_AMOUNT);
            }
            string word = wordsList.First();
            wordsList.RemoveAt(0);
            AchievementManager.Instance.AddSeenWordCount(word);
            return word;
        }
    }
}
