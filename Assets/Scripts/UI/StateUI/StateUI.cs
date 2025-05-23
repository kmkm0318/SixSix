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
        moneyText.SetText("$0");
        playRemainText.SetText("0");
        rollRemainText.SetText("0");
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Loading, null, () =>
        {
            if (MoneyManager.Instance.Money != 0)
            {
                OnMoneyChanged(MoneyManager.Instance.Money);
            }
        });
        MoneyManager.Instance.OnMoneyChanged += OnMoneyChanged;
        PlayManager.Instance.OnPlayRemainChanged += OnPlayRemainChanged;
        RollManager.Instance.OnRollRemainChanged += OnRollRemainChanged;
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnAvailityDicePurchaseAttempted;
        ShopManager.Instance.OnGambleDicePurchaseAttempted += OnGambleDicePurchaseAttempted;
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

    private void OnAvailityDicePurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (result == PurchaseResult.NotEnoughMoney)
        {
            StartCoroutine(AnimationFunction.ShakeAnimation(moneyText.transform));
        }
    }

    private void OnGambleDicePurchaseAttempted(GambleDiceSO sO, PurchaseResult result)
    {
        if (result == PurchaseResult.NotEnoughMoney)
        {
            StartCoroutine(AnimationFunction.ShakeAnimation(moneyText.transform));
        }
    }
    #endregion
}
