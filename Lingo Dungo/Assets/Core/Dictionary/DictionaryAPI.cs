using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Assets.Core.Manager;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

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
                Word word = GetMostDetailedWord(wrapper.words);

                WordManager.Instance.ImportWord(query, word);

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

    public static Word GetMostDetailedWord(List<Word> words)
    {
        Word bestResult = null;

        foreach(Word word in words)
        {
            if (bestResult == null)
            {
                bestResult = word;
                continue;
            }

            if (CompareDetailedValueOfWord(bestResult, word) == -1)
            {
                bestResult = word;
            }
        }

        return bestResult;
    }

    public static int CompareDetailedValueOfWord(Word a, Word b)
    {
        // HAS PHONETIC WILL BE THE FIRST PRIORITY

        // A more detailed than B
        if (!Utilities.StringNullOrEmpty(a.phonetic) && Utilities.StringNullOrEmpty(b.phonetic))
        {
            return 1;
        }
        // B more detailed than A
        if (Utilities.StringNullOrEmpty(a.phonetic) && !Utilities.StringNullOrEmpty(b.phonetic))
        {
            return -1;
        }

        // COMPARE THE LENGTH OF MEANINGS

        // A more detailed than B
        if (a.meanings.Length > b.meanings.Length)
        {
            return 1;
        }

        // B more detailed than A
        if (a.meanings.Length < b.meanings.Length)
        {
            return -1;
        }

        // COMPARE THE LENGTH OF DEFINITIONS
        
        int definitionsACount = 0;
        int definitionsBCount = 0;
        foreach (Meaning meaning in a.meanings)
        {
            definitionsACount += meaning.definitions.Length;
        }

        foreach (Meaning meaning in b.meanings)
        {
            definitionsBCount += meaning.definitions.Length;
        }

        // A more detailed than B
        if (definitionsACount > definitionsBCount)
        {
            return 1;
        }

        // B more detailed than A
        if (definitionsACount < definitionsBCount)
        {
            return -1;
        }

        return 0;
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