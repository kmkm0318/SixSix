using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DiceMerchantUI : MonoBehaviour
{
    [SerializeField] private ShopDiceIcon diceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private ButtonPanel buyButton;
    [SerializeField] private LocalizedString buyButtonText;
    [SerializeField] private GameObject recommendation;

    private AbilityDiceSO abilityDiceSO;
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
        if (abilityDiceSO != null)
        {
            ShopManager.Instance.TryPurchaseAbilityDice(abilityDiceSO);
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
        if (abilityDiceSO == null) return;
        if (abilityDiceSO == sO && result == PurchaseResult.Success)
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
        abilityDiceSO = sO;
        gambleDiceSO = null;
        UpdateUI();
    }

    public void Init(GambleDiceSO sO)
    {
        abilityDiceSO = null;
        gambleDiceSO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        recommendation.SetActive(false);

        if (abilityDiceSO != null)
        {
            diceImage.Init(abilityDiceSO);

            nameText.text = abilityDiceSO.DiceName;
            buyButtonText.Arguments = new object[] { abilityDiceSO.price };

            bool isRecommended = IsRecommended();

            if (isRecommended)
            {
                recommendation.SetActive(true);
            }
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

    private bool IsRecommended()
    {
        int targetId = abilityDiceSO.abilityDiceID;
        int targetRound = RoundManager.Instance.CurrentRound + 1;

        var diceList = DiceManager.Instance.AbilityDiceList;

        List<int> diceIds = new();

        foreach (var dice in diceList)
        {
            diceIds.Add(dice.AbilityDiceSO.abilityDiceID);
        }

        return AbilityDiceRecommendManager.Instance.IsRecommended(diceIds, targetId, targetRound);
    }
}
