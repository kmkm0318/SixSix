using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Transform availityDiceMerchantParent;
    [SerializeField] private Transform handEnhanceMerchantParent;
    [SerializeField] private Transform playDiceEnhanceMerchantParent;
    [SerializeField] private AvailityDiceMerchantUI availityDiceMerchantPrefab;
    [SerializeField] private HandEnhanceMerchantUI handEnhanceMerchantPrefab;
    [SerializeField] private PlayDiceEnhanceMerchantUI playDiceEnhanceMerchantPrefab;
    [SerializeField] private Button rerollButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text rerollButtonText;
    [SerializeField] private List<ScrollRect> scrollRects;

    public event Action OnShopUIOpened;
    public event Action OnShopUIClosed;

    private ObjectPool<AvailityDiceMerchantUI> availityDiceMerchantPool;
    private ObjectPool<HandEnhanceMerchantUI> handEnhanceMerchantPool;
    private ObjectPool<PlayDiceEnhanceMerchantUI> playDiceEnhanceMerchantPool;

    private Tween currentTween;

    private void Start()
    {
        InitPool();
        RegisterEvents();
        rerollButton.onClick.AddListener(OnclickRerollButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
        gameObject.SetActive(false);
    }

    private void OnclickRerollButton()
    {
        ShopManager.Instance.TryReroll();
    }

    private void OnClickCloseButton()
    {
        SequenceManager.Instance.AddCoroutine(() => Hide(true));
    }

    private void InitPool()
    {
        availityDiceMerchantPool = new(
            () => Instantiate(availityDiceMerchantPrefab, availityDiceMerchantParent),
            merchantUI =>
            {
                merchantUI.gameObject.SetActive(true);
                merchantUI.transform.SetAsLastSibling();
            },
            merchantUI => merchantUI.gameObject.SetActive(false),
            merchantUI => Destroy(merchantUI.gameObject),
            maxSize: 10
        );

        handEnhanceMerchantPool = new(
            () => Instantiate(handEnhanceMerchantPrefab, handEnhanceMerchantParent),
            merchantUI =>
            {
                merchantUI.gameObject.SetActive(true);
                merchantUI.transform.SetAsLastSibling();
            },
            merchantUI => merchantUI.gameObject.SetActive(false),
            merchantUI => Destroy(merchantUI.gameObject),
            maxSize: 10
        );

        playDiceEnhanceMerchantPool = new(
            () => Instantiate(playDiceEnhanceMerchantPrefab, playDiceEnhanceMerchantParent),
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
        InitMerchantUI();
        Show();
    }

    private void OnEnhanceStarted()
    {
        Hide(false);
    }

    private void OnEnhanceEnded()
    {
        Show(false);
    }

    private void OnRerollCompleted()
    {
        InitMerchantUI();
    }

    private void OnRerollCostChanged(int obj)
    {
        if (obj > 0)
        {
            rerollButtonText.text = $"Reroll({obj})";
        }
        else
        {
            rerollButtonText.text = "Reroll";
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(AnimationFunction.PlayShakeAnimation(rerollButtonText.transform));
        }
    }
    #endregion

    private void InitMerchantUI()
    {
        InitAvailityDiceMerchantUI();

        var handEnhanceCount = ShopManager.Instance.EnhanceMerchantCountMax;

        if (handEnhanceCount > 0)
        {
            int playDiceEnhanceCount = UnityEngine.Random.Range(1, handEnhanceCount);

            InitPlayDiceEnhanceMerchantUI(playDiceEnhanceCount);
            InitHandEnhanceMerchantUI(handEnhanceCount - playDiceEnhanceCount);
        }

        ScrollToTop();
    }

    private void InitAvailityDiceMerchantUI()
    {
        foreach (Transform child in availityDiceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out AvailityDiceMerchantUI merchantUI))
            {
                availityDiceMerchantPool.Release(merchantUI);
            }
        }

        List<AvailityDiceSO> availityDiceList = ShopManager.Instance.GetRandomAvailityDiceList();
        for (int i = 0; i < availityDiceList.Count; i++)
        {
            AvailityDiceMerchantUI merchantUI = availityDiceMerchantPool.Get();
            merchantUI.Init(availityDiceList[i]);
        }
    }

    private void InitPlayDiceEnhanceMerchantUI(int count)
    {
        foreach (Transform child in playDiceEnhanceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out PlayDiceEnhanceMerchantUI merchantUI))
            {
                playDiceEnhanceMerchantPool.Release(merchantUI);
            }
        }

        var scorePairs = ShopManager.Instance.GetRandomDiceEnhanceList(count);
        for (int i = 0; i < scorePairs.Count; i++)
        {
            PlayDiceEnhanceMerchantUI merchantUI = playDiceEnhanceMerchantPool.Get();
            merchantUI.Init(scorePairs[i].EnhanceLevel, scorePairs[i].EnhanceValue, scorePairs[i].Price, scorePairs[i].Index);
        }
    }

    private void InitHandEnhanceMerchantUI(int count)
    {
        foreach (Transform child in handEnhanceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out HandEnhanceMerchantUI merchantUI))
            {
                handEnhanceMerchantPool.Release(merchantUI);
            }
        }

        List<HandEnhancePurchaseContext> handEnhanceList = ShopManager.Instance.GetRandomHandEnhanceList(count);

        for (int i = 0; i < handEnhanceList.Count; i++)
        {
            HandEnhanceMerchantUI merchantUI = handEnhanceMerchantPool.Get();
            merchantUI.Init(handEnhanceList[i].EnhanceLevel, handEnhanceList[i].Price, handEnhanceList[i].Index);
        }
    }

    private void ScrollToTop()
    {
        foreach (var scrollRect in scrollRects)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    private void Show(bool isInvoke = true)
    {
        currentTween?.Kill(true);
        shopPanel.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        currentTween = shopPanel.DOAnchorPos(Vector3.zero, DataContainer.Instance.DefaultDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            if (isInvoke)
            {
                OnShopUIOpened?.Invoke();
            }
        });
    }

    private void Hide(bool isInvoke = true)
    {
        if (!gameObject.activeSelf) return;

        currentTween?.Kill(true);
        shopPanel.anchoredPosition = Vector3.zero;
        currentTween = shopPanel.DOAnchorPos(hidePos, DataContainer.Instance.DefaultDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            if (isInvoke)
            {
                OnShopUIClosed?.Invoke();
            }
            gameObject.SetActive(false);
        });
    }
}

