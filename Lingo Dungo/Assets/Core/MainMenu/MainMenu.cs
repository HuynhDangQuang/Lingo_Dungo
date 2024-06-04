using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReviewWords()
    {
        SceneManager.LoadScene("ReviewScene");
    }
    public void Achievement()
    {
        SceneManager.LoadScene("Achievement");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
