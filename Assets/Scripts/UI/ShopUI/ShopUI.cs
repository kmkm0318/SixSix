using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ShopUI : BaseUI
{
    [SerializeField] private Transform abilityDiceMerchantParent;
    [SerializeField] private Transform gambleDiceMerchantParent;
    [SerializeField] private Transform enhanceMerchantParent;
    [SerializeField] private DiceMerchantUI diceMerchantPrefab;
    [SerializeField] private EnhanceMerchantUI enhanceMerchantPrefab;
    [SerializeField] private ButtonPanel rerollButton;
    [SerializeField] private LocalizedString rerollButtonText;
    [SerializeField] private ButtonPanel closeButton;
    [SerializeField] private List<ScrollRect> scrollRects;

    private ObjectPool<DiceMerchantUI> diceMerchantPool;
    private ObjectPool<EnhanceMerchantUI> enhanceMerchantPool;

    private void Start()
    {
        InitPool();
        RegisterEvents();
        rerollButton.OnClick += OnclickRerollButton;
        LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
        closeButton.OnClick += OnClickCloseButton;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
    }

    private void SelectedLocaleChanged(Locale locale)
    {
        UpdateRerollButtonText();
    }

    private void OnclickRerollButton()
    {
        ShopManager.Instance.TryReroll();
    }

    private void OnClickCloseButton()
    {
        SequenceManager.Instance.AddCoroutine(() => Hide(() =>
        {
            ShopUIEvents.TriggerOnShopUIHidden();
        }));
    }

    private void InitPool()
    {
        diceMerchantPool = new(
            () => Instantiate(diceMerchantPrefab, abilityDiceMerchantParent),
            merchantUI =>
            {
                merchantUI.gameObject.SetActive(true);
                merchantUI.transform.SetAsLastSibling();
            },
            merchantUI => merchantUI.gameObject.SetActive(false),
            merchantUI => Destroy(merchantUI.gameObject),
            maxSize: 10
        );

        enhanceMerchantPool = new(
            () => Instantiate(enhanceMerchantPrefab, enhanceMerchantParent),
            merchantUI =>
            {
                merchantUI.gameObject.SetActive(true);
                merchantUI.transform.SetAsLastSibling();
            },
            merchantUI => merchantUI.gameObject.SetActive(false),
            merchantUI => Destroy(merchantUI.gameObject),
            maxSize: 10
        );
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Shop, OnShopStarted);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceEnded);

        ShopManager.Instance.OnRerollCompleted += OnRerollCompleted;
        ShopManager.Instance.OnRerollCostChanged += OnRerollCostChanged;
    }

    private void OnShopStarted()
    {
        Show(() =>
        {
            ShopUIEvents.TriggerOnShopUIShown();
        });
        InitMerchantUI();
    }

    private void OnEnhanceStarted()
    {
        Hide();
    }

    private void OnEnhanceEnded()
    {
        Show();
    }

    private void OnRerollCompleted()
    {
        InitMerchantUI();
    }

    private void OnRerollCostChanged(int obj)
    {
        UpdateRerollButtonText();

        if (gameObject.activeSelf)
        {
            StartCoroutine(AnimationFunction.ShakeAnimation(rerollButton.Text.transform, true));
        }
    }
    #endregion

    private void InitMerchantUI()
    {
        InitAbilityDiceMerchantUI();
        InitGambleDiceMerchantUI();
        InitEnhanceMerchantUI();

        ScrollToTop();
    }

    private void InitAbilityDiceMerchantUI()
    {
        foreach (Transform child in abilityDiceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out DiceMerchantUI merchantUI))
            {
                diceMerchantPool.Release(merchantUI);
            }
        }

        List<AbilityDiceSO> abilityDiceList = ShopManager.Instance.GetRandomAbilityDiceList();
        for (int i = 0; i < abilityDiceList.Count; i++)
        {
            var merchantUI = diceMerchantPool.Get();
            merchantUI.transform.SetParent(abilityDiceMerchantParent);
            merchantUI.Init(abilityDiceList[i]);
        }
    }

    private void InitGambleDiceMerchantUI()
    {
        foreach (Transform child in gambleDiceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out DiceMerchantUI merchantUI))
            {
                diceMerchantPool.Release(merchantUI);
            }
        }

        List<GambleDiceSO> gambleDiceList = ShopManager.Instance.GetRandomGambleDiceList();
        for (int i = 0; i < gambleDiceList.Count; i++)
        {
            var merchantUI = diceMerchantPool.Get();
            merchantUI.transform.SetParent(gambleDiceMerchantParent);
            merchantUI.Init(gambleDiceList[i]);
        }
    }

    private void InitEnhanceMerchantUI()
    {
        foreach (Transform child in enhanceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out EnhanceMerchantUI merchantUI))
            {
                enhanceMerchantPool.Release(merchantUI);
            }
        }

        var enhanceContext = ShopManager.Instance.GetRandomEnhanceList();
        foreach (var context in enhanceContext)
        {
            var merchantUI = enhanceMerchantPool.Get();
            merchantUI.Init(context.EnhanceType, context.EnhanceLevel, context.EnhanceValue, context.Price, context.Index);
        }
    }

    private void ScrollToTop()
    {
        foreach (var scrollRect in scrollRects)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void UpdateRerollButtonText()
    {
        rerollButtonText.Arguments = new object[] { ShopManager.Instance.RerollCost };
        rerollButtonText.RefreshString();
        rerollButton.SetText(rerollButtonText.GetLocalizedString());
    }
}

