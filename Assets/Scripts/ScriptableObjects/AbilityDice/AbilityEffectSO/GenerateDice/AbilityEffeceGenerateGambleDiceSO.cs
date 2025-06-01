using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffeceGenerateGambleDiceSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffeceGenerateGambleDiceSO")]
public class AbilityEffeceGenerateGambleDiceSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.currentAbilityDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
        DiceManager.Instance.StartGenerateGambleDice(context.currentAbilityDice.DiceValue);
    }

    public override string GetEffectDescription(AbilityDiceSO abilityDiceSO)
    {
        if (effectDescription == null)
        {
            Debug.LogError("Effect description is not set for AbilityEffectMoneySO.");
            return string.Empty;
        }
        effectDescription.Arguments = new object[] { DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
        effectDescription.RefreshString();
        return effectDescription.GetLocalizedString();
    }
}