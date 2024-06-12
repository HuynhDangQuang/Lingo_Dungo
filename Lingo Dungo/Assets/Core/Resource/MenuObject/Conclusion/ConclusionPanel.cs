using Assets.Core.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConclusionPanel : MonoBehaviour
{
    [Header("Assets")]
    public Sprite victoryPanel;
    public Sprite defeatPanel;
    public Sprite victoryBanner;
    public Sprite defeatBanner;

    [Header("Components")]
    public Image panelImage;
    public Image bannerImage;
    public Text descriptionText;

    private DungeonDataManager dungeonDataManager = DungeonDataManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        AchievementManager.Instance.ProgressTravelDungeon();
        if (dungeonDataManager.combatResult == CombatResult.lose)
        {
            AudioManager.Instance.PlayMusic("DungeonLost");
            // setup defeat panel
            panelImage.sprite = defeatPanel;
            bannerImage.sprite = defeatBanner;
            descriptionText.text = "Good luck next time!";
        }
        else
        {
            AudioManager.Instance.PlayMusic("DungeonWon");
            panelImage.sprite = victoryPanel;
            bannerImage.sprite = victoryBanner;
            descriptionText.text = "You have escaped from the dungeon!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenuPressed()
    {
        dungeonDataManager.Dispose();
        SceneManager.LoadScene("HomeScreen");
    }
}
