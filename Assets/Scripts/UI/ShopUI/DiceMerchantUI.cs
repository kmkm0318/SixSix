using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DiceMerchantUI : MonoBehaviour
{
    [SerializeField] private ShopDiceIcon diceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private LocalizedString buyButtonText;
    [SerializeField] private ButtonPanel buyButton;

    private AbilityDiceSO avilityDiceSO;
    private GambleDiceSO gambleDiceSO;

    private void Start()
    {
        buyButton.OnClick += OnBuyButtonClicked;
        RegisterEvents();
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

    private void OnBuyButtonClicked()
    {
        if (avilityDiceSO != null)
        {
            ShopManager.Instance.TryPurchaseAbilityDice(avilityDiceSO);
        }
        else if (gambleDiceSO != null)
        {
            ShopManager.Instance.TryPurchaseGambleDice(gambleDiceSO);
        }
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnAbilityDicePurchaseAttempted += OnAbilityDicePurchaseAttempted;
        ShopManager.Instance.OnGambleDicePurchaseAttempted += OnGambleDicePurchaseAttempted;
    }

    private void OnAbilityDicePurchaseAttempted(AbilityDiceSO sO, PurchaseResult result)
    {
        if (avilityDiceSO == null) return;
        if (avilityDiceSO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnGambleDicePurchaseAttempted(GambleDiceSO sO, PurchaseResult result)
    {
        if (gambleDiceSO == null) return;
        if (gambleDiceSO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(AbilityDiceSO sO)
    {
        avilityDiceSO = sO;
        UpdateUI();
    }

    public void Init(GambleDiceSO sO)
    {
        gambleDiceSO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (avilityDiceSO != null)
        {
            diceImage.Init(avilityDiceSO);

            nameText.text = avilityDiceSO.DiceName;
            buyButtonText.Arguments = new object[] { avilityDiceSO.price };

        }
        else if (gambleDiceSO != null)
        {
            diceImage.Init(gambleDiceSO);

            nameText.text = gambleDiceSO.DiceName;
            buyButtonText.Arguments = new object[] { gambleDiceSO.price };
        }
        else
        {
            Debug.LogWarning("Neither AbilityDiceSO nor GambleDiceSO is initialized.");
        }

        buyButton.SetText(buyButtonText.GetLocalizedString());
    }
}
