using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int rerollCostMax = 6;
    [SerializeField] private int availityDiceMerchantCountMax = 3;
    [SerializeField] private AvailityDiceListSO availityDiceListSO;

    public event Action OnShopStarted;
    public event Action OnShopEnded;
    public event Action<AvailityDiceSO, PurchaseResult> OnPurchaseAttempted;
    public event Action<AvailityDiceSO> OnAvailityDiceSelled;

    private int rerollCost = 1;

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
            OnPurchaseAttempted?.Invoke(null, PurchaseResult.Failed);
            return;
        }

        if (PlayerMoneyManager.Instance.Money < availityDiceSO.purchasePrice)
        {
            OnPurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughMoney);
            return;
        }

        if (PlayerDiceManager.Instance.AvailityDiceList.Count >= PlayerDiceManager.Instance.AvailityDiceCountMax)
        {
            OnPurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.NotEnoughDiceSlot);
            return;
        }

        OnPurchaseAttempted?.Invoke(availityDiceSO, PurchaseResult.Success);
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
}

public enum PurchaseResult
{
    Success,
    NotEnoughMoney,
    NotEnoughDiceSlot,
    Failed,
}