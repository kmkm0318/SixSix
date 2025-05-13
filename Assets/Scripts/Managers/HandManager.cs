using System;
using System.Collections.Generic;

public class HandManager : Singleton<HandManager>
{
    private Dictionary<Hand, int> handSelectionCounts = new();
    private Dictionary<Hand, int> handEnhanceLevels = new();
    private Dictionary<Hand, bool> handAvailabilities = new();
    private Dictionary<Hand, ScorePair> handScores = new();
    private HandSO lastSelectedHandSO = null;
    private bool isActive = false;

    public Dictionary<Hand, ScorePair> HandScores => handScores;
    public List<Hand> UsableHands { get; set; } = null;
    public HandSO LastSelectedHandSO => lastSelectedHandSO;

    public event Action<HandSO> OnHandSelected;
    public event Action<ScorePair> OnHandScoreApplied;

    private void Start()
    {
        RegisterEvents();
        Init();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;

        PlayManager.Instance.OnPlayStarted += OnPlayStarted;

        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += OnDiceEnhanceCompleted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnHandEnhanceCompleted;
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollCompleted()
    {
        UpdateHands();
        isActive = true;
    }

    private void OnPlayStarted(int playRemain)
    {
        isActive = false;
        HandScoreUI.Instance.ResetHandScoreUI();
    }

    private void OnDiceEnhanceStarted()
    {
        isActive = false;
    }

    private void OnDiceEnhanceCompleted()
    {
        isActive = false;
    }

    private void OnHandEnhanceStarted()
    {
        isActive = true;
    }

    private void OnHandEnhanceCompleted()
    {
        isActive = false;
    }
    #endregion

    private void UpdateHands()
    {
        var playDiceValues = DiceManager.Instance.GetOrderedPlayDiceValues();
        handAvailabilities = HandCalculator.GetHandCheckResults(playDiceValues);

        var handScoreDict = new Dictionary<Hand, ScorePair>();
        foreach (var hand in handAvailabilities)
        {
            if (hand.Value)
            {
                handScoreDict[hand.Key] = HandScores[hand.Key];
            }
            else
            {
                handScoreDict[hand.Key] = new(0, 0);
            }
        }

        HandScoreUI.Instance.UpdateHandScores(handScoreDict);
    }

    private void Init()
    {
        foreach (var handSO in DataContainer.Instance.TotalHandListSO.handList)
        {
            handEnhanceLevels[handSO.hand] = 0;
            handSelectionCounts[handSO.hand] = 0;
            handAvailabilities[handSO.hand] = false;
            handScores[handSO.hand] = handSO.scorePair;
        }
    }

    public void HandleSelectHand(HandSO handSO)
    {
        if (!handAvailabilities.TryGetValue(handSO.hand, out var isAvailiable) || !isAvailiable) return;
        if (UsableHands != null && !UsableHands.Contains(handSO.hand)) return;
        if (!isActive) return;
        isActive = false;

        lastSelectedHandSO = handSO;

        if (handSelectionCounts.TryGetValue(handSO.hand, out int count))
        {
            handSelectionCounts[handSO.hand] = count + 1;
        }
        else
        {
            handSelectionCounts[handSO.hand] = 1;
        }
        OnHandSelected?.Invoke(handSO);

        if (handScores.TryGetValue(handSO.hand, out var scorePair))
        {
            OnHandScoreApplied?.Invoke(scorePair);
        }
        else
        {
            handScores[handSO.hand] = handSO.scorePair;
        }
    }

    public void EnhanceHand(HandSO handSO, int enhanceLevel)
    {
        if (handEnhanceLevels.TryGetValue(handSO.hand, out int level))
        {
            handEnhanceLevels[handSO.hand] = level + enhanceLevel;
            handScores[handSO.hand] = handSO.GetEnhancedScorePair(level + enhanceLevel);
        }
        else
        {
            handEnhanceLevels[handSO.hand] = enhanceLevel;
            handScores[handSO.hand] = handSO.GetEnhancedScorePair(enhanceLevel);
        }

        HandScoreUI.Instance.PlayHandTriggerAnimation(handSO, handEnhanceLevels[handSO.hand], handScores[handSO.hand]);
    }
}