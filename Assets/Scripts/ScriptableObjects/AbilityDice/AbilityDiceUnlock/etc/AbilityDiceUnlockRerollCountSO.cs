using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockRerollCountSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockRerollCountSO", order = 0)]
public class AbilityDiceUnlockRerollCountSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var rerollCount = PlayerRecordManager.Instance.PlayerRecordData.rerollCount;

        return rerollCount >= value;
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