using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Core.Manager;
using UnityEngine.Rendering.UI;
using TMPro;

public class WordLoader : MonoBehaviour
{
    public GameObject wordButtonPrefab;
    public Transform wordButtonContainer;
    public Text topicText;
    public GameObject wordDefinitionObject;
    public Text foundCount;

    public static WordLoader instance;
    private List<string> words;

    public Color selectedWordColor;
    public Color defaultWordColor;
    public Color notSeenWordColor;
    public string selectedWord;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
        TopicManager.instance.LoadTopics();
        foundCount.text = $"Found: {AchievementManager.Instance.GetTotalSeenWords()}/{WordManager.Instance.GetWordCount()}";
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void LoadWords(string topicName)
    {
        WordManager wordManager = WordManager.Instance;
        words = new List<string>();

        foreach (string word in wordManager.GetWordsInTopic(topicName))
        {
            words.Add(word);
        }

        words.Sort();

        foreach (Transform child in wordButtonContainer)
        {
            Destroy(child.gameObject);
        }

        CreateWordButtons();
    }

    private void CreateWordButtons()
    {
        for (int i = 0; i < words.Count; i++)
        {
            GameObject newButton = Instantiate(wordButtonPrefab, wordButtonContainer);

            Text textComponent = newButton.GetComponentInChildren<Text>();

            textComponent.text = words[i];

            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => OnWordClick(textComponent.text));

            ReviewWordButton reviewWordButton = newButton.GetComponent<ReviewWordButton>();
            reviewWordButton.loader = this;
            reviewWordButton.seenWordCount = AchievementManager.Instance.GetSeenWordCount(words[i]);
        }
    }

    public void OnWordClick(string word)
    {
        selectedWord = word;

        Text wordDefinition = wordDefinitionObject.GetComponent<Text>();
        WordManager wordManager = WordManager.Instance;

        if (wordManager.CheckWordIsLoaded(word))
        {
            Word wordData = wordManager.GetWordData(word);
            string phonetic = wordData.phonetic;
            string fullDefinition = "";

            string[] partsOfSpeech = { "noun", "verb", "adjective", "adverb" };
            foreach (string partOfSpeech in partsOfSpeech)
            {
                //Debug.Log("Part of speech: " + partOfSpeech);

                List<Meaning> meanings = wordData.meanings.FindAll(x => x.partOfSpeech == partOfSpeech);

                if (meanings.Count > 0)
                {
                    fullDefinition += $"<b>{partOfSpeech.ToUpper()}</b>\n\n";
                }
                else
                {
                    continue;
                }

                foreach (Meaning meaning in meanings)
                {
                    foreach (Definition definition in meaning.definitions)
                    {
                        fullDefinition += $"+ {definition.definition}\n";

                        foreach (string example in definition.examples)
                        {
                            fullDefinition += $"<i>Ex: {example}</i>\n";
                        }
                        fullDefinition += "\n";
                    }
                }
            }

            fullDefinition = $"<b>{word}</b>\n<i>Phonetic:</i> {phonetic}\n\n{fullDefinition}";

            wordDefinition.text = fullDefinition;
        }
        else
        {
            wordDefinition.text = "Definition not found yee";
        }
    }
}