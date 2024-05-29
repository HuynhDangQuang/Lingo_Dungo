using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PresetModelManager;

public class AnimationManager : MonoBehaviour
{
    [Serializable]
    public struct PresetAnimation
    {
        public string name;
        public GameObject animation;
    }

    public List<PresetAnimation> AnimationPresets = new List<PresetAnimation>();

    public void AttachAnimation(GameObject target, string animationId)
    {
        PresetAnimation? src = AnimationPresets.Find(x => x.name == animationId);

        if (src != null)
        {
            Instantiate(src.Value.animation, target.transform);
        }
    }
}
