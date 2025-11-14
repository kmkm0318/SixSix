using System;

/// <summary>
/// 다이스 상호작용 이벤트를 관리하는 정적 클래스
/// </summary>
public static class DiceInteractionEvents
{
    public static Action<Dice> OnDiceClicked;

    public static void TriggerOnDiceClicked(Dice dice) => OnDiceClicked?.Invoke(dice);

}