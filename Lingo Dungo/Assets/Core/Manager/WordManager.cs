using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Core.Manager
{
    public class WordManager
    {
        public struct MissingWord
        {
            public string Topic;
            public string Word;
            public MissingWord(string topic, string word)
            {
                Topic = topic;
                Word = word;
            }
        }

        static private WordManager instance;
        static public WordManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WordManager();
                }
                return instance;
            }
        }

        private WordManager() { }


        public readonly string vocabularyFolderPath = "Assets/Core/Data/Word";
        public readonly string localDataPath = "Assets/Core/Data/WordData/"; // Path to save word data

        private SortedDictionary<string, List<string>> topicData = new SortedDictionary<string, List<string>>();

        private SortedDictionary<string, Word> wordData =
            new SortedDictionary<string, Word>();

        public List<MissingWord> missingWords = new List<MissingWord>();

        public void LoadWordFromPrefs()
        {
            // 1. Get all vocabulary text files
            string[] topicFiles = Directory.GetFiles(vocabularyFolderPath, "*.txt");
            topicData.Clear();
            wordData.Clear();
            missingWords.Clear();

            foreach (string topicFile in topicFiles)
            {
                string topicName = Path.GetFileNameWithoutExtension(topicFile);
                topicData.Add(topicName, new List<string>());

                // 2. Extract words from the current text file
                string[] words = File.ReadAllLines(topicFile);

                foreach (string word in words)
                {
                    Word wordObject = null;
                    topicData[topicName].Add(word);

                    // 3. Check if the word already loaded
                    if (wordData.ContainsKey(word))
                    {
                        continue;
                    }

                    // 4. Check if the word is stored locally
                    if (!CheckWordIsStoredInPrefs(word))
                    {
                        // 5. If not, push warning and add to missing words list
                        Debug.Log("Word in topic [" + topicName + "] is missing: <" + word + ">");
                        missingWords.Add(new MissingWord(topicName, word));
                        //await DictionaryAPI.FetchAndStoreData(word);
                    }
                    else
                    {
                        // 5. Load word data from local storage into Word object
                        wordObject = LoadWordFromPlayerPrefs(word);
                    }

                    // 6. Add the word object to the dictionary
                    wordData.Add(word, wordObject);
                }
            }
        }

        #region Checker
        public bool CheckWordIsStoredInPrefs(string word)
        {
            return PlayerPrefs.HasKey("WordPhonetic_" + word);
        }

        public bool CheckWordIsLoaded(string word)
        {
            return wordData.ContainsKey(word) && wordData[word] != null;
        }
        #endregion

        #region Getter
        
        public Word GetWordData(string word)
        {
            if (wordData.ContainsKey(word))
            {
                return wordData[word];
            }
            Debug.Log("Word <" + word + "> isn't loaded or doesn't exist in game data");
            return null;
        }

        public List<string> GetWordsInTopic(string topic)
        {
            if (topicData.ContainsKey(topic))
            {
                return topicData[topic];
            }
            Debug.Log("Topic [" + topic + "] isn't loaded or doesn't exist in game data");
            return null;
        }
        
        public List<string> GetAllTopics()
        {
            return topicData.Keys.ToList();
        }

        public List<string> GetAllWords()
        {
            return wordData.Keys.ToList();
        }

        public int GetWordCount()
        {
            return wordData.Count;
        }
        #endregion

        #region Importer

        // Load word data from PlayerPrefs into a Word object
        private Word LoadWordFromPlayerPrefs(string word)
        {
            Word wordObject = new Word
            {
                phonetic = PlayerPrefs.GetString("WordPhonetic_" + word),
                meanings = new List<Meaning>()
            };

            // Iterate through the stored meanings and definitions
            int meaningIndex = 0;
            string partOfSpeechKey = $"WordPartOfSpeech_{word}_{meaningIndex}";

            while (PlayerPrefs.HasKey(partOfSpeechKey))
            {
                Meaning meaning = new Meaning();
                meaning.partOfSpeech = PlayerPrefs.GetString(partOfSpeechKey);
                meaning.definitions = new List<Definition>();

                // Iterate through the stored definitions for this meaning
                int definitionIndex = 0;
                string definitionKey = $"WordDefinition_{word}_{meaningIndex}_{definitionIndex}";

                while (PlayerPrefs.HasKey(definitionKey))
                {
                    Definition definition = new Definition
                    {
                        definition = PlayerPrefs.GetString(definitionKey)
                    };

                    // Check if examples exist for this definition
                    int exampleIndex = 0;
                    string exampleKey = $"WordExample_{word}_{definitionIndex}_{exampleIndex}";

                    definition.examples = new List<string>();
                    while (PlayerPrefs.HasKey(exampleKey))
                    {
                        string examplesString = PlayerPrefs.GetString(exampleKey);
                        definition.examples.Add(examplesString);
                        
                        exampleIndex++;
                        exampleKey = $"WordExample_{word}_{definitionIndex}_{exampleIndex}";
                    }

                    meaning.definitions.Add(definition);

                    definitionIndex++;
                    definitionKey = $"WordDefinition_{word}_{meaningIndex}_{definitionIndex}";
                }

                wordObject.meanings.Add(meaning);
                meaningIndex++;
                partOfSpeechKey = $"WordPartOfSpeech_{word}_{meaningIndex}";
            }

            return wordObject;
        }

        public bool TryImportMissingWord(string missingWord)
        {
            //if (!topicData.ContainsKey(missingWord.Topic))
            //{
            //    Debug.Log("Topic [" + missingWord.Topic + "] doesn't exist");
            //    return false;
            //}
            //if (!topicData[missingWord.Topic].Contains(missingWord.Word))
            //{
            //    Debug.Log("<" + missingWord.Word + "> " + "doesn't exist in topic [" + missingWord.Topic + "]");
            //    return false;
            //}
            if (!CheckWordIsStoredInPrefs(missingWord))
            {
                Debug.Log("<" + missingWord + "> doesn't exist in Prefs");
                return false;
            }
            Word wordObj = LoadWordFromPlayerPrefs(missingWord);
            if (wordData.ContainsKey(missingWord))
            {
                wordData[missingWord] = wordObj;
            }
            else
            {
                wordData.Add(missingWord, wordObj);
            }
            return true;
        }
        #endregion
    }
}
