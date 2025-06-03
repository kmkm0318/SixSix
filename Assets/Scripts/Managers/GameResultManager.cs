public class GameResultManager : Singleton<GameResultManager>
{
    private int playCount = 0;
    private int rollCount = 0;
    private int moneyGained = 0;
    private int moneyLost = 0;
    private int rerollCount = 0;

    public int ClearRound => RoundManager.Instance.CurrentRound - 1;
    public bool IsClear { get; set; } = false;

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, null, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, null, OnRollEnded);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceEnded);
        MoneyManager.Instance.OnMoneyAdded += OnMoneyAdded;
        MoneyManager.Instance.OnMoneyRemoved += OnMoneyRemoved;
        ShopManager.Instance.OnRerollCompleted += OnRerollCompleted;
    }

    private void OnPlayEnded()
    {
        playCount++;
    }

    private void OnRollEnded()
    {
        rollCount++;
    }

    private void OnEnhanceStarted()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            GameManager.Instance.UnregisterEvent(GameState.Play, null, OnPlayEnded);
        }
        else
        {
            GameManager.Instance.UnregisterEvent(GameState.Roll, null, OnRollEnded);
        }
    }

    private void OnEnhanceEnded()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            GameManager.Instance.RegisterEvent(GameState.Play, null, OnPlayEnded);
        }
        else
        {
            GameManager.Instance.RegisterEvent(GameState.Roll, null, OnRollEnded);
        }
    }

    private void OnMoneyAdded(int obj)
    {
        moneyGained += obj;
    }

    private void OnMoneyRemoved(int obj)
    {
        moneyLost += obj;
    }

    private void OnRerollCompleted()
    {
        rerollCount++;
    }
    #endregion

    public string GetResultValue(GameResultValueType type)
    {
        return type switch
        {
            GameResultValueType.HighestRoundScore => UtilityFunctions.FormatNumber(ScoreManager.Instance.HighestRoundScore),
            GameResultValueType.MostPlayedHand => GetMostPlayedHandString(),
            GameResultValueType.ClearRound => ClearRound.ToString(),
            GameResultValueType.PlayCount => playCount.ToString(),
            GameResultValueType.RollCount => rollCount.ToString(),
            GameResultValueType.MoneyGained => "$" + moneyGained.ToString(),
            GameResultValueType.MoneyLost => "$" + moneyLost.ToString(),
            GameResultValueType.RerollCount => rerollCount.ToString(),
            _ => "0"
        };
    }

    private string GetMostPlayedHandString()
    {
        HandManager.Instance.GetMostPlayedHand(out var hand, out var playCount);
        string handName = DataContainer.Instance.GetHandSO(hand).HandName;
        return $"{handName}<size=75%><color=#888888>({playCount})</color></size>";
    }
}

public enum GameResultValueType
{
    HighestRoundScore,
    MostPlayedHand,
    ClearRound,
    PlayCount,
    RollCount,
    MoneyGained,
    MoneyLost,
    RerollCount
}