using System;

public class AudioManager : Singleton<AudioManager>
{
    public event Action<float> OnBGMVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    public void SetBGMVolume(int value)
    {
        bgmVolume = value / 10f;
        OnBGMVolumeChanged?.Invoke(bgmVolume);
    }

    public void SetSFXVolume(int value)
    {
        sfxVolume = value / 10f;
        OnSFXVolumeChanged?.Invoke(sfxVolume);
    }
}
