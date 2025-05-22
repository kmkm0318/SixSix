using System;
using UnityEngine;

public class StateUI : Singleton<StateUI>
{
    [SerializeField] private ButtonPanel optionButton;
    [SerializeField] private AnimatedText moneyText;
    [SerializeField] private AnimatedText playRemainText;
    [SerializeField] private AnimatedText rollRemainText;

    public event Action OnOptionButtonClicked;

    protected override void Awake()
    {
        base.Awake();
        optionButton.OnClick += () => OnOptionButtonClicked?.Invoke();
    }

    private void Start()
    {
        ResetUI();
        RegisterEvents();
    }

    private void ResetUI()
    {
        moneyText.SetText("$" + MoneyManager.Instance.Money.ToString());
        playRemainText.SetText(PlayManager.Instance.PlayRemain.ToString());
        rollRemainText.SetText(RollManager.Instance.RollRemain.ToString());
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        MoneyManager.Instance.OnMoneyChanged += OnMoneyChanged;
        PlayManager.Instance.OnPlayRemainChanged += OnPlayRemainChanged;
        RollManager.Instance.OnRollRemainChanged += OnRollRemainChanged;
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnPurchaseAttempted;
    }

    private void OnMoneyChanged(int money)
    {
        AnimationFunction.AddUpdateTextAndPlayAnimation(moneyText, $"${money}");
    }

    private void OnPlayRemainChanged(int playRemain)
    {
        AnimationFunction.AddUpdateTextAndPlayAnimation(playRemainText, playRemain);
    }

    private void OnRollRemainChanged(int rollRemain)
    {
        AnimationFunction.AddUpdateTextAndPlayAnimation(rollRemainText, rollRemain);
    }

    private void OnPurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (result == PurchaseResult.NotEnoughMoney)
        {
            StartCoroutine(AnimationFunction.ShakeAnimation(moneyText.transform));
        }
    }
    #endregion
}
