using Febucci.UI.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockMaxScoreSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockMaxScoreSO", order = 0)]
public class AbilityDiceUnlockMaxScoreSO : AbilityDiceUnlockSO
{
    [SerializeField] private double value = 1e6;

    public override bool IsUnlocked()
    {
        var maxScore = PlayerRecordManager.Instance.PlayerRecordData.maxScore;

        return maxScore >= value;
    }

    public override string GetDescriptionText()
    {
        if (unlockDescription == null)
        {
            Debug.LogError($"There is no Description for Ability Unlock : {typeof(AbilityDiceUnlockMoneyGainedSO)}");
            return string.Empty;
        }

        unlockDescription.Arguments = new object[] { UtilityFunctions.FormatNumber(value) };
        unlockDescription.RefreshString();
        return unlockDescription.GetLocalizedString();
    }
}