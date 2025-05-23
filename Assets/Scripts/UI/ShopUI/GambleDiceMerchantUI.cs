using TMPro;
using UnityEngine;

public class GambleDiceMerchantUI : MonoBehaviour
{
    [SerializeField] private ShopDiceIcon diceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private ButtonPanel buyButton;

    private GambleDiceSO gambleDiceSO;

    private void Start()
    {
        buyButton.OnClick += OnBuyButtonClicked;
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        if (gambleDiceSO == null) return;

        ShopManager.Instance.TryPurchaseGambleDice(gambleDiceSO);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnGambleDicePurchaseAttempted += OnPurchaseAttempted;
    }

    private void OnPurchaseAttempted(GambleDiceSO sO, PurchaseResult result)
    {
        if (gambleDiceSO == sO && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(GambleDiceSO sO)
    {
        gambleDiceSO = sO;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gambleDiceSO == null) return;

        diceImage.Init(gambleDiceSO);

        nameText.text = gambleDiceSO.diceName;
        buyButton.SetText($"Buy(${gambleDiceSO.price})");
    }
}
