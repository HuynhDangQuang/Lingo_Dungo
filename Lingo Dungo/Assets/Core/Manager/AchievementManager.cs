using Assets.Core.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AchievementManager
{
    // Achievement Manager should initialize after WordManager

    static private AchievementManager instance;
    static public AchievementManager Instance
    {
        get
        {
            instance ??= new AchievementManager();
            return instance;
        }
    }

    //public void Initialize()
    //{
    //    InitializeSeenWordCountGoal();
    //    //InitializeTravelDungeonGoal();
    //}

    public void Recaculate()
    {
        CaculateClearTopicCount();

    }

    private AchievementManager() { }



    #region Seen Words

    public SortedDictionary<string, int> seenWordCount = new SortedDictionary<string, int>();

    //public void InitializeSeenWordCountGoal()
    //{
    //    foreach (string word in WordManager.Instance.GetAllWords())
    //    {
    //        seenWordCount.Add(word, 0);
    //    }
    //}

    public int GetSeenWordCount(string word)
    {
        if (seenWordCount.ContainsKey(word))
        {
            return seenWordCount[word];
        }
        else
        {
            seenWordCount.Add(word, 0);
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
        }
        else
        {
            seenWordCount.Add(word, 1);
        }
        SaveManager.Instance.Save();
    }

    #endregion



    #region Travel Dungeon

    public int TravelDungeonCount = 0;

    //private void InitializeTravelDungeonGoal()
    //{
    //    travelDungeonCount = PlayerPrefs.HasKey($"TravelDungeonGoal") ? PlayerPrefs.GetInt($"TravelDungeonGoal") : 0;
    //}

    public void ProgressTravelDungeon()
    {
        TravelDungeonCount++;
        SaveManager.Instance.Save();
        //PlayerPrefs.SetInt($"TravelDungeonGoal", travelDungeonCount);
    }

    #endregion



    #region DealDamage

    public int DealDamageCount = 0;

    //private void InitializeDealDamageGoal()
    //{
    //    dealDamageCount = PlayerPrefs.HasKey($"DealDamageGoal") ? PlayerPrefs.GetInt($"DealDamageGoal") : 0;
    //}

    public void ProgressDealDamage(int damage)
    {
        if (damage < 0)
        {
            return;
        }
        DealDamageCount += damage;
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

    public void CaculateClearTopicCount()
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
