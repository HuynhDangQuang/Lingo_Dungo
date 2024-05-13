using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

public class DictionaryAPI : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    // Method to fetch data from API and store it locally
    public static async Task FetchAndStoreData(string query)
    {
        string url = "https://api.dictionaryapi.dev/api/v2/entries/en/" + query;

        try
        {
            var response = await client.GetStringAsync(url);
            List<Word> words = JsonConvert.DeserializeObject<List<Word>>(response);

            // Check if any words were found
            if (words != null && words.Count > 0)
            {
                foreach (var word in words)
                {
                    string phonetic = word.phonetic; // Assume phonetic is same for all meanings

                    // Store the phonetic pronunciation locally
                    PlayerPrefs.SetString("WordPhonetic_" + query, phonetic);

                    // Iterate over each meaning
                    foreach (var meaning in word.meanings)
                    {
                        string partOfSpeech = meaning.partOfSpeech;

                        // Iterate over each definition and example
                        foreach (var definition in meaning.definitions)
                        {
                            string def = definition.definition;

                            // Store the definition and part of speech locally
                            string key = "WordDefinition_" + query + "_" + partOfSpeech + "_" + meaning.definitions.IndexOf(definition);
                            PlayerPrefs.SetString(key, def);

                            // Check if examples exist for the definition
                            if (definition.examples != null && definition.examples.Count > 0)
                            {
                                for (int i = 0; i < definition.examples.Count; i++)
                                {
                                    string exampleKey = "WordExample_" + query + "_" + partOfSpeech + "_" + meaning.definitions.IndexOf(definition) + "_" + i;
                                    PlayerPrefs.SetString(exampleKey, definition.examples[i]);
                                }
                            }
                        }
                    }
                }
                PlayerPrefs.Save();
                Debug.Log("Data saved locally for word: " + query);
            }
            else
            {
                Debug.Log("No data found for word: " + query);
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    // Example usage: Call this method from another script to fetch and store data
    async void Start()
    {
        string query = "hello"; // Replace with the word you want to fetch
        await FetchAndStoreData(query);
    }
}

// Define the classes for JSON deserialization
public class Word
{
    public List<Meaning> meanings { get; set; }
    public string phonetic { get; set; }
}

public class Meaning
{
    public string partOfSpeech { get; set; }
    public List<Definition> definitions { get; set; }
}

public class Definition
{
    public string definition { get; set; }
    public List<string> examples { get; set; }
}