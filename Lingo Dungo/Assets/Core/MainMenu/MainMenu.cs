using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
public class MainMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject achieButton;
    public GameObject reviewButton;
    public GameObject quitButton;

    public void Start()
    {
        AudioManager.Instance.PlayMusic("MenuTheme");
    }
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
