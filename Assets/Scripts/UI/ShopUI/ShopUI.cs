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
    [SerializeField] private float showDuration = 0.5f;
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
        availityDiceMerchantPool = new ObjectPool<AvailityDiceMerchantUI>(
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

        handEnhanceMerchantPool = new ObjectPool<HandEnhanceMerchantUI>(
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

        playDiceEnhanceMerchantPool = new ObjectPool<PlayDiceEnhanceMerchantUI>(
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
        ShopManager.Instance.OnShopStarted += OnShopStarted;
        ShopManager.Instance.OnRerollCompleted += OnRerollCompleted;
        ShopManager.Instance.OnRerollCostChanged += OnRerollCostChanged;
        EnhanceManager.Instance.OnDiceEnhanceStarted += OnEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += OnEnhanceCompleted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnEnhanceCompleted;
    }

    private void OnEnhanceStarted()
    {
        Hide(false);
    }

    private void OnEnhanceCompleted()
    {
        Show(false);
    }

    private void OnShopStarted()
    {
        InitAvailityDiceMerchantUI();
        InitHandEnhanceMerchantUI();
        InitPlayDiceEnhanceMerchantUI();
        Show();
    }

    private void OnRerollCompleted()
    {
        InitAvailityDiceMerchantUI();
        InitHandEnhanceMerchantUI();
        InitPlayDiceEnhanceMerchantUI();
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
            StartCoroutine(AnimationManager.Instance.PlayShakeAnimation(rerollButtonText.transform, true));
        }
    }
    #endregion
    private void InitAvailityDiceMerchantUI()
    {
        foreach (Transform child in availityDiceMerchantParent)
        {
            if (child.TryGetComponent(out AvailityDiceMerchantUI merchantUI))
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

    private void InitHandEnhanceMerchantUI()
    {
        foreach (Transform child in handEnhanceMerchantParent)
        {
            if (child.TryGetComponent(out HandEnhanceMerchantUI merchantUI))
            {
                handEnhanceMerchantPool.Release(merchantUI);
            }
        }

        List<HandEnhancePurchaseContext> handEnhanceList = ShopManager.Instance.GetRandomHandEnhanceList();

        for (int i = 0; i < handEnhanceList.Count; i++)
        {
            HandEnhanceMerchantUI merchantUI = handEnhanceMerchantPool.Get();
            merchantUI.Init(handEnhanceList[i].EnhanceLevel, handEnhanceList[i].Price, handEnhanceList[i].Index);
        }
    }

    private void InitPlayDiceEnhanceMerchantUI()
    {
        foreach (Transform child in playDiceEnhanceMerchantParent)
        {
            if (child.TryGetComponent(out PlayDiceEnhanceMerchantUI merchantUI))
            {
                playDiceEnhanceMerchantPool.Release(merchantUI);
            }
        }

        var scorePairs = ShopManager.Instance.GetRandomDiceEnhanceList();
        for (int i = 0; i < scorePairs.Count; i++)
        {
            PlayDiceEnhanceMerchantUI merchantUI = playDiceEnhanceMerchantPool.Get();
            merchantUI.Init(scorePairs[i].EnhanceValue, scorePairs[i].Price, scorePairs[i].Index);
        }
    }

    private void Show(bool isInvoke = true)
    {
        currentTween?.Kill(true);
        shopPanel.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        currentTween = shopPanel.DOAnchorPos(Vector3.zero, showDuration).SetEase(Ease.InOutBack).OnComplete(() =>
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
        currentTween = shopPanel.DOAnchorPos(hidePos, showDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            if (isInvoke)
            {
                OnShopUIClosed?.Invoke();
            }
            gameObject.SetActive(false);
        });
    }
}

