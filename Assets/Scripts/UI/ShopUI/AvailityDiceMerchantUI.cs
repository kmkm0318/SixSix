using System.Collections;
using DG.Tweening;
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
        ShopManager.Instance.OnPurchaseAttempted += OnPurchaseAttempted;
    }

    private void OnPurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (availityDiceSO == sO)
        {
            switch (result)
            {
                case PurchaseResult.Success:
                    gameObject.SetActive(false);
                    break;
                case PurchaseResult.NotEnoughMoney:
                case PurchaseResult.NotEnoughDiceSlot:
                    PlayButtonAnimation();
                    break;
                default:
                    break;
            }
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

        diceImage.sprite = availityDiceSO.diceFaceSpriteListSO.diceFaceList[0].sprite;
        nameText.text = availityDiceSO.diceName;
        descriptionText.text = availityDiceSO.GetDescriptionText();
        buttonText.text = $"Buy({availityDiceSO.purchasePrice})";
    }

    private void PlayButtonAnimation()
    {
        StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(buyButton.transform, true));
    }

    private IEnumerator PlayAnimationAndReset()
    {
        buyButton.transform.DOKill();
        yield return StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(buyButton.transform, true));
    }
}
