using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class BonusManager : Singleton<BonusManager>
{
    [SerializeField] private List<BonusTypeScorePair> bonusTargetScoreList;

    private Dictionary<BonusType, int> bonusTargetScoreDict = new();
    private Dictionary<int, int> diceSumDict = new();
    private Dictionary<BonusType, bool> bonusAchievedDict = new();
    private List<BonusType> achievedBonusList = new();
    private int totalDiceSum = 0;

    public ReadOnlyDictionary<BonusType, int> BonusTargetScoreDict => new(bonusTargetScoreDict);

    public event Action<BonusType> OnBonusAchieved;
    public event Action OnAllBonusAchieved;
    public event Action<int, int> OnDiceSumChanged;
    public event Action<BonusType, int> OnTotalDiceSumChanged;

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
        if (DataContainer.Instance.CurrentDiceStat.defaultPlayDiceCount >= 6)
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
        GameManager.Instance.RegisterEvent(GameState.Play, null, OnPlayEnded);
    }

    private void OnPlayEnded()
    {
        UpdateBonusResults();
        HandleAchievedBonusList();
    }

    #region UpdateBonusResults
    private void UpdateBonusResults()
    {
        var diceValueList = DiceManager.Instance.GetOrderedPlayDiceValues();

        if (CheckShouldUpdate(diceValueList))
        {
            UpdateTotalDiceSum();
        }
    }

    private bool CheckShouldUpdate(List<int> diceValueList)
    {
        bool isUpdated = false;
        Dictionary<int, int> currentDiceSumDict = new();

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

        return isUpdated;
    }

    private void UpdateTotalDiceSum()
    {
        List<BonusType> currentAchievedBonusList = GetCurrentAchievedBonusList();
        if (currentAchievedBonusList.Count == 0) return;

        foreach (var achievedBonus in currentAchievedBonusList)
        {
            if (bonusAchievedDict[achievedBonus]) continue;
            achievedBonusList.Add(achievedBonus);
        }
    }

    private List<BonusType> GetCurrentAchievedBonusList()
    {
        List<BonusType> currentAchievedBonusList = new();

        totalDiceSum = diceSumDict.Values.Sum();

        foreach (var bonusAchievedPair in bonusAchievedDict)
        {
            if (bonusAchievedPair.Value) continue;

            OnTotalDiceSumChanged?.Invoke(bonusAchievedPair.Key, totalDiceSum);

            if (bonusTargetScoreDict.TryGetValue(bonusAchievedPair.Key, out var targetScore))
            {
                if (totalDiceSum >= targetScore)
                {
                    currentAchievedBonusList.Add(bonusAchievedPair.Key);
                }
            }
        }

        return currentAchievedBonusList;
    }

    #endregion
    #region HandleAchievedBonusList
    private void HandleAchievedBonusList()
    {
        foreach (var bonusAchieved in achievedBonusList)
        {
            if (bonusAchievedDict.TryGetValue(bonusAchieved, out var isAchieved) && !isAchieved)
            {
                HandleAchievedBonus(bonusAchieved);
            }
        }

        achievedBonusList.Clear();
    }

    private void HandleAchievedBonus(BonusType type)
    {
        OnBonusAchieved?.Invoke(type);
        SequenceManager.Instance.ApplyParallelCoroutine();

        switch (type)
        {
            case BonusType.Money:
                MoneyManager.Instance.AddMoney(MoneyManager.Instance.BonusMoney, true);
                break;
            case BonusType.AbilityDiceCountMax:
                DiceManager.Instance.IncreaseCurrentAbilityDiceMax();
                break;
            case BonusType.PlayDice:
                DiceManager.Instance.StartAddBonusPlayDice();
                break;
            case BonusType.PlayMax:
                PlayManager.Instance.IncreasePlayMaxAndRemain();
                break;
            case BonusType.RollMax:
                RollManager.Instance.IncreaseRollMaxAndRemain();
                break;
        }
        SequenceManager.Instance.ApplyParallelCoroutine();

        bonusAchievedDict[type] = true;

        if (bonusAchievedDict.All(x => x.Value))
        {
            OnAllBonusAchieved?.Invoke();
        }
    }
    #endregion
}

[Serializable]
public enum BonusType
{
    Money,
    AbilityDiceCountMax,
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
