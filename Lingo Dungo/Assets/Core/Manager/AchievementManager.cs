using Assets.Core.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AchievementManager
{
    static private AchievementManager instance;
    static public AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AchievementManager();
            }
            return instance;
        }
    }

    public void Initialize()
    {
        InitializeSeenWordCountGoal();
        InitializeTravelDungeonGoal();
    }

    public void Recaculate()
    {
        caculateClearTopicCount();

    }

    private AchievementManager() { }



    #region Seen Words

    private SortedDictionary<string, int> seenWordCount = new SortedDictionary<string, int>();

    public void InitializeSeenWordCountGoal()
    {
        foreach(string word in WordManager.Instance.GetAllWords())
        {
            int count = PlayerPrefs.HasKey($"SeenWord_{word}") ? PlayerPrefs.GetInt($"SeenWord_{word}") : 0;
            seenWordCount.Add(word, count);
        }
    }

    public int GetSeenWordCount(string word)
    {
        if (seenWordCount.ContainsKey(word))
        {
            return seenWordCount[word];
        }
        return 0;
    }

    public int GetTotalSeenWords()
    {
        int count = 0;
        foreach (KeyValuePair<string, int> pair in seenWordCount)
        {
            if (pair.Value > 0)
            {
                count++;
            }
        }
        return count;
    }

    public void AddSeenWordCount(string word)
    {
        if (seenWordCount.ContainsKey(word))
        {
            seenWordCount[word] += 1;
            PlayerPrefs.SetInt($"SeenWord_{word}", seenWordCount[word]);
        }
    }

    #endregion



    #region Travel Dungeon

    private int travelDungeonCount = 0;
    public int TravelDungeonCount
    {
        get { return travelDungeonCount; }
    }

    private void InitializeTravelDungeonGoal()
    {
        travelDungeonCount = PlayerPrefs.HasKey($"TravelDungeonGoal") ? PlayerPrefs.GetInt($"TravelDungeonGoal") : 0;
    }

    public void progressTravelDungeon()
    {
        travelDungeonCount++;
        PlayerPrefs.SetInt($"TravelDungeonGoal", travelDungeonCount);
    }

    #endregion



    #region DealDamage

    private int dealDamageCount = 0;
    public int DealDamageCount
    {
        get { return dealDamageCount; }
    }

    private void InitializeDealDamageGoal()
    {
        dealDamageCount = PlayerPrefs.HasKey($"DealDamageGoal") ? PlayerPrefs.GetInt($"DealDamageGoal") : 0;
    }

    public void progressDealDamage(int damage)
    {
        if (damage < 0)
        {
            return;
        }
        dealDamageCount += damage;
        PlayerPrefs.SetInt($"DealDamageGoal", dealDamageCount);
    }

    #endregion



    #region Clear Topic

    int clearTopicCount = 0;
    public int ClearTopicCount
    {
        get { return clearTopicCount; }
    }

    public bool IsTopicClear(string topicName)
    {
        foreach (string word in WordManager.Instance.GetWordsInTopic(topicName))
        {
            if (GetSeenWordCount(word) == 0)
                return false;
        }
        return true;
    }

    public void caculateClearTopicCount()
    {
        int result = 0;
        foreach (string topic in WordManager.Instance.GetAllTopics())
        {
            if (IsTopicClear(topic))
            {
                result++;
            }
        }
        clearTopicCount = result;
    }

    #endregion
}

public enum GoalFlag
{
    FindWord,
    TravelDungeon,
    DealDamage,
    ClearTopic,
}
