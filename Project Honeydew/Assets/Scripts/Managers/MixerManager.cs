using UnityEngine;
using UnityEngine.Audio;

public class MixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetMasterVolume(float level)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        if (level == -40) level = -80;
        mixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        if (level == -40) level = -80;
        mixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
}