using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerAllBigSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerAllBigSO")]
public class AbilityTriggerAllBigSO : AbilityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var diceList = DiceManager.Instance.PlayDiceList;

        foreach (var dice in diceList)
        {
            if (dice == null || dice.DiceValue <= 3)
            {
                return false;
            }
        }

        return true;
    }
}