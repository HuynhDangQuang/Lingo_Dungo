using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class WordLoader : MonoBehaviour
{
    public GameObject wordButtonPrefab;
    public Transform wordButtonContainer;
    public Text topicText;
    public Text wordDefinition;
    public static WordLoader instance;
    private List<string> words;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        TopicManager.instance.LoadTopics();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void LoadWords(string filePath)
    {
        words = new List<string>();

        if (File.Exists(filePath))
        {
            string[] wordLines = File.ReadAllLines(filePath);

            foreach (string line in wordLines)
            {
                if (line.Trim() != "")
                {
                    words.Add(line.Trim());
                }
            }

            words.Sort();

            foreach (Transform child in wordButtonContainer)
            {
                Destroy(child.gameObject);
            }

            CreateWordButtons();
        }
        else
        {
            Debug.LogError("File " + filePath + " not founded!");
        }
    }

    private void CreateWordButtons()
    {
        for (int i = 0; i < words.Count; i++)
        {
            GameObject newButton = Instantiate(wordButtonPrefab, wordButtonContainer);

            Text textComponent = newButton.GetComponentInChildren<Text>();

            textComponent.text = words[i];

            EventTrigger trigger = newButton.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnWordClick(textComponent.text); });
            trigger.triggers.Add(entry);
        }
    }

    public async void OnWordClick(string word)
    {
        Debug.Log("Word clicked: " + word);

        if (!PlayerPrefs.HasKey("WordDefinition_" + word))
        {
            Debug.Log("WordDefinition not found in PlayerPrefs: " + word);

            await DictionaryAPI.FetchAndStoreData(word);
        }

        if (PlayerPrefs.HasKey("WordDefinition_" + word))
        {
            Debug.Log("WordDefinition found in PlayerPrefs: " + word);

            string phonetic = PlayerPrefs.GetString("WordPhonetic_" + word, "");
            string definition = "";

            string[] partsOfSpeech = { "noun", "verb", "adjective", "adverb" };
            foreach (string partOfSpeech in partsOfSpeech)
            {
                Debug.Log("Part of speech: " + partOfSpeech);

                int index = 0;
                string definitionKey = "WordDefinition_" + word + "_" + partOfSpeech + "_" + index;
                while (PlayerPrefs.HasKey(definitionKey))
                {
                    string storedDefinition = PlayerPrefs.GetString(definitionKey);

                    definition += $"Part of Speech: {partOfSpeech}\nDefinition: {storedDefinition}\n\n";

                    int exampleIndex = 0;
                    string exampleKey = "WordExample_" + word + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                    while (PlayerPrefs.HasKey(exampleKey))
                    {
                        string storedExample = PlayerPrefs.GetString(exampleKey);

                        definition += $"Example: {storedExample}\n\n";

                        exampleIndex++;
                        exampleKey = "WordExample_" + word + "_" + partOfSpeech + "_" + index + "_" + exampleIndex;
                    }

                    index++;
                    definitionKey = "WordDefinition_" + word + "_" + partOfSpeech + "_" + index;
                }
            }

            definition = $"Phonetic: {phonetic}\n\n{definition}";

            wordDefinition.text = definition;
        }
        else
        {
            wordDefinition.text = "Definition not found yee";
        }
    }
}