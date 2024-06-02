using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordButton : MonoBehaviour
{
    public int Index;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pop()
    {
        if (transform.parent == null)
        {
            return;
        }
        WordPanel wordPanel = transform.parent.GetComponent<WordPanel>();
        AnswerPanel answerPanel = transform.parent.GetComponent<AnswerPanel>();
        if (wordPanel != null)
        {
            wordPanel.SendCharToAnswerSheet(this.gameObject);
        }
        else if (answerPanel != null)
        {
            answerPanel.SendCharToWordSheet(this.gameObject);
        }
    }
}
