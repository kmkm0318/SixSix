using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockRoundCountSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockRoundCountSO", order = 0)]
public class AbilityDiceUnlockRoundCountSO : AbilityDiceUnlockSO
{
    [SerializeField] private int value = 100;

    public override bool IsUnlocked()
    {
        var roundCount = PlayerRecordManager.Instance.PlayerRecordData.roundCount;

        return roundCount >= value;
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