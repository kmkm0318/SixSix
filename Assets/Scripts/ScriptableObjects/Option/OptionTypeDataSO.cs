using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "OptionTypeDataSO", menuName = "Scriptable Objects/OptionTypeDataSO")]
public class OptionTypeDataSO : ScriptableObject
{
    public OptionType optionType;
    public bool isLocalizedName;
    public string optionName;
    public LocalizedString optionNameLocalized;
    public bool isLocalizedValue;
    public string[] optionValues;
    public LocalizedString[] optionValuesLocalized;
}

public enum OptionType
{
    GameSpeed,
    AbilityDiceAutoKeep,
    Language,
    Fullscreen,
    Resolution,
    BGMVolume,
    SFXVolume,
}