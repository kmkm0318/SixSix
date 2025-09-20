using System;
using UnityEngine;

public class OptionManager : Singleton<OptionManager>
{
    private const string OPTION_DATA_NAME = "OptionData";

    private OptionData optionData;
    public OptionData OptionData => optionData;

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
                optionData.gameSpeed = value;
                break;
            case OptionType.AbilityDiceAutoKeep:
                optionData.abilityDiceAutoKeep = value;
                break;
            case OptionType.Language:
                optionData.language = value;
                break;
            case OptionType.Fullscreen:
                optionData.fullscreen = value;
                break;
            case OptionType.Resolution:
                optionData.resolution = value;
                break;
            case OptionType.MasterVolume:
                optionData.masterVolume = value;
                break;
            case OptionType.BGMVolume:
                optionData.bgmVolume = value;
                break;
            case OptionType.SFXVolume:
                optionData.sfxVolume = value;
                break;
        }
        SaveOptionDataSO();
    }
    #endregion

    #region SaveLoad
    private void SaveOptionDataSO()
    {
        string json = JsonUtility.ToJson(optionData);
        PlayerPrefs.SetString(OPTION_DATA_NAME, json);
        PlayerPrefs.Save();
    }

    private void LoadOptionDataSO()
    {
        string json = PlayerPrefs.GetString(OPTION_DATA_NAME, string.Empty);
        if (string.IsNullOrEmpty(json))
        {
            optionData = new()
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
            optionData = JsonUtility.FromJson<OptionData>(json);
        }
    }
    #endregion
}

[Serializable]
public struct OptionData
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