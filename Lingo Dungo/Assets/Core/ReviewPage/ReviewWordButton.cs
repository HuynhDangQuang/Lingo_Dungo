using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewWordButton : MonoBehaviour
{
    public WordLoader loader;
    public Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (loader != null)
        {
            GetComponent<Image>().color = loader.selectedWord == textComponent.text ?
                loader.selectedWordColor
                : loader.defaultWordColor;
        }
    }
}
