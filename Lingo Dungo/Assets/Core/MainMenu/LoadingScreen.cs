using Assets.Core.Manager;
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
        //PlayerPrefs.DeleteAll();
        WordManager wordManager = WordManager.Instance;

        string dataPath = Path.Combine(Application.dataPath, "Core/Data/Word");
        int progress = 0;
        int totalWordCount = 0;
        List<string> downloadList = new List<string>();



        // IMPORT DATA

        description.text = "Importing local data...";
        yield return new WaitForSeconds(0.5f);

        wordManager.LoadWordFromPrefs();
        totalWordCount = wordManager.GetWordCount();
        yield return new WaitForSeconds(0.5f);

        // VERIFYING PHASE
        description.text = "Verifying resource...";

        // to create a fake loading bar
        foreach (string word in wordManager.GetAllWords())
        {
            if (!wordManager.CheckWordIsLoaded(word))
            {
                downloadList.Add(word);
            }
            yield return new WaitForSeconds(0.01f);
            progress++;
            fillAmount = progress * 1f / totalWordCount;
        }

        yield return new WaitForSeconds(0.5f);
        description.text = "Verifying resource done!";
        yield return new WaitForSeconds(0.5f);

        // DOWNLOADING PHASE

        if (downloadList.Count > 0)
        {
            progress = 0;
            totalWordCount = downloadList.Count;

            foreach (string word in downloadList)
            {
                description.text = "Downloading resource... (" + progress + "/"
                                    + totalWordCount + ") Please make sure your device is connected with Internet.";
                fillAmount = progress * 1f / totalWordCount;

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
                            canExit = true;
                            break;
                        case DictionaryAPI.TASKRESULT_HTTPFAILED:
                            timeout--;
                            break;
                        case DictionaryAPI.TASKRESULT_JSONDESERIALIZINGFAIL:
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
                wordManager.TryImportMissingWord(word);
                yield return new WaitForSeconds(0.02f);
                progress++;
                downloadStatus = -2;
                downloadingWord = "";
            }

            yield return new WaitForSeconds(0.5f);
            description.text = "Download resource done!";
            yield return new WaitForSeconds(0.5f);
        }
        SceneManager.LoadScene("HomeScreen");
        StopCoroutine(LoadUnits());
    }

    //public int LoadWordsCount(string filePath)
    //{
    //    words = new List<string>();
    //    int count = 0;

    //    if (File.Exists(filePath))
    //    {
    //        string[] wordLines = File.ReadAllLines(filePath);

    //        foreach (string line in wordLines)
    //        {
    //            if (line.Trim() != "")
    //            {
    //                words.Add(line.Trim());
    //                DictionaryAPI.FetchAndStoreData(line.Trim());
                    
                    
    //                count++;
    //            }
    //        }

    //        words.Sort();

    //    }
    //    else
    //    {
    //        Debug.LogError("File " + filePath + " not founded!");
    //    }

    //    return count;
    //}

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
