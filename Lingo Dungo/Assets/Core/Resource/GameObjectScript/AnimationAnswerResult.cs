using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAnswerResult : MonoBehaviour
{
    [SpineAnimation]
    public string introAnimationName;
    [SpineAnimation]
    public string idleAnimationName;
    [SpineAnimation]
    public string outtroAnimationName;

    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    Spine.EventData eventData;

    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        spineAnimationState.SetAnimation(0, introAnimationName, false);
        spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
        eventData = skeletonAnimation.Skeleton.Data.FindEvent("end");
        skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
    {
        Debug.Log("Event fire! " + e.Data.Name);

        bool eventMatch = (eventData == e.Data);

        if (eventMatch)
        {
            // Perform animation or trigger something
            Destroy(gameObject);
        }
    }
    public void AnimationEnd()
    {
        spineAnimationState.SetAnimation(0, outtroAnimationName, false);
    }
}
