using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerEventSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerEventSO")]
public class AvailityTriggerEventSO : AvailityTriggerSO
{
    public override bool IsTriggered(EffectTriggerType triggerType, AvailityDiceContext context)
    {
        return triggerType == TriggerType;
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        string res = "When " + SplitString(TriggerType.ToString());

        return res;
    }

    private string SplitString(string str)
    {
        StringBuilder sb = new();
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]) && i > 0)
            {
                sb.Append(" ");
            }
            sb.Append(str[i]);
        }
        return sb.ToString();
    }
}