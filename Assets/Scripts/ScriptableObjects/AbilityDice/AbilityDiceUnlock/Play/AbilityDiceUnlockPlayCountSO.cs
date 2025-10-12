using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockPlayCountSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockPlayCountSO", order = 0)]
public class AbilityDiceUnlockPlayCountSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var playCount = PlayerRecordManager.Instance.PlayerRecordData.playCount;

        return playCount >= value;
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