using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailityDiceMerchantUI : MonoBehaviour
{
    [SerializeField] private Image diceImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private AvailityDiceSO availityDiceSO;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
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

        diceImage.material = new(diceImage.material);
        availityDiceSO.shaderDataSO.SetMaterialProperties(diceImage.material);
        diceImage.sprite = availityDiceSO.diceSpriteListSO.spriteList[availityDiceSO.MaxDiceValue - 1];

        nameText.text = availityDiceSO.diceName;
        descriptionText.text = availityDiceSO.GetDescriptionText();
        buttonText.text = $"Buy(${availityDiceSO.price})";
    }
}
