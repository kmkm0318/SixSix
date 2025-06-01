using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int initialRerollCost = 5;
    [SerializeField] private int merchantItemCountMax = 3;

    private AbilityDiceListSO abilityDiceListSO;
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

    public event Action<AbilityDiceSO, PurchaseResult> OnAbilityDicePurchaseAttempted;
    public event Action<GambleDiceSO, PurchaseResult> OnGambleDicePurchaseAttempted;
    public event Action<EnhancePurchaseContext, PurchaseResult> OnEnhancePurchaseAttempted;
    public event Action<AbilityDiceSO> OnAbilityDiceSelled;
    public event Action<GambleDiceSO> OnGambleDiceSelled;
    public event Action<int> OnRerollCostChanged;
    public event Action OnRerollCompleted;

    private void Start()
    {
        abilityDiceListSO = DataContainer.Instance.ShopAbilityDiceListSO;
        gambleDiceListSO = DataContainer.Instance.ShopGambleDiceListSO;

        RegisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopUI.Instance.OnShopUIClosed += OnShopUIClosed;
        DiceManager.Instance.OnAbilityDiceClicked += OnAbilityDiceClicked;
    }

    private void OnShopUIClosed()
    {
        EndShop();
    }

    private void OnAbilityDiceClicked(AbilityDice dice)
    {
        if (dice == null || GameManager.Instance.CurrentGameState != GameState.Shop) return;
        if (!DiceManager.Instance.AbilityDiceList.Contains(dice)) return;
        var diceSO = dice.AbilityDiceSO;

        DiceManager.Instance.RemoveAbilityDice(dice);

        OnAbilityDiceSelled?.Invoke(diceSO);
    }
    #endregion

    public void TryPurchaseAbilityDice(AbilityDiceSO abilityDiceSO)
    {
        if (abilityDiceSO == null)
        {
            OnAbilityDicePurchaseAttempted?.Invoke(null, PurchaseResult.Failed);
            return;
        }

        if (MoneyManager.Instance.Money < abilityDiceSO.price)
        {
            OnAbilityDicePurchaseAttempted?.Invoke(abilityDiceSO, PurchaseResult.NotEnoughMoney);
            return;
        }

        if (DiceManager.Instance.AbilityDiceList.Count >= DiceManager.Instance.CurrentAbilityDiceMax)
        {
            OnAbilityDicePurchaseAttempted?.Invoke(abilityDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        OnAbilityDicePurchaseAttempted?.Invoke(abilityDiceSO, PurchaseResult.Success);
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

        OnGambleDicePurchaseAttempted?.Invoke(gambleDiceSO, PurchaseResult.Success);
    }

    public void TryPurchaseEnhance(EnhancePurchaseContext context)
    {
        if (MoneyManager.Instance.Money < context.Price)
        {
            OnEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.NotEnoughMoney);
        }
        else
        {
            OnEnhancePurchaseAttempted?.Invoke(context, PurchaseResult.Success);
        }
    }

    public void SellGambleDice(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceSO == null) return;

        OnGambleDiceSelled?.Invoke(gambleDiceSO);
    }

    public List<AbilityDiceSO> GetRandomAbilityDiceList()
    {
        List<AbilityDiceSO> randomAbilityDiceList = new();
        while (randomAbilityDiceList.Count < merchantItemCountMax)
        {
            if (randomAbilityDiceList.Count >= abilityDiceListSO.abilityDiceSOList.Count) break;

            AbilityDiceSO randomAbilityDice = abilityDiceListSO.GetRandomAbilityDiceSO();
            if (randomAbilityDice == null) continue;
            if (randomAbilityDiceList.Contains(randomAbilityDice)) continue;
            randomAbilityDiceList.Add(randomAbilityDice);
        }
        return randomAbilityDiceList;
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

    public List<EnhancePurchaseContext> GetRandomEnhanceList()
    {
        int diceEnhanceCount = UnityEngine.Random.Range(1, merchantItemCountMax);

        List<EnhancePurchaseContext> diceEnhanceList = new();
        for (int i = 0; i < merchantItemCountMax; i++)
        {
            EnhanceType enhanceType = i < diceEnhanceCount ? EnhanceType.Dice : EnhanceType.Hand;

            int enhanceLevel = UnityEngine.Random.Range(1, 4);

            ScorePair scorePair = EnhanceManager.Instance.GetEnhanceValue(enhanceLevel, true);

            int price = EnhanceManager.Instance.GetEnhancePrice(enhanceLevel);

            diceEnhanceList.Add(new(enhanceType, enhanceLevel, scorePair, price, i));
        }
        return diceEnhanceList;
    }

    public void TryReroll()
    {
        if (MoneyManager.Instance.Money < RerollCost)
        {
            OnAbilityDicePurchaseAttempted?.Invoke(null, PurchaseResult.NotEnoughMoney);
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