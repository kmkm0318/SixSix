using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDiceUnlockNoneSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockNoneSO", order = 0)]
public class AbilityDiceUnlockNoneSO : AbilityDiceUnlockSO
{
    public override bool IsUnlocked()
    {
        return false;
    }
}