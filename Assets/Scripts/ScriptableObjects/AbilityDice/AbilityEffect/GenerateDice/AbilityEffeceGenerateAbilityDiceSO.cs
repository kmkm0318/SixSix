using UnityEngine;

[CreateAssetMenu(fileName = "AbilityEffeceGenerateAbilityDiceSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffeceGenerateAbilityDiceSO")]
public class AbilityEffeceGenerateAbilityDiceSO : AbilityEffectSO
{
    public override void TriggerEffect(AbilityDiceContext context)
    {
        TriggerAnimationManager.Instance.PlayTriggerAnimation(context.currentAbilityDice.transform);
        SequenceManager.Instance.ApplyParallelCoroutine();
        DiceManager.Instance.StartGenerateRandomNormalAbilityDice(context.currentAbilityDice.DiceValue);
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