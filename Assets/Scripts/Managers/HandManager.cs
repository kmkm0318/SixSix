using System;
using System.Collections.Generic;

public class HandManager : Singleton<HandManager>
{
    private Dictionary<Hand, int> handSelectionCounts = new();
    private Dictionary<Hand, int> handEnhanceLevels = new();
    private Dictionary<Hand, bool> handCompletions = new();
    private Dictionary<Hand, ScorePair> handScores = new();
    private HandSO lastSelectedHandSO = null;
    private bool isActive = false;

    public Dictionary<Hand, ScorePair> HandScores => handScores;
    public List<Hand> UsableHands { get; set; } = null;
    public List<Hand> CompletedHands
    {
        get
        {
            var completedHands = new List<Hand>();
            foreach (var hand in handCompletions)
            {
                if (hand.Value)
                {
                    completedHands.Add(hand.Key);
                }
            }
            return completedHands;
        }
    }
    public HandSO LastSelectedHandSO => lastSelectedHandSO;
    public bool IsSameHand { get; private set; } = false;

    public event Action<HandSO> OnEnhanceHandSelected;
    public event Action<ScorePair> OnHandScoreApplied;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        foreach (var handSO in DataContainer.Instance.TotalHandListSO.handList)
        {
            handEnhanceLevels[handSO.hand] = 0;
            handSelectionCounts[handSO.hand] = 0;
            handCompletions[handSO.hand] = false;
            handScores[handSO.hand] = handSO.scorePair;
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceEnded);
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollCompleted()
    {
        isActive = true;
    }

    private void OnPlayStarted()
    {
        isActive = false;

        HandScoreUIEvents.TriggerOnHandScoreUIResetRequested();
    }

    private void OnEnhanceStarted()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted);
            isActive = true;
        }
        else
        {
            GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted);
            GameManager.Instance.UnregisterEvent(GameState.Roll, null, OnRollCompleted);
        }
    }

    private void OnEnhanceEnded()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted);
        }
        else
        {
            GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted);
            GameManager.Instance.RegisterEvent(GameState.Roll, null, OnRollCompleted);
        }
    }
    #endregion

    public void UpdateHands()
    {
        var playDiceValues = DiceManager.Instance.GetOrderedPlayDiceValues();
        handCompletions = HandCalculator.GetHandCheckResults(playDiceValues);

        var handScoreDict = new Dictionary<Hand, ScorePair>();
        foreach (var hand in handCompletions)
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

        HandScoreUIEvents.TriggerOnHandScoreUIUpdateRequested(handScoreDict);
    }

    public void HandleSelectHand(HandSO handSO)
    {
        if (!handCompletions.TryGetValue(handSO.hand, out var isAvailiable) || !isAvailiable) return;
        if (UsableHands != null && !UsableHands.Contains(handSO.hand)) return;
        if (!isActive) return;
        isActive = false;

        if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            OnEnhanceHandSelected?.Invoke(handSO);
            return;
        }

        IsSameHand = lastSelectedHandSO != null && lastSelectedHandSO.hand == handSO.hand;
        lastSelectedHandSO = handSO;

        if (handSelectionCounts.TryGetValue(handSO.hand, out int count))
        {
            handSelectionCounts[handSO.hand] = count + 1;
        }
        else
        {
            handSelectionCounts[handSO.hand] = 1;
        }

        QuestManager.Instance.TriggerActiveQuest(QuestTriggerType.HandPlay, handSO);

        if (!handScores.TryGetValue(handSO.hand, out var scorePair))
        {
            scorePair = handSO.scorePair;
            handScores[handSO.hand] = scorePair;
        }

        OnHandScoreApplied?.Invoke(scorePair);
    }

    public void EnhanceHand(Hand hand, int enhanceLevel)
    {
        var handSO = DataContainer.Instance.GetHandSO(hand);

        if (handEnhanceLevels.TryGetValue(hand, out int level))
        {
            handEnhanceLevels[hand] = level + enhanceLevel;
            handScores[hand] = handSO.GetEnhancedScorePair(level + enhanceLevel);
        }
        else
        {
            handEnhanceLevels[hand] = enhanceLevel;
            handScores[hand] = handSO.GetEnhancedScorePair(enhanceLevel);
        }

        HandScoreUIEvents.TriggerOnHandScoreUIPlayHandTriggerAnimationRequested(hand, handEnhanceLevels[hand], handScores[hand]);
    }

    public void GetMostPlayedHand(out Hand handName, out int count)
    {
        Hand mostPlayedHand = Hand.Choice;
        int maxCount = 0;

        foreach (var hand in handSelectionCounts)
        {
            if (hand.Value > maxCount)
            {
                maxCount = hand.Value;
                mostPlayedHand = hand.Key;
            }
        }

        handName = mostPlayedHand;
        count = maxCount;
    }
}