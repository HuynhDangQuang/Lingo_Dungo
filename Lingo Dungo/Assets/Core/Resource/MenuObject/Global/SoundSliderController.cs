using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
    public SoundType soundType;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                slider.value = AudioManager.Instance.GetComponent<AudioManager>().musicSource.volume;
                break;
            case SoundType.SFX:
                slider.value = AudioManager.Instance.GetComponent<AudioManager>().sfxSource.volume;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBgmVolume()
    {
        AudioManager.Instance.GetComponent<AudioManager>().MusicVolume(slider.value);
    }

    public void ChangeSfxVolume()
    {
        AudioManager.Instance.GetComponent<AudioManager>().SFXVolume(slider.value);
    }
}
