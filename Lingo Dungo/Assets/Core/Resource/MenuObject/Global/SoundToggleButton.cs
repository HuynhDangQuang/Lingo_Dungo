using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    public SoundType soundType = SoundType.BGM;

    // Start is called before the first frame update
    void Start()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                GetComponent<Toggle>().isOn = !AudioManager.Instance.GetComponent<AudioManager>().musicSource.mute;
                break;
            case SoundType.SFX:
                GetComponent<Toggle>().isOn = !AudioManager.Instance.GetComponent<AudioManager>().sfxSource.mute;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleBGM()
    {
        AudioManager.Instance.GetComponent<AudioManager>().ToggleMusic();
        GetComponent<Toggle>().isOn = !AudioManager.Instance.GetComponent<AudioManager>().musicSource.mute;
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.GetComponent<AudioManager>().ToggleSFX();
        GetComponent<Toggle>().isOn = !AudioManager.Instance.GetComponent<AudioManager>().sfxSource.mute;
    }
}

public enum SoundType
{
    BGM,
    SFX
}