using RuntimePresets;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Rendering;

public class PresetModelManager : MonoBehaviour
{
    [Serializable]
    public struct NamedPreset
    {
        public string name;
        public RuntimePresets.Preset preset;
    }

    public List<NamedPreset> ModelPresets = new List<NamedPreset>();

    public bool ImportPreset(GameObject target, string presetId)
    {
        NamedPreset? src = ModelPresets.Find(x => x.name == presetId);

        if (src != null)
        {
            SkeletonAnimation skeletonAnimation = target.GetComponent<SkeletonAnimation>();
            Model model = target.GetComponent<Model>();
            if (src!.Value.preset.CanBeAppliedTo(skeletonAnimation))
            {
                //Transfers the values from the preset onto the skeletonAnimation component
                src.Value.preset.ApplyTo(skeletonAnimation);
            }
            else
            {
                Console.Error.WriteLine("Preset's SkeletonAnimation can't be applied to " + target.ToString());
            }

            if (src!.Value.preset.CanBeAppliedTo(model))
            {
                //Transfers the values from the preset onto the skeletonAnimation component
                src.Value.preset.ApplyTo(model);
            }
            else
            {
                Console.Error.WriteLine("Preset's Model can't be applied to " + target.ToString());
            }
            return true;
        }
        else
        {
            Console.Error.WriteLine("Preset is not found");
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
