using System.Collections;
using UnityEngine;

public class LoadingState : BaseGameState
{
    public override void Enter()
    {
        base.Enter();

        GameManager.Instance.StartCoroutine(DelayStartDiceGeneration(1f));
    }

    private IEnumerator DelayStartDiceGeneration(float duration)
    {
        yield return new WaitForSeconds(duration);
        DiceManager.Instance.StartFirstPlayDiceGenerate(() =>
        {
            GameManager.Instance.ChangeState(GameState.Round);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }
}