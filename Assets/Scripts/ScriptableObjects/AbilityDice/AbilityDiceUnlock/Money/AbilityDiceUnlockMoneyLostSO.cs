using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockMoneyLostSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockMoneyLostSO", order = 0)]
public class AbilityDiceUnlockMoneyLostSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var moneyLost = PlayerRecordManager.Instance.PlayerRecordData.moneyLost;

        return moneyLost >= value;
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