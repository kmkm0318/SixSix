using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int rerollCostMax = 6;
    [SerializeField] private int availityDiceMerchantCountMax = 3;
    [SerializeField] private int handCategoryEnhanceMerchantCountMax = 3;
    [SerializeField] private AvailityDiceListSO availityDiceListSO;
    [SerializeField] private HandCategoryListSO handCategoryListSO;

    public event Action OnShopStarted;
    public event Action OnShopEnded;
    public event Action<AvailityDiceSO, PurchaseResult> OnAvailityDicePurchaseAttempted;
    public event Action<HandCategorySO, PurchaseResult> OnHandCategoryEnhancePurchaseAttempted;
    public event Action<AvailityDiceSO> OnAvailityDiceSelled;
    public event Action<int> OnRerollCostChanged;
    public event Action OnRerollCompleted;

    private int rerollCost = 1;
    public int RerollCost
    {
        get => rerollCost;
        private set
        {
            if (rerollCost == value) return;
            rerollCost = value;
            OnRerollCostChanged?.Invoke(rerollCost);
        }
    }

    private void Start()
    {
        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        ShopUI.Instance.OnShopUIClosed += OnShopUIClosed;
        PlayerDiceManager.Instance.OnAvailityDiceClicked += OnAvailityDiceClicked;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Shop)
        {
            SetRandomRerollCost();
            OnShopStarted?.Invoke();
        }
    }

    private void OnShopUIClosed()
    {
        OnShopEnded?.Invoke();
    }

    private void OnAvailityDiceClicked(AvailityDice dice)
    {
        if (dice == null) return;
        var diceSO = dice.AvailityDiceSO;

        PlayerDiceManager.Instance.RemoveAvailityDice(dice);

        OnAvailityDiceSelled?.Invoke(diceSO);
    }
    #endregion

    public void TryPurchaseAvailityDice(AvailityDiceSO availityDiceSO)
    {
        if (availityDiceSO == null)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(null, PurchaseResult.Failed);
            return;
        }

        if (PlayerMoneyManager.Instance.Money < availityDiceSO.purchasePrice)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughMoney);
            return;
        }

        if (PlayerDiceManager.Instance.AvailityDiceList.Count >= PlayerDiceManager.Instance.AvailityDiceCountMax)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.Success);
    }

    public void TryPurchaseHandCategoryEnhance(HandCategorySO handCategorySO)
    {
        if (handCategorySO == null)
        {
            OnHandCategoryEnhancePurchaseAttempted?.Invoke(null, PurchaseResult.Failed);
            return;
        }

        if (PlayerMoneyManager.Instance.Money < handCategorySO.purchasePrice)
        {
            OnHandCategoryEnhancePurchaseAttempted?.Invoke(handCategorySO, PurchaseResult.NotEnoughMoney);
            return;
        }

        OnHandCategoryEnhancePurchaseAttempted?.Invoke(handCategorySO, PurchaseResult.Success);
    }

    public List<AvailityDiceSO> GetRandomAvailityDiceList()
    {
        List<AvailityDiceSO> randomAvailityDiceList = new();
        while (randomAvailityDiceList.Count < availityDiceMerchantCountMax)
        {
            AvailityDiceSO randomAvailityDice = availityDiceListSO.GetRandomAvailityDiceSO();
            if (randomAvailityDice == null) continue;
            if (randomAvailityDiceList.Contains(randomAvailityDice)) continue;
            randomAvailityDiceList.Add(randomAvailityDice);
        }
        return randomAvailityDiceList;
    }

    public List<HandCategorySO> GetRandomHandCategoryList()
    {
        List<HandCategorySO> randomHandCategoryList = new();
        while (randomHandCategoryList.Count < handCategoryEnhanceMerchantCountMax)
        {
            HandCategorySO randomHandCategory = handCategoryListSO.GetRandomHandCategorySO();
            if (randomHandCategory == null) continue;
            if (randomHandCategoryList.Contains(randomHandCategory)) continue;
            randomHandCategoryList.Add(randomHandCategory);
        }
        return randomHandCategoryList;
    }

    public void TryReroll()
    {
        if (PlayerMoneyManager.Instance.Money < RerollCost)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(null, PurchaseResult.NotEnoughMoney);
            return;
        }

        PlayerMoneyManager.Instance.Money -= RerollCost;
        OnRerollCompleted?.Invoke();
        SetRandomRerollCost();
    }

    private void SetRandomRerollCost()
    {
        RerollCost = UnityEngine.Random.Range(1, rerollCostMax + 1);
    }
}

public enum PurchaseResult
{
    Success,
    NotEnoughMoney,
    NotEnoughDiceSlot,
    Failed,
}