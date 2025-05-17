using UnityEngine;

[CreateAssetMenu(fileName = "GambleDiceSO", menuName = "Scriptable Objects/GambleDiceSO")]
public class GambleDiceSO : AvailityDiceSO
{
    public GambleEffectSO gambleEffectSO;

    public void TriggerEffect(GambleDice gambleDice)
    {
        gambleEffectSO.TriggerEffect(gambleDice);
    }

    public override string GetDescriptionText()
    {
        return gambleEffectSO.GetEffectDescription(this);
    }
}