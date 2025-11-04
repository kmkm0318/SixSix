using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    private const string PLAYER_DATA = "PlayerData";
    private const string ACHIEVED_PLAYER_STAT_ID = "AchievedPlayerStatID";

    public PlayerData PlayerData { get; private set; }

    public event Action<int> OnChipChanged;

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void AddChip(int amount)
    {
        int maxValue = (int)1e9;

        amount = Mathf.Clamp(amount, 0, maxValue);
        int preAmount = Mathf.Clamp(PlayerData.chip, 0, maxValue);
        int nextAmount = Mathf.Clamp(preAmount + amount, 0, maxValue);

        PlayerData.chip = nextAmount;
        OnChipChanged?.Invoke(PlayerData.chip);

        SaveData();
    }

    public bool TrySubtractChip(int amount)
    {
        int maxValue = (int)1e9;

        amount = Mathf.Clamp(amount, 0, maxValue);
        int preAmount = Mathf.Clamp(PlayerData.chip, 0, maxValue);

        if (preAmount < amount) return false;

        int nextAmount = Mathf.Clamp(preAmount - amount, 0, maxValue);
        PlayerData.chip = nextAmount;
        OnChipChanged?.Invoke(PlayerData.chip);

        SaveData();
        return true;
    }

    public bool TryAddAchievedPlayerStatIDs(int id)
    {
        if (!PlayerData.achievedPlayerStatIDs.Contains(id))
        {
            PlayerData.achievedPlayerStatIDs.Add(id);
            SaveData();
            return true;
        }

        return false;
    }

    #region Save, Load
    private void SaveData()
    {
        string playerDataJson = JsonUtility.ToJson(PlayerData);
        string achievedPlayerStatIDJson = JsonUtility.ToJson(new IntListWrapper()
        {
            ints = PlayerData.achievedPlayerStatIDs
        });

        PlayerPrefs.SetString(PLAYER_DATA, playerDataJson);
        PlayerPrefs.SetString(ACHIEVED_PLAYER_STAT_ID, achievedPlayerStatIDJson);

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string playerDataJson = PlayerPrefs.GetString(PLAYER_DATA, string.Empty);
        string achievedPlayerStatIDJson = PlayerPrefs.GetString(ACHIEVED_PLAYER_STAT_ID, string.Empty);

        if (string.IsNullOrEmpty(playerDataJson))
        {
            PlayerData = new()
            {
                chip = 0,
                achievedPlayerStatIDs = new() { 0 }
            };
        }
        else
        {
            PlayerData = JsonUtility.FromJson<PlayerData>(playerDataJson);

            if (string.IsNullOrEmpty(achievedPlayerStatIDJson))
            {
                PlayerData.achievedPlayerStatIDs = new() { 0 };
            }
            else
            {
                var intListWrapper = JsonUtility.FromJson<IntListWrapper>(achievedPlayerStatIDJson);
                PlayerData.achievedPlayerStatIDs = intListWrapper.ints;
            }
        }
    }
    #endregion
}

[Serializable]
public class PlayerData
{
    public int chip;
    public List<int> achievedPlayerStatIDs;
}