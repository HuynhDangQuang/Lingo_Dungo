using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewWordButton : MonoBehaviour
{
    public WordLoader loader;
    public Text textComponent;
    public Text countComponent;
    public int seenWordCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countComponent.text = $"Seen: {seenWordCount}"; 
        if (loader != null)
        {
            GetComponent<Image>().color = loader.selectedWord == textComponent.text ?
                loader.selectedWordColor
                : (seenWordCount > 0 ? loader.defaultWordColor : loader.notSeenWordColor);
        }
    }
}
