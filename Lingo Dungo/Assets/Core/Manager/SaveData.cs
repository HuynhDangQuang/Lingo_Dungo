using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    #region Word Data

    public SortedDictionary<string, Word> wordData;

    #endregion


    #region Achievements

    public SortedDictionary<string, int> seenWordCount;
    public int travelDungeonCount;
    public int dealDamageCount;

    #endregion

    public SaveData()
    {
        wordData = new SortedDictionary<string, Word>();
        seenWordCount = new SortedDictionary<string, int>();
        travelDungeonCount = 0;
        dealDamageCount = 0;
    }
}
