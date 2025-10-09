using UnityEngine;

[CreateAssetMenu(fileName = "AbilityTriggerLastPlaySO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerLastPlaySO")]
public class AbilityTriggerLastPlaySO : AbilityTriggerSO
{
    [SerializeField] private bool _isBeforeDecreasePlayCount = true;

    public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
    {
        if (triggerType != TriggerType) return false;

        return PlayManager.Instance.PlayRemain == (_isBeforeDecreasePlayCount ? 1 : 0);
    }
}