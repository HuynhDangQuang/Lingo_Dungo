using Assets.Core.Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    #region Linked Object

    [Header("UI Objects")]
    public GameObject WordSheet;
    public GameObject AnswerSheet;
    public GameObject CorrectAnswerSheet;
    public GameObject CombatTimer;
    public GameObject ButtonATK;
    public GameObject ButtonSkill1;
    public GameObject ButtonSkill2;
    public GameObject ButtonSettings;
    public GameObject ButtonRemoveAll;
    public Text AnswerHint;
    #endregion

    #region Animations
    [Header("Animations")]
    public GameObject combatStartAnimation;
    public GameObject combatEndAnimation;
    public GameObject resultCorrectAnimation;
    public GameObject resultIncorrectAnimation;
    public GameObject combatWonAnimation;
    public GameObject combatLoseAnimation;
    #endregion

    #region Prefabs
    [Header("Text Sprite")]

    public GameObject DamageSprite;
    public GameObject HealSprite;

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
    public bool HasActionPerformed = false;
    public string RoundCorrectAnswer;

    #region Params
    [Header("Combat Params")]
    public float bonusRateOnWordLength = 0.02f;
    #endregion

    public CombatState state = CombatState.init;
    public CombatResult result = CombatResult.unfinished;

    private bool needToSave = false;

    #region Static Manager
    private AchievementManager achievementManager = AchievementManager.Instance;
    private DungeonDataManager dataManager = DungeonDataManager.Instance;
    PresetModelManager presetModelManager = PresetModelManager.Instance;
    #endregion

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // Play combat BGM
        AudioManager.Instance.PlayMusic("Battle1");

        DisableSkillButtons();

        ThisPlayer = dataManager.thisPlayer;
        for (int i = 0; i < 4; i++)
        {
            Allies[i] = dataManager.party[i];
            Enemies[i] = dataManager.GetCurrentRoom().enemies[i];
        }

        dataManager.GetCurrentRoom().ReleaseEnemies();

        for (int i = 0; i < Allies.Length; i++)
        {
            if (Allies[i] != null)
            {
                presetModelManager.ImportPreset(AlliesModel[i], Allies[i].modelId);
                Allies[i].AttachModel(AlliesModel[i]);
                AlliesModel[i].GetComponent<Model>().RefreshModel();
            }
            else
            {
                AlliesModel[i].SetActive(false);
            }
        }

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                presetModelManager.ImportPreset(EnemiesModel[i], Enemies[i].modelId);
                Enemies[i].AttachModel(EnemiesModel[i]);
                EnemiesModel[i].GetComponent<Model>().RefreshModel();
            }
            else
            {
                EnemiesModel[i].SetActive(false);
            }
        }

        AnswerHint.text = "";

        StartCoroutine(DoCombatRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CombatState.waitForExplaination || state == CombatState.performingAction)
        {
            EnableSettingsButton();
        }
        else
        {
            DisableSettingsButton();
        }

        if (state == CombatState.answering && WordSheet.transform.childCount == 0)
        {
            EnableAvailableSkillButtons();
        }
        else
        {
            DisableSkillButtons();
        }

        if (state == CombatState.answering && AnswerSheet.transform.childCount > 0)
        {
            ButtonRemoveAll.GetComponent<Button>().interactable = true;
        }
        else
        {
            ButtonRemoveAll.GetComponent<Button>().interactable = false;
        }


        // Update skill cost
        Text primarySkillCost = GameObject.FindGameObjectWithTag("PrimarySkillCost").GetComponent<Text>();
        Text secondarySkillCost = GameObject.FindGameObjectWithTag("SecondarySkillCost").GetComponent<Text>();
        if (primarySkillCost != null)
        {
            primarySkillCost.text = ThisPlayer.PrimarySkill.Cost.ToString();
        }
        if (secondarySkillCost != null)
        {
            secondarySkillCost.text = ThisPlayer.SecondarySkill.Cost.ToString();
        }
    }

    IEnumerator DoCombatRoutine()
    {
        GameObject answerResult = null;
        while (true)
        {
            switch (state)
            {
                case CombatState.init:
                    {
                        yield return new WaitForSeconds(0.5f);
                        // play combat start animation
                        Instantiate(combatStartAnimation, null);
                        yield return new WaitForSeconds(0.1f);
                        AudioManager.Instance.PlaySFX("Sword4");
                        yield return new WaitForSeconds(1.9f);
                        state = CombatState.getNewQuestion;
                        break;
                    }
                case CombatState.getNewQuestion:
                    {
                        WordSheet.GetComponent<WordPanel>().NewWord(CreateQuestion());
                        AnswerHint.text = $"Hint: {WordManager.Instance.GetRandomDefinition(RoundCorrectAnswer)}";

                        TimerGauge timer = CombatTimer.GetComponent<TimerGauge>();
                        timer.maxValue = 250 + RoundCorrectAnswer.Length * 80;
                        timer.StartTimer();

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

                        // Enemies answer
                        RegisterEnemiesAction();

                        yield return new WaitForSeconds(0.2f);
                        state = CombatState.processAnswer;

                        break;
                    }
                case CombatState.processAnswer:
                    {
                        ThisPlayerAnswered = false;
                        bool isThisPlayerCorrect = false;
                        // Validate action
                        foreach (Action action in registeredActions)
                        {
                            if (action.Answer == RoundCorrectAnswer)
                            {
                                confirmedActions.Add(action);
                                if (action.User == ThisPlayer)
                                {
                                    isThisPlayerCorrect = true;
                                }
                            }
                        }

                        confirmedActions = confirmedActions.OrderByDescending(action => action.TimeRate).ToList();

                        registeredActions.Clear();

                        answerResult = Instantiate(isThisPlayerCorrect ? resultCorrectAnimation : resultIncorrectAnimation, null);
                        AudioManager.Instance.PlaySFX(isThisPlayerCorrect ? "AnswerCorrect" : "AnswerWrong");
                        yield return new WaitForSeconds(0.5f);

                        // Show what is the correct answer
                        CorrectAnswerBoard correctBoard = CorrectAnswerSheet.GetComponent<CorrectAnswerBoard>();
                        correctBoard.Show(RoundCorrectAnswer);
                        correctBoard.transform.SetSiblingIndex(correctBoard.transform.childCount - 1);

                        yield return new WaitForSeconds(0.5f);

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
                            
                            // Check if Performer can perform action
                            if (ActionPerformer.IsDied)
                            {
                                continue;
                            }
                            HasActionPerformed = true;

                            // Play animation attack
                            switch (action.Type)
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
                            yield return new WaitForSeconds(1.2f);
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
                        if (!HasActionPerformed)
                        {
                            yield return new WaitForSeconds(2.5f);
                        }
                        else
                        {
                            yield return new WaitForSeconds(1.5f);
                            HasActionPerformed = false;
                        }
                        // Kill the correct/incorrect result animation
                        answerResult.GetComponent<AnimationLong>().AnimationEnd();

                        // Clear the answer sheet
                        WordSheet.GetComponent<WordPanel>().ClearWord();
                        AnswerSheet.GetComponent<AnswerPanel>().ClearWord();
                        CombatTimer.GetComponent<TimerGauge>().CurrentValue = 0;
                        AnswerHint.text = "";

                        CorrectAnswerBoard correctBoard = CorrectAnswerSheet.GetComponent<CorrectAnswerBoard>();
                        correctBoard.Hide();
                        yield return new WaitForSeconds(1f);
                        correctBoard.transform.SetSiblingIndex(0);
                        
                        if (needToSave)
                        {
                            needToSave = false;
                            SaveManager.Instance.Save();
                        }

                        // Check if combat end
                        result = CheckCombatResult();
                        switch (result)
                        {
                            case CombatResult.win:
                                {
                                    // Play victory animation
                                    Instantiate(combatWonAnimation, null);
                                    yield return new WaitForSeconds(4f);

                                    // Handle other thing
                                    state = CombatState.end;

                                    break;
                                }
                            case CombatResult.lose:
                                {
                                    // Play gameover animation
                                    Instantiate(combatLoseAnimation, null);
                                    yield return new WaitForSeconds(4f);

                                    // Handle other thing
                                    state = CombatState.end;
                                    break;
                                }
                            case CombatResult.unfinished:
                                {
                                    // Handle end turn effect
                                    ReduceBarrier();

                                    state = CombatState.getNewQuestion;
                                    break;
                                }
                        }

                        break;
                    }
                case CombatState.end:
                    {
                        // Clear combat
                        List<Combatant> all = new List<Combatant>();
                        all.AddRange(Allies);
                        all.AddRange(Enemies);
                        foreach (Combatant combatant in all)
                        {
                            if (combatant != null)
                                combatant.Barrier = 0;
                        }

                        switch (result)
                        {
                            case CombatResult.win:
                                {
                                    SceneManager.LoadScene("DungeonScene");
                                    break;
                                }
                            case CombatResult.lose:
                                {
                                    dataManager.combatResult = CombatResult.lose;
                                    SceneManager.LoadScene("ConclusionScene");
                                    break;
                                }
                        }
                        StopCoroutine(DoCombatRoutine());
                        yield return new WaitForSeconds(0f);
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

    private string CreateQuestion()
    {
        int index = Random.Range(0,5);
        string word = dataManager.GetWord().ToUpper();
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
        AudioManager.Instance.PlaySFX("SkillPressed");
        ThisPlayerAnswered = true;
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            new List<Combatant>(){ Enemies[0] },
            ThisPlayer.NormalAttack,
            Action.SkillType.NormalAttack,
            ThisPlayer.NormalAttack.CastAnimation,
            ThisPlayer.NormalAttack.TargetsAnimation
        ));
    }

    public void ButtonSkill1Pressed()
    {
        if (ThisPlayerAnswered || state != CombatState.answering)
        {
            return;
        }
        AudioManager.Instance.PlaySFX("SkillPressed");
        ThisPlayerAnswered = true;
        ThisPlayer.GainMp(-ThisPlayer.PrimarySkill.Cost);
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            new List<Combatant>() { ThisPlayer },
            ThisPlayer.PrimarySkill,
            Action.SkillType.PrimarySkill,
            ThisPlayer.PrimarySkill.CastAnimation,
            ThisPlayer.PrimarySkill.TargetsAnimation
        ));
    }

    public void ButtonSkill2Pressed()
    {
        if (ThisPlayerAnswered || state != CombatState.answering)
        {
            return;
        }
        AudioManager.Instance.PlaySFX("SkillPressed");
        ThisPlayerAnswered = true;
        ThisPlayer.GainMp(-ThisPlayer.SecondarySkill.Cost);
        registeredActions.Add(new Action(
            CombatTimer.GetComponent<TimerGauge>().TimeRate,
            AnswerSheet.GetComponent<AnswerPanel>().GetAnswerString(),
            ThisPlayer,
            new List<Combatant>() { Enemies[0] },
            ThisPlayer.SecondarySkill,
            Action.SkillType.SecondarySkill,
            ThisPlayer.SecondarySkill.CastAnimation,
            ThisPlayer.SecondarySkill.TargetsAnimation
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
        Combatant user = CurrentAction.User;
        // target dmg animation
        switch (flag)
        {
            case "cast":
                // Handle SFX
                AudioManager.Instance.PlaySFX(CurrentAction.Skill.CastSFX);
                break;

            case "trigger":
                if (CurrentAction.Type == Action.SkillType.NormalAttack && user is Player)
                {
                    float bonus = CurrentAction.TimeRate + GetBonusRateOnWordLength(user);
                    user.MP += Mathf.RoundToInt(bonus * 20f);
                }
                CurrentAction.Skill.ApplySelfEffect(user, CurrentAction.TimeRate);

                if (ActionTargets.Count > 0 && CurrentAction != null)
                {
                    foreach (Combatant target in ActionTargets)
                    {
                        // Append Damage
                        float bonus = CurrentAction.TimeRate + GetBonusRateOnWordLength(user);
                        int damage = CaculateDamage(user, target, bonus);

                        // Barrier Block
                        int blockAmount = 0;
                        if (damage > 0 && Utilities.IsOpponent(user, target) && target.Barrier > 0) 
                        {
                            blockAmount = Mathf.Min(damage, target.Barrier);
                            target.Barrier -= blockAmount;
                            damage -= blockAmount; 
                        }

                        target.GainHp(-damage);

                        // Append Effect
                        CurrentAction.Skill.ApplyTargetEffect(user, target, CurrentAction.TimeRate);


                        // Popup
                        if (blockAmount > 0)
                        {
                            // Will be implemented later
                        }

                        if (damage > 0)
                        {
                            // achievement
                            if (user == ThisPlayer)
                            {
                                achievementManager.ProgressDealDamage(damage);
                                needToSave = true;
                            }

                            GameObject damageSprite = Instantiate(DamageSprite, target.Model.transform);
                            damageSprite.GetComponent<DamageSprite>().Setup(
                                Mathf.RoundToInt(damage),
                                CurrentAction.TimeRate >= user.MinCriticalTimeRate
                            );
                        }
                        else if (damage < 0)
                        {
                            GameObject healSprite = Instantiate(HealSprite, target.Model.transform);
                            healSprite.GetComponent<DamageSprite>().Setup(
                                Mathf.RoundToInt(damage),
                                CurrentAction.TimeRate >= user.MinCriticalTimeRate
                            );
                        }
                        else if (CurrentAction.Skill.DamageType != DamageType.None)
                        {
                            // Will be implemented later
                        }

                        // Handle Animation
                        animationManager.AttachAnimation(target.Model, CurrentAction.targetAnimationId);
                        
                        // Handle Targets motion
                        if (Utilities.IsOpponent(user, target))
                        {
                            if (target.HP == 0)
                                target.Model.GetComponent<Model>().PlayDie();
                            else
                                target.Model.GetComponent<Model>().PlayDamage();
                        }
                        else if (target.HP == 0)
                        {
                            target.Model.GetComponent<Model>().PlayDie();
                        }

                        // Handle SFX
                        AudioManager.Instance.PlaySFX(CurrentAction.Skill.TargetsHitSFX);
                    }
                }
                break;
        }

    }

    public int CaculateDamage(Combatant user, Combatant target, float TimeRate)
    {
        float critBonus = CurrentAction.TimeRate > user.MinCriticalTimeRate ? 1.5f * user.CriticalDamageBonus : 1.5f;
        float value = CurrentAction.Skill.GetDamage(user, target, Mathf.Max(TimeRate, 0.3f) * critBonus);
        value *= target.DamageTakenRate;
        return Mathf.RoundToInt(value);
    }

    public void ReduceBarrier()
    {
        foreach (Combatant combatant in Allies)
        {
            if (combatant == null)
                continue;
            if (!combatant.JustGainBarrier)
                combatant.Barrier -= Mathf.RoundToInt(Mathf.Max(5, combatant.Barrier * 0.1f));
            else
                combatant.JustGainBarrier = false;
        }

        foreach (Combatant combatant in Enemies)
        {
            if (combatant == null)
                continue;
            if (!combatant.JustGainBarrier)
                combatant.Barrier -= Mathf.RoundToInt(Mathf.Max(5, combatant.Barrier * 0.1f));
            else 
                combatant.JustGainBarrier = false;
        }
    }

    public float GetBonusRateOnWordLength(Combatant user)
    {
        if (user is Enemy)
        {
            return 0f;
        }
        return bonusRateOnWordLength * RoundCorrectAnswer.Length;
    }

    void RegisterEnemiesAction()
    {
        foreach (Enemy enemy in Enemies)
        {
            if (enemy == null) continue;
            Action action = enemy.RegisterAction();
            if (action != null)
            {
                registeredActions.Add(action);
                enemy.MP = 0;
            }
            else
            {
                enemy.GainMp(Mathf.RoundToInt(enemy.mpRegen * enemy.GetFinalAnswerTimeRate()));
            }
        }
    }

    CombatResult CheckCombatResult()
    {
        bool aliveAllies = false;
        foreach (Combatant combatant in Allies)
        {
            if (combatant != null && !combatant.IsDied)
            {
                aliveAllies = true;
                break;
            }
        }

        bool aliveEnemies = false;
        foreach (Combatant combatant in Enemies)
        {
            if (combatant != null && !combatant.IsDied)
            {
                aliveEnemies = true;
                break;
            }
        }

        if (!aliveAllies && aliveEnemies)
        {
            return CombatResult.lose;
        }

        if (aliveAllies && !aliveEnemies)
        {
            return CombatResult.win;
        }

        return CombatResult.unfinished;
    }
    #endregion

    #region UI Control

    private void DisableSkillButtons()
    {
        ButtonATK.GetComponent<Button>().interactable = false;
        ButtonSkill1.GetComponent<Button>().interactable = false;
        ButtonSkill2.GetComponent<Button>().interactable = false;
    }

    private void EnableAvailableSkillButtons()
    {
        ButtonATK.GetComponent<Button>().interactable = true;
        if (ThisPlayer.MP >= ThisPlayer.PrimarySkill.Cost)
        {
            ButtonSkill1.GetComponent<Button>().interactable = true;
        }
        if (ThisPlayer.MP >= ThisPlayer.SecondarySkill.Cost)
        {
            ButtonSkill2.GetComponent<Button>().interactable = true;
        }
    }

    private void DisableSettingsButton()
    {
        ButtonSettings.GetComponent<Button>().interactable = false;
    }

    private void EnableSettingsButton()
    {
        ButtonSettings.GetComponent<Button>().interactable = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void StopPauseGame()
    {
        Time.timeScale = 1;
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
        end
    }
}

public enum CombatResult
{
    win,
    lose,
    unfinished,
    notFighting
}
