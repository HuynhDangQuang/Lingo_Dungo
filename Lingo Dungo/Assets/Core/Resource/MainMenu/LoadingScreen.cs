using Assets.Core.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class LoadingScreen : MonoBehaviour
{
    public Image fill;
    public Text description;

    private float fillAmount;
    //public GameObject loadingBar;

    public string downloadingWord = "";
    public int downloadStatus = -2;

    // Start is called before the first frame update
    void Start()
    {
        fillAmount = 0f;
        StartCoroutine(LoadUnits());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();

        if (downloadingWord != "" && downloadStatus == -2)
        {
            _ = LoadWordAsync(downloadingWord);
        }
    }

    public IEnumerator LoadUnits()
    {
        WordManager wordManager = WordManager.Instance;

        int progress = 0;
        int totalWordCount = 0;
        List<string> downloadList = new List<string>();



        // IMPORT DATA

        description.text = "Importing local data...";
        bool error = false;


        yield return new WaitForSeconds(0.5f);
        //description.text = SaveManager.Instance.SAVE_FILE;

        //yield return new WaitForSeconds(0.5f);
        try
        {
            WordManager.Instance.LoadTopics();
        }
        catch (Exception e)
        {
            description.text = "Load topics failed: " + e.Message;
            error = true;
        }

        if (error)
        {
            yield return new WaitForSeconds(3f);
            error = false;
        }

        yield return new WaitForSeconds(0.5f);
        
        try
        {
            SaveManager.Instance.Initialize();
        }
        catch (Exception e)
        {
            description.text = $"SaveManager Initialize failed: {e.Message}";
            error = true;
        }

        if (error)
        {
            yield return new WaitForSeconds(3f);
            error = false;
        }

        yield return new WaitForSeconds(0.5f);

        try
        {
            SaveManager.Instance.Load();
        }
        catch (Exception e)
        {
            description.text = $"SaveManager Load failed: {e.Message}";
            error = true;
        }

        if (error)
        {
            yield return new WaitForSeconds(3f);
            error = false;
        }

        totalWordCount = wordManager.GetTotalWordsInTopics();
        yield return new WaitForSeconds(0.5f);

        // VERIFYING PHASE
        description.text = "Verifying resource...";

        // Validate data

        foreach (List<string> words in wordManager.topicData.Values)
        {
            foreach(string word in words)
            {
                if (!wordManager.CheckWordIsLoaded(word) && !downloadList.Contains(word))
                {
                    downloadList.Add(word);
                }
                yield return new WaitForSeconds(0.01f);
                progress++;
                fillAmount = progress * 1f / totalWordCount;
            }
        }

        yield return new WaitForSeconds(0.5f);
        description.text = "Verifying resource done!";
        yield return new WaitForSeconds(0.5f);

        // DOWNLOADING PHASE
        if (downloadList.Count > 0)
        {
            // TRY IMPORT FROM PRESET RESOURCE FIRST
            SaveData presetSave = SaveManager.Instance.ImportPresetSaveData();
            bool needToDownload = false;

            List<string> removeWord = new List<string>();
            foreach (string word in downloadList)
            {
                if (presetSave.wordData.ContainsKey(word) && presetSave.wordData[word] != null)
                {
                    wordManager.wordData[word] = presetSave.wordData[word];
                    removeWord.Add(word);
                }
            }
            downloadList.RemoveAll(word => removeWord.Contains(word));
            needToDownload = downloadList.Count > 0;

            if (needToDownload)
            {
                progress = 0;
                totalWordCount = downloadList.Count;
                description.text = "Downloading resource... (" + progress + "/"
                        + totalWordCount + ") Please make sure your device is connected with Internet.";
                fillAmount = progress * 1f / totalWordCount;

                foreach (string word in downloadList)
                {
                    downloadingWord = word;

                    int timeout = 5;
                    while (downloadStatus != DictionaryAPI.TASKRESULT_SUCCESS)
                    {
                        bool canExit = true;
                        yield return new WaitForSeconds(0.05f);
                        switch (downloadStatus)
                        {
                            case -2:
                                canExit = false;
                                continue;
                            case -1:
                                canExit = false;
                                continue;
                            case DictionaryAPI.TASKRESULT_DATANOTFOUND:
                                description.text = "Data not found";
                                canExit = true;
                                break;
                            case DictionaryAPI.TASKRESULT_HTTPFAILED:
                                timeout--;
                                description.text = $"Data HTTP Failed ({timeout})";
                                break;
                            case DictionaryAPI.TASKRESULT_JSONDESERIALIZINGFAIL:
                                description.text = $"Json serializing fail";
                                canExit = true;
                                break;
                            case DictionaryAPI.TASKRESULT_UNKNOWN_ERROR:
                                description.text = DictionaryAPI.bugReport;
                                canExit = true;
                                break;
                        }
                        if (timeout == 0)
                        {
                            // Push warning and exit game
                            StopCoroutine(LoadUnits());
                            break;
                        }
                        if (canExit)
                        {
                            break;
                        }
                    }
                    SaveManager.Instance.Save();
                    yield return new WaitForSeconds(0.5f);
                    progress++;
                    downloadStatus = -2;
                    downloadingWord = "";

                    description.text = "Downloading resource... (" + progress + "/"
                        + totalWordCount + ") Please make sure your device is connected with Internet.";
                    fillAmount = progress * 1f / totalWordCount;
                }

                yield return new WaitForSeconds(0.5f);
                description.text = "Download resource done!";
                yield return new WaitForSeconds(0.5f);
            }
        }
        description.text = "Ready to start game...";
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("HomeScreen");
        StopCoroutine(LoadUnits());
    }

    public async Task LoadWordAsync(string word)
    {
        downloadStatus = -1;
        Debug.Log("Downloading <" + word + ">");
        downloadStatus = await DictionaryAPI.FetchAndStoreData(word.Trim());
        Debug.Log("Downloaded <" + word + "> successfully");
    }

    public void UpdateBar()
    {
        fill.fillAmount = fillAmount;
    }

    //enum LoadingState
    //{
    //    verifying,
    //    downloading,
    //}
}
