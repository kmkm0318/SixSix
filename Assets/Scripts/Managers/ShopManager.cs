using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int initialRerollCost = 5;
    [SerializeField] private int merchantItemCountMax = 3;

    private AvailityDiceListSO availityDiceListSO;
    private GambleDiceListSO gambleDiceListSO;
    private int rerollCost = 0;

    public int MerchantItemCountMax => merchantItemCountMax;
    public int RerollCost
    {
        get => rerollCost;
        set
        {
            if (rerollCost == value) return;
            if (value < 0) value = 0;
            rerollCost = value;
            OnRerollCostChanged?.Invoke(rerollCost);
        }
    }

    public event Action<AvailityDiceSO, PurchaseResult> OnAvailityDicePurchaseAttempted;
    public event Action<GambleDiceSO, PurchaseResult> OnGambleDicePurchaseAttempted;
    public event Action<HandEnhancePurchaseContext, PurchaseResult> OnHandEnhancePurchaseAttempted;
    public event Action<DiceEnhancePurchaseContext, PurchaseResult> OnPlayDiceEnhancePurchaseAttempted;
    public event Action<AvailityDiceSO> OnAvailityDiceSelled;
    public event Action<int> OnRerollCostChanged;
    public event Action OnRerollCompleted;

    private void Start()
    {
        availityDiceListSO = DataContainer.Instance.ShopAvailityDiceListSO;
        gambleDiceListSO = DataContainer.Instance.ShopGambleDiceListSO;

        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopUI.Instance.OnShopUIClosed += OnShopUIClosed;
        DiceManager.Instance.OnAvailityDiceClicked += OnAvailityDiceClicked;
    }

    private void OnShopUIClosed()
    {
        EndShop();
    }

    private void OnAvailityDiceClicked(AvailityDice dice)
    {
        if (dice == null || GameManager.Instance.CurrentGameState != GameState.Shop) return;
        if (!DiceManager.Instance.AvailityDiceList.Contains(dice)) return;
        var diceSO = dice.AvailityDiceSO;

        DiceManager.Instance.RemoveAvailityDice(dice);

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

        if (MoneyManager.Instance.Money < availityDiceSO.price)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughMoney);
            return;
        }

        if (DiceManager.Instance.AvailityDiceList.Count >= DiceManager.Instance.CurrentAvailityDiceMax)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        OnAvailityDicePurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.Success);
    }

    public void TryPurchaseGambleDice(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceSO == null)
        {
            OnGambleDicePurchaseAttempted?.Invoke(null, PurchaseResult.Failed);
            return;
        }

        if (GambleDiceSaveManager.Instance.IsFull)
        {
            OnGambleDicePurchaseAttempted?.Invoke(gambleDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        if (MoneyManager.Instance.Money < gambleDiceSO.price)
        {
            OnGambleDicePurchaseAttempted?.Invoke(gambleDiceSO, PurchaseResult.NotEnoughMoney);
            return;
        }

        if (DiceManager.Instance.GambleDiceList.Count >= DiceManager.Instance.CurrentGambleDiceMax)
        {
            OnGambleDicePurchaseAttempted?.Invoke(gambleDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        OnGambleDicePurchaseAttempted?.Invoke(gambleDiceSO, PurchaseResult.Success);
    }

    public void TryPurchaseHandEnhance(HandEnhancePurchaseContext context)
    {
        if (MoneyManager.Instance.Money < context.Price)
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
        if (MoneyManager.Instance.Money < context.Price)
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
        while (randomAvailityDiceList.Count < merchantItemCountMax)
        {
            if (randomAvailityDiceList.Count >= availityDiceListSO.availityDiceSOList.Count) break;

            AvailityDiceSO randomAvailityDice = availityDiceListSO.GetRandomAvailityDiceSO();
            if (randomAvailityDice == null) continue;
            if (randomAvailityDiceList.Contains(randomAvailityDice)) continue;
            randomAvailityDiceList.Add(randomAvailityDice);
        }
        return randomAvailityDiceList;
    }

    public List<GambleDiceSO> GetRandomGambleDiceList()
    {
        List<GambleDiceSO> randomGambleDiceList = new();
        while (randomGambleDiceList.Count < merchantItemCountMax)
        {
            if (randomGambleDiceList.Count >= gambleDiceListSO.gambleDiceSOList.Count) break;

            GambleDiceSO randomGambleDice = gambleDiceListSO.GetRandomGambleDiceSO();
            if (randomGambleDice == null) continue;
            if (randomGambleDiceList.Contains(randomGambleDice)) continue;
            randomGambleDiceList.Add(randomGambleDice);
        }
        return randomGambleDiceList;
    }

    public List<DiceEnhancePurchaseContext> GetRandomDiceEnhanceList(int count)
    {
        List<DiceEnhancePurchaseContext> diceEnhanceList = new();
        for (int i = 0; i < count; i++)
        {
            int enhanceLevel = UnityEngine.Random.Range(1, 4);

            ScorePair scorePair = EnhanceManager.Instance.GetEnhanceValue(enhanceLevel, true);

            int price = EnhanceManager.Instance.GetEnhancePrice(enhanceLevel);

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
            int price = enhanceLevel * 6 - (enhanceLevel - 1);

            res.Add(new(enhanceLevel, price, i));
        }
        return res;
    }

    public void TryReroll()
    {
        if (MoneyManager.Instance.Money < RerollCost)
        {
            OnAvailityDicePurchaseAttempted?.Invoke(null, PurchaseResult.NotEnoughMoney);
            return;
        }

        MoneyManager.Instance.Money -= RerollCost;
        SequenceManager.Instance.ApplyParallelCoroutine();
        OnRerollCompleted?.Invoke();
        RerollCost++;
    }

    public void StartShop()
    {
        RerollCost = initialRerollCost;
        GameManager.Instance.ChangeState(GameState.Shop);
    }

    public void EndShop()
    {
        GameManager.Instance.ChangeState(GameState.Round);
    }
}

public enum PurchaseResult
{
    Success,
    NotEnoughMoney,
    NotEnoughDiceSlot,
    Failed,
}