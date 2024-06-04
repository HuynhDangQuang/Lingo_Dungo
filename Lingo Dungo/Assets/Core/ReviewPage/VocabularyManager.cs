using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VocabularyManager : MonoBehaviour
{
    public string vocabularyFolderPath = "Assets/Data/Word";
    public string localDataPath = "Assets/Data/WordData/"; // Path to save word data

    private Dictionary<string, Dictionary<string, Word>> wordData =
        new Dictionary<string, Dictionary<string, Word>>();

    void Start()
    {
        LoadVocabulary();
        string testWord = "banana"; // Replace with a word you know is in your vocabulary files

        if (IsWordDataStored(testWord))
        {
            string storedPhonetic = PlayerPrefs.GetString("WordPhonetic_" + testWord);
            Debug.Log("Stored phonetic: " + storedPhonetic);

            // Iterate over each part of speech
            string[] partsOfSpeech = { "noun", "verb", "interjection" }; // Add other parts of speech as needed
            foreach (string partOfSpeech in partsOfSpeech)
            {
                Debug.Log("Part of speech: " + partOfSpeech);

                // Iterate over each definition for the part of speech
                int index = 0;
                string definitionKey = "WordDefinition_" + testWord + "_" + partOfSpeech + "_" + index;
                while (PlayerPrefs.HasKey(definitionKey))
                {
                    string storedDefinition = PlayerPrefs.GetString(definitionKey);
                    Debug.Log("Stored definition " + index + ": " + storedDefinition);

                    // Iterate over each example for the definition
                    int exampleIndex = 0;
                    string exampleKey = "WordExample_" + testWord + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                    while (PlayerPrefs.HasKey(exampleKey))
                    {
                        string storedExample = PlayerPrefs.GetString(exampleKey);
                        Debug.Log("Stored example " + exampleIndex + " for definition " + index + ": " + storedExample);
                        exampleIndex++;
                        exampleKey = "WordExample_" + testWord + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                    }

                    index++;
                    definitionKey = "WordDefinition_" + testWord + "_" + partOfSpeech + "_" + index;
                }
            }
        }
        else
        {
            Debug.Log("Data for '" + testWord + "' NOT found in PlayerPrefs.");
        }
    }

    async void LoadVocabulary()
    {
        // 1. Get all vocabulary text files
        string[] topicFiles = Directory.GetFiles(vocabularyFolderPath, "*.txt");

        foreach (string topicFile in topicFiles)
        {
            string topicName = Path.GetFileNameWithoutExtension(topicFile);
            wordData[topicName] = new Dictionary<string, Word>();

            // 2. Extract words from the current text file
            string[] words = File.ReadAllLines(topicFile);

            foreach (string word in words)
            {
                // 3. Check if the word is stored locally
                if (!IsWordDataStored(word))
                {
                    // 4. If not, fetch data from the API
                    await DictionaryAPI.FetchAndStoreData(word);
                }

                // 5. Load word data from local storage into Word object
                Word wordObject = LoadWordFromPlayerPrefs(word);

                // 6. Add the word object to the dictionary
                wordData[topicName].Add(word, wordObject);
            }
        }
    }

    // Check if word data exists in PlayerPrefs
    bool IsWordDataStored(string word)
    {
        return PlayerPrefs.HasKey("WordPhonetic_" + word);
    }

    // Load word data from PlayerPrefs into a Word object
    Word LoadWordFromPlayerPrefs(string word)
    {
        Word wordObject = new Word();
        wordObject.phonetic = PlayerPrefs.GetString("WordPhonetic_" + word);
        wordObject.meanings = new List<Meaning>();

        // Iterate through the stored meanings and definitions
        int meaningIndex = 0;
        while (PlayerPrefs.HasKey($"WordMeaning_{word}_{meaningIndex}"))
        {
            Meaning meaning = new Meaning();
            meaning.partOfSpeech = PlayerPrefs.GetString($"WordMeaning_{word}_{meaningIndex}_PartOfSpeech");
            meaning.definitions = new List<Definition>();

            // Iterate through the stored definitions for this meaning
            int definitionIndex = 0;
            while (PlayerPrefs.HasKey($"WordDefinition_{word}_{meaningIndex}_{definitionIndex}"))
            {
                Definition definition = new Definition();
                definition.definition = PlayerPrefs.GetString($"WordDefinition_{word}_{meaningIndex}_{definitionIndex}");

                // Check if examples exist for this definition
                if (PlayerPrefs.HasKey($"WordExample_{word}_{meaningIndex}_{definitionIndex}"))
                {
                    string examplesString = PlayerPrefs.GetString($"WordExample_{word}_{meaningIndex}_{definitionIndex}");
                    definition.examples = new List<string>(examplesString.Split(';'));
                }

                meaning.definitions.Add(definition);
                definitionIndex++;
            }

            wordObject.meanings.Add(meaning);
            meaningIndex++;
        }

        return wordObject;
    }
}