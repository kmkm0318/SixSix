using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private float showDuration = 0.5f;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Button RerollButton;
    [SerializeField] private Button closeButton;

    public event Action OnShopUIOpened;
    public event Action OnShopUIClosed;

    private void Start()
    {
        RegisterEvents();
        closeButton.onClick.AddListener(() => SequenceManager.Instance.AddCoroutine(Hide));
        gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopManager.Instance.OnShopStarted += OnShopStarted;
    }

    private void OnShopStarted()
    {
        SequenceManager.Instance.AddCoroutine(Show);
    }
    #endregion

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
        shopPanel.DOAnchorPos(hidePos, showDuration).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            OnShopUIClosed?.Invoke();
            gameObject.SetActive(false);
        });
    }
}

