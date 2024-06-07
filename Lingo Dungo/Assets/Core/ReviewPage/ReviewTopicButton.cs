using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewTopicButton : MonoBehaviour
{
    public TopicManager topicManager;
    public Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (topicManager != null)
        {
            GetComponent<Image>().color = topicManager.selectedTopic == textComponent.text ?
                topicManager.selectedTopicColor
                : topicManager.defaultTopicColor;
        }
    }
}
