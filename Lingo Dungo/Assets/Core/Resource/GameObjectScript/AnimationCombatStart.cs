using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationCombatStart : MonoBehaviour
{
    [SpineAnimation]
    public string animationName;

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

        spineAnimationState.SetAnimation(0, animationName, false);
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
}
