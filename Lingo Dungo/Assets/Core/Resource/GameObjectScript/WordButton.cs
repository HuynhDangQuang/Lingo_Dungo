using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordButton : MonoBehaviour
{
    public int Index;
    public CombatManager combatManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (combatManager != null)
        {
            Button button = GetComponent<Button>();
            if (combatManager.state == CombatManager.CombatState.answering)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }

        WordPanel wordPanel = transform.parent.GetComponent<WordPanel>();
        if (wordPanel != null)
        {
            Text character = transform.GetChild(0).GetComponent<Text>();
            character.fontSize = wordPanel.transform.childCount <= 10 ? wordPanel.textSize1 : wordPanel.textSize2;
        }
    }

    public void Pop()
    {
        if (transform.parent == null)
        {
            return;
        }
        WordPanel wordPanel = transform.parent.GetComponent<WordPanel>();
        AnswerPanel answerPanel = transform.parent.GetComponent<AnswerPanel>();
        AudioManager.Instance.PlaySFX("ButtonPressed");
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
