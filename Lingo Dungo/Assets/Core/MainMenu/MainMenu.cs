using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.IO;
public class MainMenu : MonoBehaviour
{
    public Image fill;
    private List<string> words;
    private float fillAmount;
    public GameObject startButton;
    public GameObject achieButton;
    public GameObject reviewButton;
    public GameObject quitButton;
    public GameObject loadingBar;
    public void Start()
    {
        StartCoroutine(LoadUnits());
    }
    void Update()
    {
        UpdateBar();
    }

    public IEnumerator LoadUnits()
    {
        string dataPath = Path.Combine(Application.dataPath, "Core/Data/Word");
        int totalWordCount = 0;
        int loadedWordCount = 0;

        string[] files = Directory.GetFiles(dataPath, "*.txt");

        foreach (string file in files)
        {
            if (File.Exists(file))
            {
                totalWordCount += File.ReadAllLines(file).Length;
            }
        }

        foreach (string file in files)
        {
            loadedWordCount += LoadWordsCount(file);
            fillAmount = (float)loadedWordCount / totalWordCount;

            yield return new WaitForSeconds(0.5f); 
        }

        startButton.SetActive(true);
        achieButton.SetActive(true);
        reviewButton.SetActive(true);
        quitButton.SetActive(true);
        loadingBar.SetActive(false);
    }

    public int LoadWordsCount(string filePath)
    {
        words = new List<string>();
        int count = 0;

        if (File.Exists(filePath))
        {
            string[] wordLines = File.ReadAllLines(filePath);

            foreach (string line in wordLines)
            {
                if (line.Trim() != "")
                {
                    words.Add(line.Trim());
                    DictionaryAPI.FetchAndStoreData(line.Trim());
                    count++;
                }
            }

            words.Sort();

        }
        else
        {
            Debug.LogError("File " + filePath + " not founded!");
        }

        return count;
    }

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
    public void UpdateBar()
    {
        fill.fillAmount = fillAmount;
    }
}
