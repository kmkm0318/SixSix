using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int rerollCostMax = 6;
    [SerializeField] private int availityDiceMerchantCountMax = 3;
    [SerializeField] private int handCategoryEnhanceMerchantCountMax = 3;
    [SerializeField] private int playDiceEnhanceMerchantCountMax = 3;

    public event Action OnShopStarted;
    public event Action OnShopEnded;
    public event Action<AvailityDiceSO, PurchaseResult> OnAvailityDicePurchaseAttempted;
    public event Action<HandCategorySO, PurchaseResult> OnHandCategoryEnhancePurchaseAttempted;
    public event Action<ScorePair, int, PurchaseResult> OnPlayDiceEnhancePurchaseAttempted;
    public event Action<AvailityDiceSO> OnAvailityDiceSelled;
    public event Action<int> OnRerollCostChanged;
    public event Action OnRerollCompleted;

    private AvailityDiceListSO availityDiceListSO;
    private HandCategoryListSO handCategoryListSO;
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
        availityDiceListSO = DataContainer.Instance.MerchantAvailityDiceListSO;
        handCategoryListSO = DataContainer.Instance.StandardHandCategoryListSO;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        ShopUI.Instance.OnShopUIClosed += OnShopUIClosed;
        PlayerDiceManager.Instance.OnAvailityDiceClicked += OnAvailityDiceClicked;
        BonusManager.Instance.OnAllBonusAchieved += OnAllBonusAchieved;
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

    private void OnAllBonusAchieved()
    {
        handCategoryListSO = DataContainer.Instance.HandCategoryListSO;
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

    public void TryPurchasePlayDiceEnhance(ScorePair enhanceValue, int price)
    {
        if (PlayerMoneyManager.Instance.Money < price)
        {
            OnPlayDiceEnhancePurchaseAttempted?.Invoke(enhanceValue, price, PurchaseResult.NotEnoughMoney);
            return;
        }

        OnPlayDiceEnhancePurchaseAttempted?.Invoke(enhanceValue, price, PurchaseResult.Success);
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

    public List<ScorePair> GetRandomPlayDiceEnhanceList()
    {
        List<ScorePair> randomPlayDiceEnhanceList = new();
        while (randomPlayDiceEnhanceList.Count < playDiceEnhanceMerchantCountMax)
        {
            ScorePair randomScorePair = new(UnityEngine.Random.Range(1, 7), UnityEngine.Random.Range(1, 7));

            bool flag = false;
            foreach (var scorePair in randomPlayDiceEnhanceList)
            {
                if (scorePair.baseScore == randomScorePair.baseScore && scorePair.multiplier == randomScorePair.multiplier)
                {
                    flag = true;
                    break;
                }
            }
            if (flag) continue;

            randomPlayDiceEnhanceList.Add(randomScorePair);
        }
        return randomPlayDiceEnhanceList;
    }

    public void TryReroll()
    {
        if (PlayerMoneyManager.Instance.Money < RerollCost)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(null, PurchaseResult.NotEnoughMoney);
            return;
        }

        PlayerMoneyManager.Instance.Money -= RerollCost;
        SequenceManager.Instance.ApplyParallelCoroutine();
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