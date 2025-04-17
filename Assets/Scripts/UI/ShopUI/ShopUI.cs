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
    [SerializeField] private Transform handCategoryEnhanceMerchantParent;
    [SerializeField] private AvailityDiceMerchantUI availityDiceMerchantPrefab;
    [SerializeField] private HandCategoryEnhanceMerchantUI handCategoryEnhanceMerchantPrefab;
    [SerializeField] private Button rerollButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text rerollButtonText;

    public event Action OnShopUIOpened;
    public event Action OnShopUIClosed;

    private ObjectPool<AvailityDiceMerchantUI> availityDiceMerchantPool;
    private ObjectPool<HandCategoryEnhanceMerchantUI> handCategoryEnhanceMerchantPool;

    private void Start()
    {
        InitPool();
        RegisterEvents();
        rerollButton.onClick.AddListener(ShopManager.Instance.TryReroll);
        closeButton.onClick.AddListener(() => SequenceManager.Instance.AddCoroutine(Hide));
        gameObject.SetActive(false);
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

        handCategoryEnhanceMerchantPool = new ObjectPool<HandCategoryEnhanceMerchantUI>(
            () => Instantiate(handCategoryEnhanceMerchantPrefab, handCategoryEnhanceMerchantParent),
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
    }

    private void OnShopStarted()
    {
        InitAvailityDiceMerchantUI();
        InitHandCategoryEnhanceMerchantUI();
        Show();
    }

    private void OnRerollCompleted()
    {
        InitAvailityDiceMerchantUI();
        InitHandCategoryEnhanceMerchantUI();
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
    }
    #endregion
    private void InitAvailityDiceMerchantUI()
    {
        foreach (Transform child in availityDiceMerchantParent)
        {
            availityDiceMerchantPool.Release(child.GetComponent<AvailityDiceMerchantUI>());
        }

        List<AvailityDiceSO> availityDiceList = ShopManager.Instance.GetRandomAvailityDiceList();
        for (int i = 0; i < availityDiceList.Count; i++)
        {
            AvailityDiceMerchantUI merchantUI = availityDiceMerchantPool.Get();
            merchantUI.Init(availityDiceList[i]);
        }
    }

    private void InitHandCategoryEnhanceMerchantUI()
    {
        foreach (Transform child in handCategoryEnhanceMerchantParent)
        {
            handCategoryEnhanceMerchantPool.Release(child.GetComponent<HandCategoryEnhanceMerchantUI>());
        }

        List<HandCategorySO> handCategoryList = ShopManager.Instance.GetRandomHandCategoryList();
        for (int i = 0; i < handCategoryList.Count; i++)
        {
            HandCategoryEnhanceMerchantUI merchantUI = handCategoryEnhanceMerchantPool.Get();
            merchantUI.Init(handCategoryList[i]);
        }
    }

    private void Show()
    {
        shopPanel.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        shopPanel.DOAnchorPos(Vector3.zero, showDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            OnShopUIOpened?.Invoke();
        });
    }

    private void Hide()
    {
        if (!gameObject.activeSelf) return;

        shopPanel.anchoredPosition = Vector3.zero;
        shopPanel.DOAnchorPos(hidePos, showDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            OnShopUIClosed?.Invoke();
            gameObject.SetActive(false);
        });
    }
}

