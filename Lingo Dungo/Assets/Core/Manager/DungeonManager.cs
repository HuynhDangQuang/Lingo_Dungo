using Assets.Core.Manager;
using Assets.Core.Resource.Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    DungeonDataManager dungeonDataManager = DungeonDataManager.Instance;

    #region GameObjects


    #region Manager
    [Header("Manager")]
    public PresetModelManager presetModelManager;

    #endregion

    [Header("UI Objects")]
    public GameObject map;
    public Button buttonUp;
    public Button buttonDown;
    public Button buttonLeft;
    public Button buttonRight;
    public Button buttonInteract;

    public FaceEffect faceEffect;
    public DungeonEvent dungeonEvent;
    public PartyReaction partyReaction;
    #endregion

    #region OnMap Actor

    [Header("Actors")]
    public GameObject[] AlliesModel = new GameObject[4];
    public GameObject[] EnemiesModel = new GameObject[4];

    #endregion


    #region Private param
    private Combatant ThisPlayer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dungeonDataManager.combatResult = CombatResult.notFighting;
        AudioManager.Instance.PlayMusic("DungeonTheme");
        DisableAllMoveButtons();
        ThisPlayer = dungeonDataManager.thisPlayer;
        StartCoroutine(StartDungeonCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    private void DisableAllMoveButtons()
    {
        buttonUp.interactable = false;
        buttonDown.interactable = false;
        buttonLeft.interactable = false;
        buttonRight.interactable = false;
    }

    public void ActiveMoveButtons()
    {
        DungeonRoom room = dungeonDataManager.GetCurrentRoom();
        buttonUp.interactable = room.up;
        buttonDown.interactable = room.down;
        buttonLeft.interactable = room.left;
        buttonRight.interactable = room.right;
    }

    private void BringFadeEffectToBackward()
    {
        faceEffect.SendToBack();
    }

    private void BringFadeEffectToFrontward()
    {
        faceEffect.BringToFront();
    }

    #region Change Room Update
    public void UpdateMap()
    {
        int baseX = dungeonDataManager.playerX;
        int baseY = dungeonDataManager.playerY;
        int index = 0;
        for (int y = -2; y <= 2; y++)
        {
            for (int x = -2; x <= 2; x++)
            {
                MapCell mapCell = map.transform.GetChild(index).GetComponent<MapCell>();
                DungeonRoom room = dungeonDataManager.GetRoom(baseX + x, baseY + y);

                if (room == null)
                {
                    mapCell.Setup(false, false, false, false, false);
                    index++;
                    continue;
                }

                bool unknown = false;      
                if (!room.revealed)
                    if (x == 0 && y == -1 && dungeonDataManager.GetCurrentRoom().up)
                    {
                        unknown = true;
                    }
                    else if (x == 0 && y == 1 && dungeonDataManager.GetCurrentRoom().down)
                    {
                        unknown = true;
                    }
                    else if (x == -1 && y == 0 && dungeonDataManager.GetCurrentRoom().left)
                    {
                        unknown = true;
                    }
                    else if (x == 1 && y == 0 && dungeonDataManager.GetCurrentRoom().right)
                    {
                        unknown = true;
                    }
                    else if (x == 0 && y == 0)
                    {
                        dungeonDataManager.GetCurrentRoom().revealed = true;
                    }
                    else
                    {
                        // This room is hidden, not reached yet
                        mapCell.Setup(false, false, false, false, false);
                        index++;
                        continue;
                    }        

                mapCell.Setup(room.up, room.down, room.left, room.right, unknown);

                if (!unknown & room.revealed)
                {
                    if (room.isExit)
                    {
                        mapCell.SetIcon("exit");
                    }
                }
                
                index++;
            }
        }
    }

    public void SetupModels()
    {
        Combatant[] Allies = dungeonDataManager.party;
        for (int i = 0; i < Allies.Length; i++)
        {
            if (Allies[i] != null)
            {
                presetModelManager.ImportPreset(AlliesModel[i], Allies[i].modelId);
                Allies[i].AttachModel(AlliesModel[i]);
                AlliesModel[i].GetComponent<Model>().RefreshModel();
                EnemiesModel[i].SetActive(true);
            }
            else
            {
                AlliesModel[i].SetActive(false);
            }
        }

        Combatant[] Enemies = dungeonDataManager.GetCurrentRoom().enemies;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                presetModelManager.ImportPreset(EnemiesModel[i], Enemies[i].modelId);
                EnemiesModel[i].GetComponent<Model>().RefreshModel();
                EnemiesModel[i].SetActive(true);
            }
            else
            {
                EnemiesModel[i].SetActive(false);
            }
        }
    }

    public void SetupEvent()
    {
        DungeonRoom room = dungeonDataManager.GetCurrentRoom();

        if (room.isExit)
        {
            dungeonEvent.SetupEvent("exit");
            buttonInteract.gameObject.SetActive(!room.CanStartCombat());
            return;
        }

        dungeonEvent.SetupEvent("empty");
        buttonInteract.gameObject.SetActive(false);
    }

    public void DoFindNewRoomEffect()
    {
        foreach (Combatant combatant in dungeonDataManager.party)
        {
            if (combatant != null)
            {
                combatant.GainHp(combatant.MaxHP / 2);
            }
        }
    }
    #endregion

    #region Buttons

    public void MoveUpPressed()
    {
        DisableAllMoveButtons();
        dungeonDataManager.playerY --;
        StartCoroutine(ChangeRoomCoroutine("You are moving to the North..."));
    }

    public void MoveDownPressed()
    {
        DisableAllMoveButtons();
        dungeonDataManager.playerY ++;
        StartCoroutine(ChangeRoomCoroutine("You are moving to the South..."));
    }

    public void MoveLeftPressed()
    {
        DisableAllMoveButtons();
        dungeonDataManager.playerX --;
        StartCoroutine(ChangeRoomCoroutine("You are moving to the West..."));
    }

    public void MoveRightPressed()
    {
        DisableAllMoveButtons();
        dungeonDataManager.playerX ++;
        StartCoroutine(ChangeRoomCoroutine("You are moving to the East..."));
    }

    public void InteractPressed()
    {
        DungeonRoom room = dungeonDataManager.GetCurrentRoom();

        if (room.isExit)
        {
            SceneManager.LoadScene("ConclusionScene");
        }
    }
    #endregion

    #region Coroutine

    IEnumerator StartDungeonCoroutine()
    {
        BringFadeEffectToFrontward();
        faceEffect.InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
       
        // Update model on screen
        SetupModels();

        // Find new room Effect
        if (!dungeonDataManager.GetCurrentRoom().revealed)
        {
            DoFindNewRoomEffect();
        }   
        
        // Load room data
        UpdateMap();

        // Update Event on screen
        SetupEvent();

        if (!dungeonDataManager.isStarted)
        {
            faceEffect.FadeIn(3f, "Adventure begin!");
            yield return new WaitForSeconds(3.5f);
            dungeonDataManager.isStarted = true;
        }
        else
        {
            faceEffect.FadeIn(1f);
            yield return new WaitForSeconds(1.5f);
        }    

        while (!faceEffect.canStartCoroutine)
        {
            yield return new WaitForSeconds(0.01f);
        }

        ActiveMoveButtons();
        BringFadeEffectToBackward();
        yield return null;
    }

    IEnumerator ChangeRoomCoroutine(string description)
    {
        BringFadeEffectToFrontward();
        yield return new WaitForSeconds(0.05f);

        faceEffect.FadeOut(1.5f, description);

        yield return new WaitForSeconds(0.05f);
        while (!faceEffect.canStartCoroutine)
        {
            yield return new WaitForSeconds(0.01f);
        }

        // Update model on screen
        SetupModels();

        // Load room data
        UpdateMap();

        // Update Event on screen
        SetupEvent();

        yield return new WaitForSeconds(0.5f);

        faceEffect.FadeIn(1.5f, description);

        yield return new WaitForSeconds(0.05f);
        while (!faceEffect.canStartCoroutine)
        {
            yield return new WaitForSeconds(0.01f);
        }

        if (dungeonDataManager.GetCurrentRoom().CanStartCombat())
        {
            StartCoroutine(StartCombatCoroutine());
        }
        else
        {
            ActiveMoveButtons();
            BringFadeEffectToBackward();
            yield return null;
        }
    }

    IEnumerator StartCombatCoroutine()
    {
        // Show Exclamination animation
        AudioManager.Instance.PlaySFX("Surprise");
        partyReaction.PlayExclamation();
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX("CombatStart");
        faceEffect.CombatStartFade();

        yield return new WaitForSeconds(0.05f);
        while (!faceEffect.canStartCoroutine)
        {
            yield return new WaitForSeconds(0.01f);
        }

        SceneManager.LoadScene("CombatScene");

        yield return null;
    }

    #endregion
}
