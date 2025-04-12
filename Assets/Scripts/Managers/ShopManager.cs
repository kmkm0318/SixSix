using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private int rerollCostMax = 6;
    [SerializeField] private int availityDiceNum = 3;
    [SerializeField] private List<AvailityDiceMerchant> availityDiceMerchants = new();


    public event Action OnShopStarted;
    public event Action OnShopEnded;

    private int rerollCost = 1;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        ShopUI.Instance.OnShopUIClosed += OnShopUIClosed;
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
}

public struct AvailityDiceMerchant
{
    public AvailityDiceSO availityDiceSO;
    public int price;
}

public struct EnhancementMerchant
{

}