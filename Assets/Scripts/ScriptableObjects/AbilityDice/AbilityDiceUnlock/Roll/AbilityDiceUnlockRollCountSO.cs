using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockRollCountSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockRollCountSO", order = 0)]
public class AbilityDiceUnlockRollCountSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var rollCount = PlayerRecordManager.Instance.PlayerRecordData.rollCount;

        return rollCount >= value;
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