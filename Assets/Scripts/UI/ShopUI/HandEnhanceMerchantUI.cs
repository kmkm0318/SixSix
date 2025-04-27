using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandEnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private HandSO handSO;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        if (handSO == null) return;

        ShopManager.Instance.TryPurchaseHandEnhance(handSO);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnHandEnhancePurchaseAttempted += OnHandEnhancePurchaseAttempted;
    }

    private void OnHandEnhancePurchaseAttempted(HandSO sO, PurchaseResult result)
    {
        if (handSO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(HandSO sO)
    {
        handSO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (handSO == null) return;

        nameText.text = handSO.handName;
        descriptionText.text = handSO.GetDescriptionText();
        buttonText.text = $"Buy({handSO.purchasePrice})";
    }
}
