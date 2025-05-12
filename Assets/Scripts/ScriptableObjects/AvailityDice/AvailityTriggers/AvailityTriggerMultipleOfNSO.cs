using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerMultipleOfNSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerMultipleOfNSO")]
public class AvailityTriggerMultipleOfNSO : AvailityTriggerSO
{
    [SerializeField] private int multipleOfN = 7;

    public override bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var playDiceList = DiceManager.Instance.PlayDiceList;
        int sum = 0;
        foreach (var playDice in playDiceList)
        {
            sum += playDice.DiceValue;
        }
        return sum % multipleOfN == 0;
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        return "When the sum of play dice\nis a multiple of " + multipleOfN;
    }
}