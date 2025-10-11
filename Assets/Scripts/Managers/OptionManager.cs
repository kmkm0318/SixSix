using System;
using UnityEngine;

public class OptionManager : Singleton<OptionManager>
{
    private const string OPTION_DATA_NAME = "OptionData";

    public OptionData OptionData { get; private set; }

    override protected void Awake()
    {
        base.Awake();
        LoadOptionDataSO();
    }

    private void Start()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged += OnOptionValueChanged;
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged -= OnOptionValueChanged;
    }

    private void OnOptionValueChanged(OptionType type, int value)
    {
        switch (type)
        {
            case OptionType.GameSpeed:
                OptionData.gameSpeed = value;
                break;
            case OptionType.AbilityDiceAutoKeep:
                OptionData.abilityDiceAutoKeep = value;
                break;
            case OptionType.Language:
                OptionData.language = value;
                break;
            case OptionType.Fullscreen:
                OptionData.fullscreen = value;
                break;
            case OptionType.Resolution:
                OptionData.resolution = value;
                break;
            case OptionType.MasterVolume:
                OptionData.masterVolume = value;
                break;
            case OptionType.BGMVolume:
                OptionData.bgmVolume = value;
                break;
            case OptionType.SFXVolume:
                OptionData.sfxVolume = value;
                break;
        }
        SaveOptionDataSO();
    }
    #endregion

    #region SaveLoad
    private void SaveOptionDataSO()
    {
        string json = JsonUtility.ToJson(OptionData);
        PlayerPrefs.SetString(OPTION_DATA_NAME, json);
        PlayerPrefs.Save();
    }

    private void LoadOptionDataSO()
    {
        string json = PlayerPrefs.GetString(OPTION_DATA_NAME, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            OptionData = new()
            {
                gameSpeed = 0,
                abilityDiceAutoKeep = 0,
                language = 0,
                fullscreen = 0,
                resolution = 0,
                masterVolume = 5,
                bgmVolume = 5,
                sfxVolume = 5
            };
        }
        else
        {
            OptionData = JsonUtility.FromJson<OptionData>(json);
        }
    }
    #endregion
}

[Serializable]
public class OptionData
{
    public int gameSpeed;
    public int abilityDiceAutoKeep;
    public int language;
    public int fullscreen;
    public int resolution;
    public int masterVolume;
    public int bgmVolume;
    public int sfxVolume;
}