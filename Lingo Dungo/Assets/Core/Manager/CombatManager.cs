using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    #region Prefabs
    [Header("Text Sprite")]

    public GameObject DamageSprite;

    #endregion

    #region Combatant
    [Header("Combatants")]
    public GameObject[] AlliesModel = new GameObject[4];
    public GameObject[] EnemiesModel = new GameObject[4];

    public Combatant[] Allies = new Combatant[4];
    public Combatant[] Enemies = new Combatant[4];

    public Combatant ThisPlayer;

    public Combatant ActionPerformer;
    public List<Combatant> ActionTargets = new List<Combatant>();
    public Action CurrentAction;
    #endregion

    public List<Action> registeredActions = new List<Action>();
    public List<Action> confirmedActions = new List<Action>();

    public bool ThisPlayerAnswered = false;
    public string RoundCorrectAnswer;

    public CombatState state = CombatState.init;
    public CombatState State { get { return state; } }

    // Start is called before the first frame update

    void Start()
    {
        // This is demo. First puppet will be treat as current player
        ThisPlayer = new Combatant
        {
            ownerClass = new Class(100, 100, 10, 10)
        };

        Allies[0] = ThisPlayer;

        Enemies[0] = new Combatant
        {
            ownerClass = new Class(100, 100, 10, 10)
        };

        Allies[0].AttachModel(AlliesModel[0]);
        Enemies[0].AttachModel(EnemiesModel[0]);

        foreach (Combatant combatant in Allies)
        {
            if (combatant == null)
            {
                continue;
            }
            combatant.UpdateStat();
            combatant.HP = combatant.MaxHP;
        }

        foreach(Combatant combatant in Enemies)
        {
            if (combatant == null)
            {
                continue;
            }
            combatant.UpdateStat();
            combatant.HP = combatant.MaxHP;
        }

        // Hide puppets that don't have owner
        foreach (GameObject ally in AlliesModel)
        {
            Model model = ally.GetComponent<Model>();
            if (model.owner == null)
            {
                ally.SetActive(false);
            }
        }

        foreach (GameObject enemy in EnemiesModel)
        {
            Model model = enemy.GetComponent<Model>();
            if (model.owner == null)
            {
                enemy.SetActive(false);
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
                        AnimationManager aniManager = transform.GetComponent<AnimationManager>();
                        // the winners perform their action
                        foreach(Action action in confirmedActions)
                        {
                            CurrentAction = action;
                            ActionPerformer = action.User;
                            ActionTargets = action.Targets;
                            // Play animation attack
                            switch (action.type)
                            {
                                case Action.SkillType.NormalAttack:
                                    {
                                        action.User.Model.GetComponent<Model>().PlayAttack();
                                        //action.Targets.ForEach(target => aniManager.AttachAnimation(target.Model, "Hit"));
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
                                        //action.Targets.ForEach(target => aniManager.AttachAnimation(target.Model, "Hit"));
                                        break;
                                    }
                            }
                            yield return new WaitForSeconds(2f);
                            CurrentAction = null;
                            ActionPerformer = null;
                            ActionTargets = new List<Combatant>();
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
        word = Utilities.Shuffle(word);
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
            new List<Combatant>(){ Enemies[0] },
            ThisPlayer.NormalAttack,
            Action.SkillType.NormalAttack,
            "",
            "Hit"
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
            new List<Combatant>() { ThisPlayer },
            ThisPlayer.PrimarySkill,
            Action.SkillType.PrimarySkill,
            "",
            "Hit"
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
            new List<Combatant>() { Enemies[0] },
            ThisPlayer.SecondarySkill,
            Action.SkillType.SecondarySkill,
            "",
            "Hit"
        ));
    }

    #endregion

    #region Utilities
    private bool TimerStopped
    {
        get { return CombatTimer.GetComponent<TimerGauge>().stop; }
    }

    public void TriggerActionEvent(string flag)
    {
        AnimationManager animationManager = GetComponent<AnimationManager>();
        // target dmg animation
        switch (flag)
        {
            case "cast":
                break;
            case "trigger":
                if (ActionTargets.Count > 0 && CurrentAction != null)
                {
                    foreach (Combatant target in ActionTargets)
                    {
                        animationManager.AttachAnimation(target.Model, CurrentAction.targetAnimationId);
                        // Temporary use
                        if (Enemies.Contains(target))
                        {
                            target.Model.GetComponent<Model>().PlayDamage();
                        }

                        // Append Damage
                        GameObject damageSprite = Instantiate(DamageSprite, target.Model.transform);
                        damageSprite.GetComponent<DamageSprite>().Setup(
                            Mathf.RoundToInt(40 * CurrentAction.TimeRate),
                            CurrentAction.TimeRate >= 0.5f
                        );
                    }
                }
                break;
        }

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
