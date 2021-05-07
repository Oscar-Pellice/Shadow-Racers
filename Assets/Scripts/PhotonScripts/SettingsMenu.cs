using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer soundMixer;

    public void setSound (float sound)
    {
        Debug.Log(sound);
        soundMixer.SetFloat("sound", sound);
    }

    public void setVolume (float volume)
    {
        InfoSaver.Instance.volume = volume;
        audioMixer.SetFloat ("volume", volume);
    }

}
