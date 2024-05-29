using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Model : MonoBehaviour
{

    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string idleAnimationName;

    [SpineAnimation]
    public string damageAnimationName;

    [SpineAnimation]
    public string dieAnimationName;

    [SpineAnimation]
    public string attackAnimationName;

    [SpineAnimation]
    public string primarySkillAnimationName;

    [SpineAnimation]
    public string secondarySkillAnimationName;

    [Header("Transitions")]
    [SpineAnimation]
    public string dyingAnimationName;

    //public float runWalkDuration = 1.5f;
    #endregion

    SkeletonAnimation skeletonAnimation;

    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public Combatant owner = null;

    Spine.EventData eventData;

    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        spineAnimationState.SetAnimation(0, idleAnimationName, true);
        eventData = skeletonAnimation.Skeleton.Data.FindEvent("trigger");
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Actions

    public void PlayDamage()
    {
        spineAnimationState.SetAnimation(0, damageAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

    public void PlayDie()
    {
        spineAnimationState.SetAnimation(0, dyingAnimationName, false);
        spineAnimationState.AddAnimation(0, dieAnimationName, true, 0);
    }

    public void PlayAttack()
    {
        spineAnimationState.SetAnimation(0, attackAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

    public void PlayPrimarySkill()
    {
        spineAnimationState.SetAnimation(0, primarySkillAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

    public void PlaySecondarySkill()
    {
        spineAnimationState.SetAnimation(0, secondarySkillAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

    #endregion

    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log("Event fire! " + e.Data.Name);

        bool eventMatch = (eventData == e.Data);
    
        if (eventMatch)
        {
            GameObject manager = GameObject.FindGameObjectsWithTag("GameController").First();
            if (manager != null)
            {
                CombatManager combatManager = manager.GetComponent<CombatManager>();
                combatManager.TriggerActionEvent("trigger");
            }
            // Perform animation or trigger something
        }
    }
}
