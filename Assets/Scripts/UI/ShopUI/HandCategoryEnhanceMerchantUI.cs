using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandCategoryEnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private HandCategorySO handCategorySO;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        if (handCategorySO == null) return;

        ShopManager.Instance.TryPurchaseHandCategoryEnhance(handCategorySO);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnHandCategoryEnhancePurchaseAttempted += OnHandCategoryEnhancePurchaseAttempted;
    }

    private void OnHandCategoryEnhancePurchaseAttempted(HandCategorySO sO, PurchaseResult result)
    {
        if (handCategorySO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(HandCategorySO sO)
    {
        handCategorySO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (handCategorySO == null) return;

        nameText.text = handCategorySO.handCategoryName;
        descriptionText.text = handCategorySO.GetDescriptionText();
        buttonText.text = $"Buy({handCategorySO.purchasePrice})";
    }
}
