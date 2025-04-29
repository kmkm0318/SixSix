using UnityEngine;

[CreateAssetMenu(fileName = "BossRound_LimitHandSO", menuName = "Scriptable Objects/BossRounds/BossRound_LimitHandSO")]
public class BossRound_LimitHandSO : BossRoundSO
{
    [SerializeField] private HandSO targetHandSO;

    public override void OnEnter()
    {
        HandScoreUI.Instance.UsableHandSO = targetHandSO;
    }

    public override void OnExit()
    {
        HandScoreUI.Instance.UsableHandSO = null;
    }
}