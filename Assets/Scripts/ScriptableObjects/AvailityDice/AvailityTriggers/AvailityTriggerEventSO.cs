using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "AvailityTriggerEventSO", menuName = "Scriptable Objects/AvailityTriggers/AvailityTriggerEventSO")]
public class AvailityTriggerEventSO : AvailityTriggerSO
{
    public override bool IsTriggered(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        return triggerType == TriggerType;
    }

    public override string GetTriggerDescription(AvailityDiceSO availityDiceSO)
    {
        string res = "When ";

        switch (TriggerType)
        {
            case AvailityTriggerType.RoundStarted:
                res += "Round Started";
                break;
            case AvailityTriggerType.RoundCleared:
                res += "Round Cleared";
                break;
            case AvailityTriggerType.ShopStarted:
                res += "Shop Started";
                break;
            case AvailityTriggerType.ShopEnded:
                res += "Shop Ended";
                break;
            default:
                res = "Unknown Event";
                break;
        }

        return res;
    }
}