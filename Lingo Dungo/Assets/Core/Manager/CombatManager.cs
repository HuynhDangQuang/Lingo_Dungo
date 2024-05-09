using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public GameObject WordSheet;
    public GameObject AnswerSheet;
    public GameObject CombatTimer;
    public bool ThisPlayerAnswered;
    public string RoundCorrectAnswer;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 4; i++)
        {
            allies.Add(GameObject.FindGameObjectWithTag("Actor" + i));
        }
        StartCoroutine(DoCombatRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoCombatRoutine()
    {
        Model actor1 = allies[0].GetComponent<Model>();
        CombatState state = CombatState.init;
        while (true)
        {
            switch (state)
            {
                case CombatState.init:
                    {
                        // play combat start animation
                        yield return new WaitForSeconds(1f);
                        state = CombatState.getNewQuestion;
                        break;
                    }
                case CombatState.getNewQuestion:
                    {
                        
                        break;
                    }
                case CombatState.answering:
                    {
                        // Check if all player have already answered question
                        if (ThisPlayerAnswered)
                        {

                            state = CombatState.answerConfirm;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.01f);
                        }
                        break;
                    }
                case CombatState.performingAction:
                    {
                        // the winners perform their action
                        actor1.PlayAttack();

                        yield return new WaitForSeconds(2f);

                        state = CombatState.waitForExplaination;
                        break;
                    }
                case CombatState.waitForExplaination:
                    {
                        // Clear the answer sheet
                        WordSheet.GetComponent<WordPanel>().ClearWord();
                        AnswerSheet.GetComponent<AnswerPanel>().ClearWord();

                        // Show what is the correct answer
                        
                        yield return new WaitForSeconds(3f);
                        state = CombatState.getNewQuestion;
                        break;
                    }
                default:
                    {
                        yield return 0;
                        break;
                    }
            }
        }
    }

    #region Action
    private string createQuestion()
    {
        string word = "hello".ToUpper();
        RoundCorrectAnswer = word;
        word = Utility.Shuffle(word);
        return word;
    }


    #endregion

    #region Utility
    bool TimerStopped
    {
        get { return CombatTimer.GetComponent<TimerGauge>().stop; }
    }


    #endregion

    enum CombatState
    {
        init,
        answering,
        answerConfirm,
        processAnswer,
        performingAction,
        waitForExplaination,
        getNewQuestion,
    }
}
