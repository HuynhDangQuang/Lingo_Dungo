using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPanel : MonoBehaviour
{
    string Word;
    public GameObject answerSheet;
    public GameObject refObject;
    public readonly int textSize1 = 60;
    public readonly int textSize2 = 40;
    public readonly Vector2 cellSize1 = new Vector2(80, 80);
    public readonly Vector2 cellSize2 = new Vector2(60, 60);

    // Start is called before the first frame update
    void Start()
    {
        //NewWord("apple");
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
        Beardy.GridLayoutGroup gridLayoutGroup = GetComponent<Beardy.GridLayoutGroup>();
        gridLayoutGroup.cellSize = transform.childCount <= 10 ? cellSize1 : cellSize2;
    }

    public void NewWord(string word)
    {
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        Word = word;
        if (transform.childCount > 0)
        {
            ClearWord();
        }
        GameObject g;
        for (int i = 0; i < word.Length; i++)
        {
            g = Instantiate(refObject, transform);
            Text textComponent = g.transform.GetChild(0).GetComponent<Text>();
            textComponent.text = word[i].ToString();
            textComponent.fontSize = transform.childCount <= 10 ? textSize1 : textSize2;
            WordButton wordButton = g.GetComponent<WordButton>();
            wordButton.Index = i;
            wordButton.combatManager = manager.GetComponent<CombatManager>();
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
        g.transform.GetChild(0).GetComponent<Text>().fontSize = transform.childCount <= 10 ? textSize1 : textSize2;
        g.transform.SetSiblingIndex(bestIndex);
    }

    public void SendCharToAnswerSheet(GameObject g)
    {
        g.transform.SetParent(answerSheet.transform);
        g.transform.GetChild(0).GetComponent<Text>().fontSize = AnswerPanel.TextSize;
    }
}
