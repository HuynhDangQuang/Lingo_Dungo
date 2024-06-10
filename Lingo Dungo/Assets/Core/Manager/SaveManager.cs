using Assets.Core.Manager;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    #region Instance zone
    static private SaveManager instance;
    static public SaveManager Instance
    {
        get
        {
            instance ??= new SaveManager();
            return instance;
        }
    }
    private SaveManager() { }
    #endregion

    private SaveData data = new SaveData();
    public readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    public readonly string SAVE_FILE = Application.persistentDataPath + "/Saves/savefile.txt";

    public void Initialize()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // create save folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        if (!File.Exists(SAVE_FILE))
        {
            FileStream newFile = File.Create(SAVE_FILE);
            newFile.Close();
        }
    }

    public void Save()
    {
        WordManager wordManager = WordManager.Instance;
        AchievementManager achievementManager = AchievementManager.Instance;
        
        data.wordData = wordManager.wordData;
        data.dealDamageCount = achievementManager.DealDamageCount;
        data.travelDungeonCount = achievementManager.TravelDungeonCount;
        data.seenWordCount = achievementManager.seenWordCount;

        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(SAVE_FILE, json);
    }

    public void Load()
    {
        WordManager wordManager = WordManager.Instance;
        AchievementManager achievementManager = AchievementManager.Instance;

        string json = File.ReadAllText(SAVE_FILE);
        data = JsonConvert.DeserializeObject<SaveData>(json);

        data ??= new SaveData();

        wordManager.wordData = data.wordData;
        achievementManager.DealDamageCount = data.dealDamageCount;
        achievementManager.TravelDungeonCount = data.travelDungeonCount;
        achievementManager.seenWordCount = data.seenWordCount;

    }
}
