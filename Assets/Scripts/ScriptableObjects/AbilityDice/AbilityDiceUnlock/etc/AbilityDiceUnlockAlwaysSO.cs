using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockAlwaysSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockAlwaysSO", order = 0)]
public class AbilityDiceUnlockAlwaysSO : AbilityDiceUnlockSO
{
    public override bool IsUnlocked()
    {
        return true;
    }
}