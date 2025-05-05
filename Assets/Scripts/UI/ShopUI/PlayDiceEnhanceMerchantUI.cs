using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayDiceEnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private int enhanceLevel;
    private ScorePair enhanceValue;
    private int price;
    private int idx;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        ShopManager.Instance.TryPurchasePlayDiceEnhance(new(enhanceLevel, enhanceValue, price, idx));
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
    }

    private void OnPlayDiceEnhancePurchaseAttempted(DiceEnhancePurchaseContext context, PurchaseResult result)
    {
        if (context.Index == idx && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(int enhanceLevel, ScorePair enhanceValue, int price, int idx)
    {
        this.enhanceLevel = enhanceLevel;
        this.enhanceValue = enhanceValue;
        this.price = price;
        this.idx = idx;

        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = $"Dice Enhance Lv{enhanceLevel}";
        descriptionText.text = $"Enhance Selected Play Dice\nEnhance Value: {enhanceValue}";
        buttonText.text = $"Buy(${price})";
    }
}
