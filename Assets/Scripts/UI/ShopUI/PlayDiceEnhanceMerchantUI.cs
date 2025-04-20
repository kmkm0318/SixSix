using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayDiceEnhanceMerchantUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;

    private ScorePair enhanceValue;

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        RegisterEvents();
    }
    private void OnBuyButtonClicked()
    {
        ShopManager.Instance.TryPurchasePlayDiceEnhance(enhanceValue, GetPrice());
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnPlayDiceEnhancePurchaseAttempted += OnPlayDiceEnhancePurchaseAttempted;
    }

    private void OnPlayDiceEnhancePurchaseAttempted(ScorePair pair, int price, PurchaseResult result)
    {
        if (enhanceValue.baseScore == pair.baseScore && enhanceValue.multiplier == pair.multiplier && result == PurchaseResult.Success)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void Init(ScorePair scorePair)
    {
        enhanceValue = scorePair;

        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = "Enhance Play Dice";
        descriptionText.text = GetDescriptionText();
        buttonText.text = $"Buy({GetPrice()})";
    }

    private string GetDescriptionText()
    {
        return $"Enhance Play Dice\n" +
               $"Enhance Value: ({enhanceValue.baseScore}, {enhanceValue.multiplier})\n";
    }

    private int GetPrice()
    {
        return enhanceValue.baseScore + enhanceValue.multiplier;
    }
}
