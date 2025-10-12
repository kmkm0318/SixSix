using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockMoneyGainedSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockMoneyGainedSO", order = 0)]
public class AbilityDiceUnlockMoneyGainedSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var moneyGained = PlayerRecordManager.Instance.PlayerRecordData.moneyGained;

        return moneyGained >= value;
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