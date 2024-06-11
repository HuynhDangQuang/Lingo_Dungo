using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Assets.Core.Manager;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class DictionaryAPI : MonoBehaviour
{
    public static string bugReport = "";

    private static readonly HttpClient client = new HttpClient();
    // Method to fetch data from API and store it locally
    public static async Task<int> FetchAndStoreData(string query)
    {
        string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + query;

        try
        {
            string response = await client.GetStringAsync(url);

            response = "{\"words\":" + $"{response}" + "}";

            JsonWordWrapper wrapper = new JsonWordWrapper();
            JsonUtility.FromJsonOverwrite(response, wrapper);

            if (wrapper != null && wrapper.words.Count > 0)
            {
                foreach (Word word in wrapper.words)
                {
                    WordManager.Instance.ImportWord(query, word);
                }
                Debug.Log("Data saved locally for word: " + query);
                return 0;
            }
            else
            {
                Debug.Log("No data found for word: " + query);
                return 1;
            }
        }
        catch (HttpRequestException e)
        {
            bugReport = e.Message;
            Debug.LogError("Error fetching data: " + e.Message);
            return 2;
        }
        catch (JsonSerializationException e)
        {
            bugReport = e.Message;
            Debug.LogError("Error deserializing data: " + e.Message);
            return 3;
        }
        catch (Exception e)
        {
            bugReport = e.Message;
            Debug.LogError(e.Message);
            return 4;
        }   
    }

    public const int TASKRESULT_SUCCESS = 0;
    public const int TASKRESULT_DATANOTFOUND = 1;
    public const int TASKRESULT_HTTPFAILED = 2;
    public const int TASKRESULT_JSONDESERIALIZINGFAIL = 3;
    public const int TASKRESULT_UNKNOWN_ERROR = 4;
}


// Define the classes for JSON deserialization
[Serializable]
public class JsonWordWrapper
{
    public List<Word> words;
}

[Serializable]
public class Word
{
    public string word;
    public string phonetic;
    public Phonetic[] phonetics;
    public Meaning[] meanings;
    public License license;
    public string[] sourceUrls;
}

[Serializable]
public class Phonetic
{
    public string text;
    public string audio;
}

[Serializable]
public class Meaning
{
    public string partOfSpeech;
    public Definition[] definitions;
    public string[] synonyms;
    public string[] antonyms;
}

[Serializable]
public class Definition
{
    public string definition;
    public string[] synonyms;
    public string[] antonyms;
    public string example;
}

[Serializable]
public class License
{
    public string name;
    public string url;
}