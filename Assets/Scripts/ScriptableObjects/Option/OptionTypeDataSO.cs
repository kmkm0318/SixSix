using UnityEngine;

[CreateAssetMenu(fileName = "OptionTypeDataSO", menuName = "Scriptable Objects/OptionTypeDataSO")]
public class OptionTypeDataSO : ScriptableObject
{
    public OptionType optionType;
    public string optionName;
    public string[] optionValues;
}

public enum OptionType
{
    GameSpeed,
    AvailityDiceAutoKeep,
    Language,
    Fullscreen,
    Resolution,
    BGMVolume,
    SFXVolume,
}