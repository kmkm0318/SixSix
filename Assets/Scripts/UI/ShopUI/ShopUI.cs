using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Transform availityDiceMerchantParent;
    [SerializeField] private Transform gambleDiceMerchantParent;
    [SerializeField] private Transform handEnhanceMerchantParent;
    [SerializeField] private Transform playDiceEnhanceMerchantParent;
    [SerializeField] private AvailityDiceMerchantUI availityDiceMerchantPrefab;
    [SerializeField] private GambleDiceMerchantUI gambleDiceMerchantPrefab;
    [SerializeField] private HandEnhanceMerchantUI handEnhanceMerchantPrefab;
    [SerializeField] private PlayDiceEnhanceMerchantUI playDiceEnhanceMerchantPrefab;
    [SerializeField] private ButtonPanel rerollButton;
    [SerializeField] private ButtonPanel closeButton;
    [SerializeField] private List<ScrollRect> scrollRects;

    public event Action OnShopUIOpened;
    public event Action OnShopUIClosed;

    private ObjectPool<AvailityDiceMerchantUI> availityDiceMerchantPool;
    private ObjectPool<GambleDiceMerchantUI> gambleDiceMerchantPool;
    private ObjectPool<HandEnhanceMerchantUI> handEnhanceMerchantPool;
    private ObjectPool<PlayDiceEnhanceMerchantUI> playDiceEnhanceMerchantPool;

    private Tween currentTween;

    private void Start()
    {
        InitPool();
        RegisterEvents();
        rerollButton.OnClick += OnclickRerollButton;
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

        gambleDiceMerchantPool = new(
            () => Instantiate(gambleDiceMerchantPrefab, gambleDiceMerchantParent),
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
        if (obj > 0)
        {
            rerollButton.SetText($"Reroll(${obj})");
        }
        else
        {
            rerollButton.SetText("Reroll");
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(AnimationFunction.ShakeAnimation(rerollButton.Text.transform, true));
        }
    }
    #endregion

    private void InitMerchantUI()
    {
        InitAvailityDiceMerchantUI();
        InitGambleDiceMerchantUI();

        var handEnhanceCount = ShopManager.Instance.MerchantItemCountMax;

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

    private void InitGambleDiceMerchantUI()
    {
        foreach (Transform child in gambleDiceMerchantParent)
        {
            if (child.gameObject.activeSelf && child.TryGetComponent(out GambleDiceMerchantUI merchantUI))
            {
                gambleDiceMerchantPool.Release(merchantUI);
            }
        }

        List<GambleDiceSO> gambleDiceList = ShopManager.Instance.GetRandomGambleDiceList();
        for (int i = 0; i < gambleDiceList.Count; i++)
        {
            GambleDiceMerchantUI merchantUI = gambleDiceMerchantPool.Get();
            merchantUI.Init(gambleDiceList[i]);
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

    private void Show(Action onComplete = null)
    {
        currentTween?.Kill(true);
        shopPanel.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        currentTween = shopPanel.DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
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
    }
}

