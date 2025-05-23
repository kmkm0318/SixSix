using System;
using System.Collections.Generic;

public class GambleDiceSaveManager : Singleton<GambleDiceSaveManager>
{
    private List<GambleDiceSO> savedGambleDiceSOs = new();
    private int currentGambleDiceSaveMax = 0;
    private bool isActive = false;

    public int CurrentGambleDiceSaveMax
    {
        get => currentGambleDiceSaveMax;
        set
        {
            if (value < 0) return;
            currentGambleDiceSaveMax = value;
            OnGambleDiceSaveMaxChanged?.Invoke(currentGambleDiceSaveMax);
        }
    }
    public bool IsFull => savedGambleDiceSOs.Count >= currentGambleDiceSaveMax;

    public event Action<int> OnGambleDiceSaveMaxChanged;
    public event Action<GambleDiceSO> OnGambleDiceAdded;
    public event Action<int> OnGambleDiceRemoved;

    private void Start()
    {
        currentGambleDiceSaveMax = DataContainer.Instance.CurrentDiceStat.defaultGambleDiceSaveMax;
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnGambleDicePurchaseAttempted += OnGambleDicePurchaseAttempted;
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollEnded);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceEnded);
    }

    private void OnGambleDicePurchaseAttempted(GambleDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;
        if (result == PurchaseResult.Success)
        {
            TryAddGambleDiceIcon(sO);
        }
    }

    private void OnPlayStarted()
    {
        isActive = true;
    }

    private void OnPlayEnded()
    {
        isActive = false;
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollEnded()
    {
        isActive = RollManager.Instance.RollRemain != 0;
    }

    private void OnEnhanceStarted()
    {
        GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.UnregisterEvent(GameState.Roll, OnRollStarted, OnRollEnded);
    }

    private void OnEnhanceEnded()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollEnded);
    }
    #endregion

    private bool TryAddGambleDiceIcon(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceSO == null) return false;
        if (savedGambleDiceSOs.Count >= currentGambleDiceSaveMax) return false;

        savedGambleDiceSOs.Add(gambleDiceSO);
        OnGambleDiceAdded?.Invoke(gambleDiceSO);

        return true;
    }

    private bool TryRemoveGambleDiceIcon(int idx)
    {
        if (!isActive) return false;
        if (idx < 0 || idx >= savedGambleDiceSOs.Count) return false;

        savedGambleDiceSOs.RemoveAt(idx);
        OnGambleDiceRemoved?.Invoke(idx);

        return true;
    }

    public bool TryAddRandomBossGambleDiceIcon()
    {
        var bossGambleDiceListSO = DataContainer.Instance.BossGambleDiceListSO;
        if (bossGambleDiceListSO == null) return false;
        if (savedGambleDiceSOs.Count >= currentGambleDiceSaveMax) return false;

        GambleDiceSO randomGambleDiceSO = bossGambleDiceListSO.GetRandomGambleDiceSO();
        if (randomGambleDiceSO == null) return false;

        return randomGambleDiceSO != null && TryAddGambleDiceIcon(randomGambleDiceSO);
    }

    public bool TryGenerateGambleDice(int idx)
    {
        if (!isActive) return false;
        if (idx < 0 || idx >= savedGambleDiceSOs.Count) return false;
        if (DiceManager.Instance.GambleDiceList.Count >= DiceManager.Instance.CurrentGambleDiceMax) return false;

        var savedGambleDiceSO = savedGambleDiceSOs[idx];

        if (!TryRemoveGambleDiceIcon(idx)) return false;

        return TryGenerateGambleDice(savedGambleDiceSO);
    }

    private bool TryGenerateGambleDice(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceSO == null) return false;

        DiceManager.Instance.GenerateGambleDice(gambleDiceSO);
        return true;
    }
}