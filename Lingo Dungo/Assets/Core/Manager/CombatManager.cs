using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    #region Linked Object

    [Header("UI Objects")]
    public GameObject WordSheet;
    public GameObject AnswerSheet;
    public GameObject CombatTimer;
    public GameObject ButtonATK;
    public GameObject ButtonSkill1;
    public GameObject ButtonSkill2;
    #endregion

    #region Combatant
    [Header("Combatants")]
    public List<GameObject> AlliesModel = new List<GameObject>();
    public List<GameObject> EnemiesModel = new List<GameObject>();

    public List<Combatant> Allies = new List<Combatant>();
    public List<Combatant> Enemies = new List<Combatant>();

    public Combatant ThisPlayer;
    #endregion

    public List<Action> registeredActions = new List<Action>();
    private List<Action> confirmedActions = new List<Action>();

    public bool ThisPlayerAnswered = false;
    public string RoundCorrectAnswer;

    public CombatState state = CombatState.init;
    public CombatState State { get { return state; } }

    // Start is called before the first frame update

    void Start()
    {
        // This is demo. First puppet will be treat as current player
        ThisPlayer = new Combatant();
        // fake stat
        ThisPlayer.MaxHP = 20;
        ThisPlayer.HP = 20;
        ThisPlayer.MaxMP = 10;
        ThisPlayer.MP = 10;

        ThisPlayer.AttachModel(AlliesModel[0]);

        // Hide puppets that don't have owner
        foreach (GameObject ally in AlliesModel)
        {
            Model model = ally.GetComponent<Model>();
            if (model.owner == null)
            {
                ally.SetActive(false);
            }
        }

        StartCoroutine(DoCombatRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CombatState.answering && WordSheet.transform.childCount == 0)
        {
            ButtonATK.GetComponent<Button>().interactable = true;
            ButtonSkill1.GetComponent<Button>().interactable = true;
            ButtonSkill2.GetComponent<Button>().interactable = true;
        }
        else
        {
            ButtonATK.GetComponent<Button>().interactable = false;
            ButtonSkill1.GetComponent<Button>().interactable = false;
            ButtonSkill2.GetComponent<Button>().interactable = false;
        }
    }

    IEnumerator DoCombatRoutine()
    {
        //Model actor1 = AlliesModel[0].GetComponent<Model>();
        while (true)
        {
            switch (state)
            {
                case CombatState.init:
                    {
                        // play combat start animation
                        yield return new WaitForSeconds(2f);
                        state = CombatState.getNewQuestion;
                        break;
                    }
                case CombatState.getNewQuestion:
                    {
                        WordSheet.GetComponent<WordPanel>().NewWord(createQuestion());
                        CombatTimer.GetComponent<TimerGauge>().StartTimer();
                        yield return new WaitForSeconds(0.5f);
                        state = CombatState.answering;
                        break;
                    }
                case CombatState.answering:
                    {
                        // Check if all player have already answered question
                        if (ThisPlayerAnswered || TimerStopped)
                        {
                            state = CombatState.answerConfirm;
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.01f);
                        }
                        break;
                    }
                case CombatState.answerConfirm:
                    {
                        CombatTimer.GetComponent<TimerGauge>().StopTimer();
                        // Await other player answer the question
                        


                        yield return new WaitForSeconds(0.2f);
                        state = CombatState.processAnswer;

                        break;
                    }
                case CombatState.processAnswer:
                    {
                        ThisPlayerAnswered = false;
                        // Validate action
                        foreach (Action action in registeredActions)
                        {
                            if (action.Answer == RoundCorrectAnswer)
                            {
                                confirmedActions.Add(action);
                            }
                        }
                        registeredActions.Clear();

                        yield return new WaitForSeconds(0.2f);
                        state = CombatState.performingAction;
                        break;
                    }
                case CombatState.performingAction:
                    {
                        // the winners perform their action
                        foreach(Action action in confirmedActions)
                        {
                            // Play animation attack
                            switch (action.type)
                            {
                                case Action.SkillType.NormalAttack:
                                    {
                                        action.User.Model.GetComponent<Model>().PlayAttack();
                                        break;
                                    }
                                case Action.SkillType.PrimarySkill:
                                    {
                                        action.User.Model.GetComponent<Model>().PlayPrimarySkill();
                                        break;
                                    }
                                case Action.SkillType.SecondarySkill:
                                    {
                                        action.User.Model.GetComponent<Model>().PlaySecondarySkill();
                                        break;
                                    }
                            }
                            yield return new WaitForSeconds(2f);
                        }
                        confirmedActions.Clear();
                       
                        //actor1.PlayAttack();

                        yield return new WaitForSeconds(0.5f);

                        state = CombatState.waitForExplaination;
                        break;
                    }
                case CombatState.waitForExplaination:
                    {
                        // Clear the answer sheet
                        WordSheet.GetComponent<WordPanel>().ClearWord();
                        AnswerSheet.GetComponent<AnswerPanel>().ClearWord();
                        CombatTimer.GetComponent<TimerGauge>().CurrentValue = 0;
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

    // demo test case
    string[] demoTest = { "hello", "apple", "water", "unity", "engine", "demo" };

    private string createQuestion()
    {
        int index = Random.Range(0,5);
        string word = demoTest[index].ToUpper();
        RoundCorrectAnswer = word.ToLower();
        word = Utility.Shuffle(word);
        return word;
    }

    public void ButtonNormalAttackPressed()
    {
        if (ThisPlayerAnswered || state != CombatState.answering)
        {
            return;
        }
        ThisPlayerAnswered = true;
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            null,
            null,
            Action.SkillType.NormalAttack
        ));
    }

    public void ButtonSkill1Pressed()
    {
        if (ThisPlayerAnswered || state != CombatState.answering)
        {
            return;
        }
        ThisPlayerAnswered = true;
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            null,
            null,
            Action.SkillType.PrimarySkill
        ));
    }

    public void ButtonSkill2Pressed()
    {
        if (ThisPlayerAnswered || state != CombatState.answering)
        {
            return;
        }
        ThisPlayerAnswered = true;
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            null,
            null,
            Action.SkillType.SecondarySkill
        ));
    }

    #endregion

    #region Utility
    private bool TimerStopped
    {
        get { return CombatTimer.GetComponent<TimerGauge>().stop; }
    }
    #endregion

    public enum CombatState
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
