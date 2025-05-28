using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class EnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private AnimatedText nameText;
    [SerializeField] private AnimatedText descriptionText;
    [SerializeField] private ButtonPanel buyButton;
    [SerializeField] private LocalizedString diceEnhanceNameText;
    [SerializeField] private LocalizedString diceEnhanceDescriptionText;
    [SerializeField] private LocalizedString handEnhanceNameText;
    [SerializeField] private LocalizedString handEnhanceDescriptionText;
    [SerializeField] private LocalizedString buyButtonText;

    private EnhanceType enhanceType;
    private int enhanceLevel;
    private ScorePair enhanceValue;
    private int price;
    private int idx;

    private void Start()
    {
        buyButton.OnClick += OnBuyButtonClicked;
        RegisterEvents();
    }

    private void OnBuyButtonClicked()
    {
        ShopManager.Instance.TryPurchaseEnhance(new(enhanceType, enhanceLevel, enhanceValue, price, idx));
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
    }

    private void SelectedLocaleChanged(Locale locale)
    {
        UpdateUI();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnEnhancePurchaseAttempted += OnEnhancePurchaseAttempted;
    }

    private void OnEnhancePurchaseAttempted(EnhancePurchaseContext context, PurchaseResult result)
    {
        if (context.Index == idx && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(EnhancePurchaseContext context)
    {
        Init(context.EnhanceType, context.EnhanceLevel, context.EnhanceValue, context.Price, context.Index);
    }

    public void Init(EnhanceType enhanceType, int enhanceLevel, ScorePair enhanceValue, int price, int idx)
    {
        this.enhanceType = enhanceType;
        this.enhanceLevel = enhanceLevel;
        this.enhanceValue = enhanceValue;
        this.price = price;
        this.idx = idx;

        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.SetText(GetLocalizedName());
        descriptionText.SetText(GetLocalizedDescription());
        buyButton.SetText(GetBuyButtonText());
    }

    private string GetLocalizedName()
    {
        var localizedName = enhanceType switch
        {
            EnhanceType.Dice => diceEnhanceNameText,
            EnhanceType.Hand => handEnhanceNameText,
            _ => throw new ArgumentOutOfRangeException(nameof(enhanceType), enhanceType, null)
        };

        localizedName.Arguments = enhanceType switch
        {
            EnhanceType.Dice => new object[] { enhanceValue },
            EnhanceType.Hand => new object[] { enhanceLevel },
            _ => throw new ArgumentOutOfRangeException(nameof(enhanceType), enhanceType, null)
        };

        return localizedName.GetLocalizedString();
    }

    private string GetLocalizedDescription()
    {
        var localizedDescription = enhanceType switch
        {
            EnhanceType.Dice => diceEnhanceDescriptionText,
            EnhanceType.Hand => handEnhanceDescriptionText,
            _ => throw new ArgumentOutOfRangeException(nameof(enhanceType), enhanceType, null)
        };

        localizedDescription.Arguments = enhanceType switch
        {
            EnhanceType.Dice => new object[] { enhanceValue },
            EnhanceType.Hand => new object[] { enhanceLevel },
            _ => throw new ArgumentOutOfRangeException(nameof(enhanceType), enhanceType, null)
        };

        return localizedDescription.GetLocalizedString();
    }

    private string GetBuyButtonText()
    {
        buyButtonText.Arguments = new object[] { price };
        return buyButtonText.GetLocalizedString();
    }
}