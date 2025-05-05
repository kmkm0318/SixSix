using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int rerollCostMax = 6;
    [SerializeField] private int availityDiceMerchantCountMax = 3;
    [SerializeField] private int enhanceMerchantCountMax = 3;
    public int EnhanceMerchantCountMax => enhanceMerchantCountMax;

    public event Action OnShopStarted;
    public event Action OnShopEnded;
    public event Action<AvailityDiceSO, PurchaseResult> OnAvailityDicePurchaseAttempted;
    public event Action<HandEnhancePurchaseContext, PurchaseResult> OnHandEnhancePurchaseAttempted;
    public event Action<DiceEnhancePurchaseContext, PurchaseResult> OnPlayDiceEnhancePurchaseAttempted;
    public event Action<AvailityDiceSO> OnAvailityDiceSelled;
    public event Action<int> OnRerollCostChanged;
    public event Action OnRerollCompleted;

    private AvailityDiceListSO availityDiceListSO;
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
        availityDiceListSO = DataContainer.Instance.ShopAvailityDiceListSO;
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
        if (dice == null || GameManager.Instance.CurrentGameState != GameState.Shop) return;
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

        if (PlayerMoneyManager.Instance.Money < availityDiceSO.price)
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

    public void TryPurchaseHandEnhance(HandEnhancePurchaseContext context)
    {
        if (PlayerMoneyManager.Instance.Money < context.Price)
        {
            OnHandEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.NotEnoughMoney);
        }
        else
        {
            OnHandEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.Success);
        }
    }

    public void TryPurchasePlayDiceEnhance(DiceEnhancePurchaseContext context)
    {
        if (PlayerMoneyManager.Instance.Money < context.Price)
        {
            OnPlayDiceEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.NotEnoughMoney);
        }
        else
        {
            OnPlayDiceEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.Success);
        }
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

    public List<DiceEnhancePurchaseContext> GetRandomDiceEnhanceList(int count)
    {
        List<DiceEnhancePurchaseContext> diceEnhanceList = new();
        for (int i = 0; i < count; i++)
        {
            int enhanceLevel = UnityEngine.Random.Range(1, 4);
            int price = enhanceLevel * 5 - (enhanceLevel - 1);

            int baseScore = UnityEngine.Random.Range(0, enhanceLevel * 10 + 1);
            ScorePair scorePair = new(baseScore, 1f + (enhanceLevel * 10 - baseScore) * 0.1f);

            diceEnhanceList.Add(new(enhanceLevel, scorePair, price, i));
        }
        return diceEnhanceList;
    }

    public List<HandEnhancePurchaseContext> GetRandomHandEnhanceList(int count)
    {
        List<HandEnhancePurchaseContext> res = new();
        for (int i = 0; i < count; i++)
        {
            int enhanceLevel = UnityEngine.Random.Range(1, 4);
            int price = enhanceLevel * 3 - (enhanceLevel - 1);

            res.Add(new(enhanceLevel, price, i));
        }
        return res;
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