using UnityEngine;

public class TestPlayerPrefs : MonoBehaviour
{
    // Example usage: Call this method to test if PlayerPrefs stored the correct data
    void Start()
    {
        string query = "hello"; // Replace with the word you tested with

        // Retrieve and log the stored phonetic pronunciation
        string storedPhonetic = PlayerPrefs.GetString("WordPhonetic_" + query);
        Debug.Log("Stored phonetic: " + storedPhonetic);

        // Iterate over each part of speech
        string[] partsOfSpeech = { "noun", "verb", "interjection" }; // Add other parts of speech as needed
        foreach (string partOfSpeech in partsOfSpeech)
        {
            Debug.Log("Part of speech: " + partOfSpeech);

            // Iterate over each definition for the part of speech
            int index = 0;
            string definitionKey = "WordDefinition_" + query + "_" + partOfSpeech + "_" + index;
            while (PlayerPrefs.HasKey(definitionKey))
            {
                string storedDefinition = PlayerPrefs.GetString(definitionKey);
                Debug.Log("Stored definition " + index + ": " + storedDefinition);

                // Iterate over each example for the definition
                int exampleIndex = 0;
                string exampleKey = "WordExample_" + query + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                while (PlayerPrefs.HasKey(exampleKey))
                {
                    string storedExample = PlayerPrefs.GetString(exampleKey);
                    Debug.Log("Stored example " + exampleIndex + " for definition " + index + ": " + storedExample);
                    exampleIndex++;
                    exampleKey = "WordExample_" + query + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                }

                index++;
                definitionKey = "WordDefinition_" + query + "_" + partOfSpeech + "_" + index;
            }
        }
    }
}
