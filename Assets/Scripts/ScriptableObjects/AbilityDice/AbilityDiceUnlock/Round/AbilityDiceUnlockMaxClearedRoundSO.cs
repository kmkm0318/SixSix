using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockMaxClearedRoundSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockMaxClearedRoundSO", order = 0)]
public class AbilityDiceUnlockMaxClearedRoundSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var maxClearedRound = PlayerRecordManager.Instance.PlayerRecordData.maxClearedRound;

        return maxClearedRound >= value;
    }

    public override string GetDescriptionText()
    {
        if (unlockDescription == null)
        {
            Debug.LogError($"There is no Description for Ability Unlock : {typeof(AbilityDiceUnlockMoneyGainedSO)}");
            return string.Empty;
        }

        unlockDescription.Arguments = new object[] { value };
        unlockDescription.RefreshString();
        return unlockDescription.GetLocalizedString();
    }
}