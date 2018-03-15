using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    English,
    Chinese
}

public class SettingManager : MonoBehaviour {
    public static SettingManager instance;

    [SerializeField]
    private InputDevice inputDevice = InputDevice.MouseAndKeyboard;
    public InputDevice InputDevice
    {
        get
        {
            return inputDevice;
        }
    }
    [SerializeField]
    private Language language = Language.English;
    public Language Language
    {
        get
        {
            return Language;
        }
    }
    [SerializeField]
    private float audioVolumn = 1;
    public float AudioVolumn
    {
        get
        {
            return audioVolumn;
        }
    }

    void Awake()
    {
        instance = this;
    }

    public void SetInputDevice(InputDevice newInputDevice)
    {
        if (newInputDevice != inputDevice)
        {
            inputDevice = newInputDevice;
        }
    }

    public void SetLanguage(Language newLanguage)
    {
        if (newLanguage != language)
        {
            language = newLanguage;
        }
    }

    public void SetAudioVolumn(float newAudioVolumn)
    {
        if (newAudioVolumn != audioVolumn)
        {
            audioVolumn = newAudioVolumn;
        }
    }
}
