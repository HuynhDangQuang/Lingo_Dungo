using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
using Assets.Core.Manager;
public class MainMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject achieButton;
    public GameObject reviewButton;
    public GameObject quitButton;
    public GameObject settingsWindow;
    public void Start()
    {
        AudioManager.Instance.PlayMusic("MenuTheme");
    }
    void Update()
    {

    }

    public void PlayGame()
    {
        // Temporary use 
        DungeonDataManager.Instance.SaveTopics(WordManager.Instance.GetAllTopics());
        DungeonDataManager.Instance.Dispose();
        DungeonDataManager.Instance.InitializeDemoParty();
        DungeonDataManager.Instance.CreateDemoDungeon();
        //

        SceneManager.LoadScene("DungeonScene");
    }
    public void ReviewWords()
    {
        SceneManager.LoadScene("ReviewScene");
        AudioManager.Instance.PlayMusic("Gallery");
    }
    public void Achievement()
    {
        SceneManager.LoadScene("Achievement");
        AudioManager.Instance.PlayMusic("Gallery");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
