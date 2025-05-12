using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class BonusManager : Singleton<BonusManager>
{
    [SerializeField] private List<BonusTypeScorePair> bonusTargetScoreList;

    public event Action<BonusType> OnBonusAchieved;
    public event Action OnAllBonusAchieved;
    public event Action<int, int> OnDiceSumChanged;
    public event Action<BonusType, int> OnTotalDiceSumChanged;

    private Dictionary<BonusType, int> bonusTargetScoreDict = new();
    public ReadOnlyDictionary<BonusType, int> BonusTargetScoreDict => new(bonusTargetScoreDict);
    private Dictionary<int, int> diceSumDict = new();
    private Dictionary<BonusType, bool> bonusAchievedDict = new();
    private int totalDiceSum = 0;

    protected override void Awake()
    {
        base.Awake();

        foreach (var bonusTargetScore in bonusTargetScoreList)
        {
            bonusTargetScoreDict[bonusTargetScore.type] = bonusTargetScore.score;
            bonusAchievedDict[bonusTargetScore.type] = false;
        }

        for (int i = 1; i <= 6; i++)
        {
            diceSumDict[i] = 0;
        }
    }

    private void Start()
    {
        if (DataContainer.Instance.DefaultPlayDiceCount >= 6)
        {
            HandScoreUI.Instance.ScrollLayoutPanel(true);
        }
        else
        {
            RegisterEvents();
        }
    }

    private void RegisterEvents()
    {
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnCurrentRoundScoreUpdated(float score)
    {
        var diceValueList = DiceManager.Instance.GetOrderedPlayDiceValues();
        Dictionary<int, int> currentDiceSumDict = new();

        bool isUpdated = false;
        foreach (var value in diceValueList)
        {
            if (currentDiceSumDict.ContainsKey(value))
            {
                currentDiceSumDict[value] += value;
            }
            else
            {
                currentDiceSumDict[value] = value;
            }
        }

        foreach (var pair in currentDiceSumDict)
        {
            if (diceSumDict.TryGetValue(pair.Key, out var sum))
            {
                if (sum < pair.Value)
                {
                    diceSumDict[pair.Key] = pair.Value;
                    OnDiceSumChanged?.Invoke(pair.Key, pair.Value);
                    isUpdated = true;
                }
            }
        }

        if (isUpdated)
        {
            UpdateTotalDiceSum();
        }
    }

    private void UpdateTotalDiceSum()
    {
        totalDiceSum = diceSumDict.Values.Sum();
        List<BonusType> achievedBonusList = new();

        bool isChanged = false;
        foreach (var bonusAchievedPair in bonusAchievedDict)
        {
            if (bonusAchievedPair.Value) continue;
            isChanged = true;

            OnTotalDiceSumChanged?.Invoke(bonusAchievedPair.Key, totalDiceSum);

            if (bonusTargetScoreDict.TryGetValue(bonusAchievedPair.Key, out var targetScore))
            {
                if (totalDiceSum >= targetScore)
                {
                    achievedBonusList.Add(bonusAchievedPair.Key);
                }
            }
        }

        if (isChanged)
        {
            SequenceManager.Instance.ApplyParallelCoroutine();
        }

        if (achievedBonusList.Count > 0)
        {
            foreach (var achievedBonus in achievedBonusList)
            {
                bonusAchievedDict[achievedBonus] = true;
                OnBonusAchieved?.Invoke(achievedBonus);
            }
            SequenceManager.Instance.ApplyParallelCoroutine();
        }

        if (achievedBonusList.Count > 0 && bonusAchievedDict.All(x => x.Value))
        {
            OnAllBonusAchieved?.Invoke();
        }
    }
}

[Serializable]
public enum BonusType
{
    Money,
    AvailityDiceCountMax,
    PlayDice,
    PlayMax,
    RollMax,
}

[Serializable]
public struct BonusTypeScorePair
{
    public BonusType type;
    public int score;

    public BonusTypeScorePair(BonusType type, int score)
    {
        this.type = type;
        this.score = score;
    }
}
