using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerMoreLessSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerMoreLessSO")]
public class AbilityTriggerMoreLessSO : AbilityTriggerSO
{
    [SerializeField] private bool _isMore = true;
    [SerializeField] private int _targetValue = 25;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        var diceList = DiceManager.Instance.PlayDiceList;

        int sum = 0;
        foreach (var dice in diceList)
        {
            sum += dice.DiceValue;
        }

        return _isMore ? (sum >= _targetValue) : (sum <= _targetValue);
    }
}