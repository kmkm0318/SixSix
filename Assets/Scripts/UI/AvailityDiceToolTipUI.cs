using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AvailityDiceToolTipUI : Singleton<AvailityDiceToolTipUI>
{
    [SerializeField] private TMP_Text diceNameText;
    [SerializeField] private TMP_Text triggerTypeText;
    [SerializeField] private TMP_Text effectTypeText;
    [SerializeField] private Vector3 offset;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        HideToolTip();
    }

    public void ShowToolTip(AvailityDice availityDice)
    {
        if (availityDice == null || availityDice.AvailityDiceSO == null) return;
        AvailityDiceSO availityDiceSO = availityDice.AvailityDiceSO;

        gameObject.SetActive(true);

        var targetPos = Camera.main.WorldToScreenPoint(availityDice.transform.position + offset);
        rectTransform.position = targetPos;
        diceNameText.text = availityDiceSO.diceName;
        triggerTypeText.text = GetTriggerTypeText(availityDiceSO);
        effectTypeText.text = GetEffectTypeText(availityDiceSO);
    }

    #region GetText
    private string GetTriggerTypeText(AvailityDiceSO availityDiceSO)
    {
        return availityDiceSO.availityTriggerType switch
        {
            AvailityTriggerType.OnPlayDiceApplied => GetPlayDiceTriggerText(availityDiceSO.targetPlayDiceValueList),
            AvailityTriggerType.OnHandCategoryApplied => GetHandCategoryTriggerText(availityDiceSO),
            _ => "Unknown Trigger Type"
        };
    }

    private string GetPlayDiceTriggerText(List<int> playDiceValueList)
    {
        string listString = "";

        foreach (int value in playDiceValueList)
        {
            listString += value.ToString() + ", ";
        }
        if (listString.Length > 0)
        {
            listString = listString[..^2];
        }

        return $"When Dice Value is ({listString})";
    }

    private string GetHandCategoryTriggerText(AvailityDiceSO availityDiceSO)
    {
        return $"When Play with ({availityDiceSO.targetHandCategorySO.handCategoryName})";
    }

    private string GetEffectTypeText(AvailityDiceSO availityDiceSO)
    {
        return availityDiceSO.availityEffectType switch
        {
            AvailityEffectType.ApplyScorePair => GetScorePairEffectText(availityDiceSO),
            AvailityEffectType.AchieveMoney => GetMoneyEffectText(availityDiceSO),
            _ => "Unknown Effect Type"
        };
    }

    private string GetScorePairEffectText(AvailityDiceSO availityDiceSO)
    {
        string res = "Score";

        if (availityDiceSO.scorePairAmount.baseScore != 0)
        {
            res += $"(+{availityDiceSO.scorePairAmount.baseScore})";
        }
        else if (availityDiceSO.scorePairAmount.multiplier != 0)
        {
            res += $"(x{availityDiceSO.scorePairAmount.multiplier})";
        }
        else
        {
            res += "(0)";
        }

        if (availityDiceSO.availityDiceValueCalculationType == AvailityDiceValueCalculationType.Multiply)
        {
            res += "x";
        }
        else if (availityDiceSO.availityDiceValueCalculationType == AvailityDiceValueCalculationType.Power)
        {
            res += "^";
        }

        res += $"(1~{availityDiceSO.maxFaceValue})";

        return res;
    }

    private string GetMoneyEffectText(AvailityDiceSO availityDiceSO)
    {
        string res = "Get Money";

        res += $"({availityDiceSO.moneyAmount})";

        if (availityDiceSO.availityDiceValueCalculationType == AvailityDiceValueCalculationType.Multiply)
        {
            res += "x";
        }
        else if (availityDiceSO.availityDiceValueCalculationType == AvailityDiceValueCalculationType.Power)
        {
            res += "^";
        }
        res += $"(1~{availityDiceSO.maxFaceValue})";

        return res;
    }
    #endregion

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}