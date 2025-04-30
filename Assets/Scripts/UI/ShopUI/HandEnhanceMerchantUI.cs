using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandEnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private int enhanceLevel;
    private int price;
    private int idx;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        ShopManager.Instance.TryPurchaseHandEnhance(new(enhanceLevel, price, idx));
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
    }

    private void OnHandEnhancePurchaseAttempted(HandEnhancePurchaseContext context, PurchaseResult result)
    {
        if (context.Index == idx)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(int enhanceLevel, int price, int idx)
    {
        this.enhanceLevel = enhanceLevel;
        this.price = price;
        this.idx = idx;

        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = $"Hand Enhance({enhanceLevel})";
        descriptionText.text = $"Enhance Hand {enhanceLevel} levels\nwhich hand you selected";
        buttonText.text = $"Buy({price})";
    }
}
