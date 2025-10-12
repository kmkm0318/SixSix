using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerRecordManager : Singleton<PlayerRecordManager>
{
    private const string PLAYER_RECORD = "PlayerRecord";

    public PlayerRecordData PlayerRecordData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void UpdatePlayerRecord()
    {
        double maxScore = ScoreManager.Instance.HighestRoundScore;
        int maxClearedRound = GameResultManager.Instance.GetResultValue(GameResultValueType.ClearRound);
        int roundCount = GameResultManager.Instance.GetResultValue(GameResultValueType.ClearRound);
        int playCount = GameResultManager.Instance.GetResultValue(GameResultValueType.PlayCount);
        int rollCount = GameResultManager.Instance.GetResultValue(GameResultValueType.RollCount);
        int rerollCount = GameResultManager.Instance.GetResultValue(GameResultValueType.RerollCount);
        int moneyGained = GameResultManager.Instance.GetResultValue(GameResultValueType.MoneyGained);
        int moneyLost = GameResultManager.Instance.GetResultValue(GameResultValueType.MoneyLost);

        PlayerRecordData.maxScore = Math.Max(PlayerRecordData.maxScore, maxScore);
        PlayerRecordData.maxClearedRound = Mathf.Max(PlayerRecordData.maxClearedRound, maxClearedRound);
        PlayerRecordData.roundCount = Mathf.Clamp(PlayerRecordData.roundCount + roundCount, 0, (int)1e9);
        PlayerRecordData.playCount = Mathf.Clamp(PlayerRecordData.playCount + playCount, 0, (int)1e9);
        PlayerRecordData.rollCount = Mathf.Clamp(PlayerRecordData.rollCount + rollCount, 0, (int)1e9);
        PlayerRecordData.rerollCount = Mathf.Clamp(PlayerRecordData.rerollCount + rerollCount, 0, (int)1e9);
        PlayerRecordData.moneyGained = Mathf.Clamp(PlayerRecordData.moneyGained + moneyGained, 0, (int)1e9);
        PlayerRecordData.moneyLost = Mathf.Clamp(PlayerRecordData.moneyLost + moneyLost, 0, (int)1e9);

        SaveData();
    }

    #region Save, Load
    private void SaveData()
    {
        string json = JsonUtility.ToJson(PlayerRecordData);
        PlayerPrefs.SetString(PLAYER_RECORD, json);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(PLAYER_RECORD, string.Empty);

        if (string.IsNullOrEmpty(json))
        {
            PlayerRecordData = new()
            {
                unlockedAbilityDiceIDList = new(),
                maxScore = 0.0f,
                maxClearedRound = 0,
                roundCount = 0,
                playCount = 0,
                rollCount = 0,
                rerollCount = 0,
                moneyGained = 0,
                moneyLost = 0,
            };
        }
        else
        {
            PlayerRecordData = JsonUtility.FromJson<PlayerRecordData>(json);
        }
    }
    #endregion
}

[Serializable]
public class PlayerRecordData
{
    public List<int> unlockedAbilityDiceIDList;
    public double maxScore;
    public int maxClearedRound;
    public int roundCount;
    public int playCount;
    public int rollCount;
    public int rerollCount;
    public int moneyGained;
    public int moneyLost;
}