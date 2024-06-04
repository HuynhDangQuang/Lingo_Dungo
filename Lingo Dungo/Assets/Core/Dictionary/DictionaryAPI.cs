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
                PlayerPrefs.SetString("WordDefinition_" + query, "");
                foreach (var word in words)
                {
                    // Store the phonetic pronunciation locally
                    PlayerPrefs.SetString("WordPhonetic_" + query, word.phonetic);

                    // Iterate over each meaning
                    int meaningIndex = 0;
                    foreach (var meaning in word.meanings)
                    {
                        string partOfSpeech = meaning.partOfSpeech;

                        // Store the part of speech
                        string partOfSpeechKey = "WordPartOfSpeech_" + query + "_" + partOfSpeech;
                        PlayerPrefs.SetString(partOfSpeechKey, partOfSpeech);

                        // Iterate over each definition
                        int definitionIndex = 0;
                        foreach (var definition in meaning.definitions)
                        {
                            // Store the definition
                            string definitionKey = "WordDefinition_" + query + "_" + partOfSpeech + "_" + definitionIndex;
                            PlayerPrefs.SetString(definitionKey, definition.definition);

                            // Check if examples exist for the definition
                            if (definition.examples != null && definition.examples.Count > 0)
                            {
                                for (int i = 0; i < definition.examples.Count; i++)
                                {
                                    string exampleKey = "WordExample_" + query + "_" + meaningIndex + "_" + definitionIndex + "_" + i;
                                    PlayerPrefs.SetString(exampleKey, definition.examples[i]);
                                }
                            }

                            definitionIndex++;
                        }

                        meaningIndex++;
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
            Debug.LogError("Error fetching data: " + e.Message);
        }
        catch (JsonSerializationException e)
        {
            Debug.LogError("Error deserializing data: " + e.Message);
        }
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