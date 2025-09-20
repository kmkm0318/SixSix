using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Transform abilityDiceMerchantParent;
    [SerializeField] private Transform gambleDiceMerchantParent;
    [SerializeField] private Transform enhanceMerchantParent;
    [SerializeField] private DiceMerchantUI diceMerchantPrefab;
    [SerializeField] private EnhanceMerchantUI enhanceMerchantPrefab;
    [SerializeField] private ButtonPanel rerollButton;
    [SerializeField] private LocalizedString rerollButtonText;
    [SerializeField] private ButtonPanel closeButton;
    [SerializeField] private List<ScrollRect> scrollRects;

    public event Action OnShopUIOpened;
    public event Action OnShopUIClosed;

    private ObjectPool<DiceMerchantUI> diceMerchantPool;
    private ObjectPool<EnhanceMerchantUI> enhanceMerchantPool;

    private Tween currentTween;

    private void Start()
    {
        InitPool();
        RegisterEvents();
        rerollButton.OnClick += OnclickRerollButton;
        LocalizationSettings.SelectedLocaleChanged += (locale) =>
        {
            UpdateRerollButtonText();
        };
        closeButton.OnClick += OnClickCloseButton;
        gameObject.SetActive(false);
    }

    private void OnclickRerollButton()
    {
        ShopManager.Instance.TryReroll();
    }

    private void OnClickCloseButton()
    {
        SequenceManager.Instance.AddCoroutine(() => Hide(() =>
        {
            OnShopUIClosed?.Invoke();
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
            OnShopUIOpened?.Invoke();
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

    private void Show(Action onComplete = null)
    {
        currentTween?.Kill(true);
        shopPanel.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        currentTween = shopPanel.DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            onComplete?.Invoke();
        });

        AudioManager.Instance.PlaySFX(SFXType.UIShowHide);
    }

    private void Hide(Action onComplete = null)
    {
        if (!gameObject.activeSelf) return;

        currentTween?.Kill(true);
        shopPanel.anchoredPosition = Vector3.zero;
        currentTween = shopPanel.DOAnchorPos(hidePos, AnimationFunction.DefaultDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            onComplete?.Invoke();
            gameObject.SetActive(false);
        });

        AudioManager.Instance.PlaySFX(SFXType.UIShowHide);
    }
}

