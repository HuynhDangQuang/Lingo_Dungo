using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPanel : MonoBehaviour
{
    string Word;
    public GameObject answerSheet;
    public GameObject refObject;
    public int TextSize = 60;

    // Start is called before the first frame update
    void Start()
    {
        newWord("apple");
        //GameObject template = transform.GetChild(0).gameObject;
        //GameObject g;

        //for (int i = 0; i < 5; i++)
        //{
        //    g = Instantiate(template, transform);
        //    g.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
        //}

        //// Delete Button template
        //Destroy(template);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newWord(string word)
    {
        Word = word;
        if (transform.childCount > 0)
        {
            ClearWord();
        }
        GameObject g;
        for (int i = 0; i < word.Length; i++)
        {
            g = Instantiate(refObject, transform);
            g.transform.GetChild(0).GetComponent<Text>().text = word[i].ToString();
            g.GetComponent<WordButton>().Index = i;
            g.transform.GetChild(0).GetComponent<Text>().fontSize = TextSize;
        }
    }

    public void ClearWord()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void AddCharFromAnswerSheet(GameObject g)
    {
        g.transform.SetParent(transform);

        // find suitable position for new char
        int bestIndex = -1;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<WordButton>().Index > g.GetComponent<WordButton>().Index)
            {
                bestIndex = child.transform.GetSiblingIndex();
                break;
            }
        }

        bestIndex = bestIndex == -1 ? transform.childCount : bestIndex;

        // set position of this new object
        g.transform.GetChild(0).GetComponent<Text>().fontSize = TextSize;
        g.transform.SetSiblingIndex(bestIndex);
    }

    public void SendCharToAnswerSheet(GameObject g)
    {
        g.transform.SetParent(answerSheet.transform);
        g.transform.GetChild(0).GetComponent<Text>().fontSize = AnswerPanel.TextSize;
    }
}
