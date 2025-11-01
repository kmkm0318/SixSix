using System;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    private const string PLAYER_DATA = "PlayerData";

    public PlayerData PlayerData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void AddChip(int addAmount)
    {
        int maxValue = (int)1e9;

        addAmount = Mathf.Clamp(addAmount, 0, maxValue);
        int ogirinalAmount = Mathf.Clamp(PlayerData.chip, 0, maxValue);

        PlayerData.chip = Mathf.Clamp(ogirinalAmount + addAmount, 0, maxValue);

        SaveData();
    }

    #region Save, Load
    private void SaveData()
    {
        string json = JsonUtility.ToJson(PlayerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(PLAYER_DATA, string.Empty);

        if (string.IsNullOrEmpty(json))
        {
            PlayerData = new()
            {
                chip = 0,
            };
        }
        else
        {
            PlayerData = JsonUtility.FromJson<PlayerData>(json);
        }
    }
    #endregion
}

[Serializable]
public class PlayerData
{
    public int chip;
}