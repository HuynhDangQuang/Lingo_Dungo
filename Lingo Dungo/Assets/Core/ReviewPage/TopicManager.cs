using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.IO;
using Assets.Core.Manager;

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
        //string dataPath = Path.Combine(Application.dataPath, "Core/Data/Word");
        WordManager wordManager = WordManager.Instance;

        //string[] files = Directory.GetFiles(dataPath, "*.txt");

        foreach (string topicName in wordManager.GetAllTopics())
        {
            GameObject newTopicButton = Instantiate(topicButtonPrefab, topicButtonContainer);

            newTopicButton.GetComponentInChildren<Text>().text = topicName;


            EventTrigger trigger = newTopicButton.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnTopicClick(topicName); });
            trigger.triggers.Add(entry);
        }
    }

    public void OnTopicClick(string topicName)
    {
        WordLoader.instance.LoadWords(topicName);
    }
}