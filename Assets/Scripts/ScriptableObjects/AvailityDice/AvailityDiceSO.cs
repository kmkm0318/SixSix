using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityDiceSO", menuName = "Scriptable Objects/AvailityDiceSO")]
public class AvailityDiceSO : ScriptableObject
{
    [Header("Dice Info")]
    public string diceName;
    public int price;
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

    #region GetDescriptionText
    public string GetDescriptionText()
    {
        return GetTriggerTypeText() + GetEffectTypeText();
    }
    private string GetTriggerTypeText()
    {
        return availityTriggerType switch
        {
            AvailityTriggerType.OnPlayDiceApplied => GetPlayDiceTriggerText(),
            AvailityTriggerType.OnHandCategoryApplied => GetHandCategoryTriggerText(),
            _ => "Unknown Trigger Type"
        };
    }

    private string GetPlayDiceTriggerText()
    {
        string listString = "";

        foreach (int value in targetPlayDiceValueList)
        {
            listString += value.ToString() + ", ";
        }
        if (listString.Length > 0)
        {
            listString = listString[..^2];
        }

        return $"When Dice Value is ({listString})";
    }

    private string GetHandCategoryTriggerText()
    {
        return $"When Play with ({targetHandCategorySO.handCategoryName})";
    }

    private string GetEffectTypeText()
    {
        return availityEffectType switch
        {
            AvailityEffectType.ApplyScorePair => GetScorePairEffectText(),
            AvailityEffectType.AchieveMoney => GetMoneyEffectText(),
            _ => "Unknown Effect Type"
        };
    }

    private string GetScorePairEffectText()
    {
        string res = "Score";

        if (scorePairAmount.baseScore != 0)
        {
            res += $"(+{scorePairAmount.baseScore})";
        }
        else if (scorePairAmount.multiplier != 0)
        {
            res += $"(x{scorePairAmount.multiplier})";
        }
        else
        {
            res += "(0)";
        }

        if (availityDiceValueCalculationType == AvailityDiceValueCalculationType.Multiply)
        {
            res += "x";
        }
        else if (availityDiceValueCalculationType == AvailityDiceValueCalculationType.Power)
        {
            res += "^";
        }

        res += $"(1~{maxFaceValue})";

        return res;
    }

    private string GetMoneyEffectText()
    {
        string res = "Get Money";

        res += $"({moneyAmount})";

        if (availityDiceValueCalculationType == AvailityDiceValueCalculationType.Multiply)
        {
            res += "x";
        }
        else if (availityDiceValueCalculationType == AvailityDiceValueCalculationType.Power)
        {
            res += "^";
        }
        res += $"(1~{maxFaceValue})";

        return res;
    }
    #endregion
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