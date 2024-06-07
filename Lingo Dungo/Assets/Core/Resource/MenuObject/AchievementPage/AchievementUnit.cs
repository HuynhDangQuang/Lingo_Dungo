using Assets.Core.Resource.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUnit : MonoBehaviour
{
    public Image achievementImage;
    public Text title;
    public Text description;
    public Image progressGauge;
    public Text progressValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Achievement achievement)
    {
        achievementImage.sprite = achievement.icon;
        title.text = $"{achievement.name}";
        description.text = $"{achievement.description}\n({achievement.progressValue}/{achievement.goalValue})";
        progressValue.text = $"{achievement.GetProgressPercent()}%";
        progressGauge.fillAmount = achievement.GetProgressRate();
    }
}
