using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailityDiceMerchantUI : MonoBehaviour
{
    [SerializeField] private ShopDiceIcon diceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private ButtonPanel buyButton;

    private AvailityDiceSO availityDiceSO;

    private void Start()
    {
        buyButton.OnClick += OnBuyButtonClicked;
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        if (availityDiceSO == null) return;

        ShopManager.Instance.TryPurchaseAvailityDice(availityDiceSO);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnPurchaseAttempted;
    }

    private void OnPurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (availityDiceSO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(AvailityDiceSO sO)
    {
        availityDiceSO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (availityDiceSO == null) return;

        diceImage.Init(availityDiceSO);

        nameText.text = availityDiceSO.diceName;
        buyButton.SetText($"Buy(${availityDiceSO.price})");
    }
}
