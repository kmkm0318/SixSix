using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceSO", menuName = "Scriptable Objects/AvailityDiceSO")]
public class AvailityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public int maxFaceValue;
    public DiceFaceSpriteListSO diceFaceSpriteListSO;

    [Header("Dice Trigger")]
    public AvailityTriggerType availityTriggerType;
    public List<int> targetPlayDiceValueList;
    public HandCategorySO targetHandCategorySO;

    [Header("Dice Effect")]
    public AvailityEffectType availityEffectType;
    public AvailityDiceValueCalculationType availityDiceValueCalculationType;
    public ScorePair scorePairAmount;
    public int moneyAmount;
}

public enum AvailityTriggerType
{
    OnPlayDiceApplied,
    OnHandCategoryApplied,
}

public enum AvailityEffectType
{
    ApplyScorePair,
    AchieveMoney,
}

public enum AvailityDiceValueCalculationType
{
    Multiply,
    Power,
}