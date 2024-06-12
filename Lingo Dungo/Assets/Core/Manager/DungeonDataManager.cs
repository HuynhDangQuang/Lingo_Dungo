using Assets.Core.Resource.Dungeon;
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

        #region Instance
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
        #endregion

        #region Control

        public bool isStarted = false;

        public void Dispose()
        {
            isStarted = false;
            combatResult = CombatResult.notFighting;
        }

        #endregion

        #region Topic Zone

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
        #endregion

        #region Mapping

        public int playerX = 0;
        public int playerY = 0;

        private int dungeonWidth = 1;
        private int dungeonHeight = 1;

        public DungeonRoom[,] dungeonRooms = null;

        public DungeonRoom GetRoom(int x, int y)
        {
           if (x < 0 || x >= dungeonWidth || y < 0 || y >= dungeonHeight)
           {
                return null;
           }

           return dungeonRooms[x,y];
        }

        public DungeonRoom GetCurrentRoom()
        {
            return dungeonRooms[playerX, playerY];
        }

        public void CreateDemoDungeon()
        {
            dungeonHeight = 7;
            dungeonWidth = 1;
            dungeonRooms = new DungeonRoom[dungeonWidth,dungeonHeight];

            // Create rooms
            dungeonRooms[0, 6] = new DungeonRoom()
            {
                up = true,
                down = false,
                left = false,
                right = false,
                revealed = true
            };

            dungeonRooms[0, 5] = new DungeonRoom()
            {
                up = true,
                down = true,
                left = false,
                right = false
            };
            dungeonRooms[0, 5].enemies[0] = new Enemy(EnemyTypes.MonsterA);

            dungeonRooms[0, 4] = new DungeonRoom()
            {
                up = true,
                down = true,
                left = false,
                right = false,
            };
            dungeonRooms[0, 4].enemies[0] = new Enemy(EnemyTypes.MonsterB);

            dungeonRooms[0, 3] = new DungeonRoom()
            {
                up = true,
                down = true,
                left = false,
                right = false,
            };
            dungeonRooms[0, 3].enemies[0] = new Enemy(EnemyTypes.MonsterC);

            dungeonRooms[0, 2] = new DungeonRoom()
            {
                up = true,
                down = true,
                left = false,
                right = false,
            };
            dungeonRooms[0, 2].enemies[0] = new Enemy(EnemyTypes.MonsterI);

            dungeonRooms[0, 1] = new DungeonRoom()
            {
                up = true,
                down = true,
                left = false,
                right = false,
            };
            dungeonRooms[0, 1].enemies[0] = new Enemy(EnemyTypes.MonsterO);


            dungeonRooms[0, 0] = new DungeonRoom()
            {
                up = false,
                down = true,
                left = false,
                right = false,
                isExit = true
            };

            // start position of player
            playerX = 0;
            playerY = 6;
        }

        #endregion

        #region Combat Control

        public CombatResult combatResult = CombatResult.notFighting;
        public Combatant thisPlayer = null;
        public Combatant[] party = new Combatant[4];

        public void InitializeDemoParty()
        {
            thisPlayer = new Player(PlayerClasses.Knight);
            party[0] = thisPlayer;

        }

        #endregion
    }
}
