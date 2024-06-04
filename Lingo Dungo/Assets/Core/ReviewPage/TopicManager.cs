using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;

public class TopicManager : MonoBehaviour
{
    public GameObject topicButtonPrefab;
    public Transform topicButtonContainer;

    public static TopicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void LoadTopics()
    {
        string dataPath = Path.Combine(Application.dataPath, "Core/Data/Word");


        string[] files = Directory.GetFiles(dataPath, "*.txt");

        foreach (string file in files)
        {

            GameObject newTopicButton = Instantiate(topicButtonPrefab, topicButtonContainer);


            string topicName = Path.GetFileNameWithoutExtension(file);


            newTopicButton.GetComponentInChildren<Text>().text = topicName;


            EventTrigger trigger = newTopicButton.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnTopicClick(file); });
            trigger.triggers.Add(entry);
        }
    }

    public void OnTopicClick(string filePath)
    {
        WordLoader.instance.LoadWords(filePath);
    }
}