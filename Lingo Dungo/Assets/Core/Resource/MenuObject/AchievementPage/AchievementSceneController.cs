using Assets.Core.Manager;
using Assets.Core.Resource.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementSceneController : MonoBehaviour
{
    public Transform achievementContainer;
    public GameObject achievementPrefab;

    public Text collectedCount;

    public List<Achievement> achievements = new List<Achievement>();

    // Start is called before the first frame update
    void Start()
    {
        AchievementManager manager = AchievementManager.Instance;
        manager.Recaculate();

        int collected = 0;
        foreach (Achievement achievement in achievements)
        {
            achievement.ReloadProgress();
            GameObject newObject = Instantiate(achievementPrefab, achievementContainer);
            newObject.GetComponent<AchievementUnit>().Setup(achievement);
            if (achievement.GetProgressRate() == 1f)
            {
                collected++;
            }
        }

        collectedCount.text = $"{collected} / {achievements.Count}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
