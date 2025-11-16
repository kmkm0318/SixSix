using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GambleDiceSaveUI : BaseUI
{
    [SerializeField] private UIFocusHandler _panelMouseHandler;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private Transform _slotPrefab;
    [SerializeField] private GambleDiceIcon _gambleDiceIconPrefab;
    [SerializeField] private Ease _easeType = Ease.OutBack;

    private List<Slot> _slots = new();
    private bool _isAlwaysShow = false;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        _panelMouseHandler.OnPointerEntered += OnPointerEntered;
        _panelMouseHandler.OnPointerExited += OnPointerExited;

        for (int i = 0; i < DataContainer.Instance.CurrentPlayerStat.gambleDiceSaveMax; i++)
        {
            AddSlot();
        }
    }

    private void OnPointerEntered()
    {
        Show();
    }

    private void OnPointerExited()
    {
        if (_isAlwaysShow) return;
        Hide();
    }

    #region Slots
    private void AddSlot()
    {
        Transform newSlot = Instantiate(_slotPrefab, _slotParent);
        _slots.Add(new(newSlot));
        newSlot.gameObject.SetActive(true);
    }

    private void RemoveSlot()
    {
        if (_slots.Count == 0) return;
        Transform lastSlot = _slots[^1].slotTransform;
        _slots.RemoveAt(_slots.Count - 1);
        Destroy(lastSlot.gameObject);
    }
    #endregion

    #region RegisterEvents
    private void RegisterEvents()
    {
        GambleDiceSaveManager.Instance.OnGambleDiceSaveMaxChanged += OnGambleDiceSaveMaxChanged;
        GambleDiceSaveManager.Instance.OnGambleDiceAdded += OnGambleDiceAdded;
        GambleDiceSaveManager.Instance.OnGambleDiceRemoved += OnGambleDiceRemoved;

        GameManager.Instance.RegisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
    }

    private void OnGambleDiceSaveMaxChanged(int count)
    {
        while (_slots.Count < count)
        {
            AddSlot();
        }

        while (_slots.Count > count)
        {
            RemoveSlot();
        }
    }

    private void OnGambleDiceAdded(GambleDiceSO sO)
    {
        AddGambleDiceIcon(sO);
    }

    private void OnGambleDiceRemoved(int idx)
    {
        RemoveGambleDiceIcon(idx);
    }

    private void OnShopStarted()
    {
        _isAlwaysShow = true;
        Show();
    }

    private void OnShopEnded()
    {
        _isAlwaysShow = false;
        Hide();
    }
    #endregion

    #region GambleDiceIcon
    public void AddGambleDiceIcon(GambleDiceSO gambleDiceSO)
    {
        int idx = GetFirstEmptySlotIndex();
        if (idx == -1) return;

        var slot = _slots[idx];
        var gambleDiceIcon = Instantiate(_gambleDiceIconPrefab, slot.slotTransform);
        gambleDiceIcon.Init(gambleDiceSO, HandleGambleDiceIconClicked);
        slot.gambleDiceIcon = gambleDiceIcon;
    }

    private void HandleGambleDiceIconClicked(GambleDiceIcon gambleDiceIcon)
    {
        int idx = -1;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].gambleDiceIcon == gambleDiceIcon)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1) return;

        GambleDiceSaveManager.Instance.TryHandleGambleDiceClicked(idx);
    }

    private void RemoveGambleDiceIcon(int idx)
    {
        if (idx < 0 || idx >= _slots.Count) return;

        var slot = _slots[idx];
        if (slot.gambleDiceIcon == null) return;

        Destroy(slot.gambleDiceIcon.gameObject);
        slot.gambleDiceIcon = null;

        for (int i = idx; i < _slots.Count - 1; i++)
        {
            _slots[i].gambleDiceIcon = _slots[i + 1].gambleDiceIcon;
            _slots[i + 1].gambleDiceIcon = null;
            if (_slots[i].gambleDiceIcon == null) break;
            _slots[i].gambleDiceIcon.transform.SetParent(_slots[i].slotTransform);
            _slots[i].gambleDiceIcon.transform.localPosition = Vector3.zero;
        }
    }
    #endregion

    private int GetFirstEmptySlotIndex()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].gambleDiceIcon == null)
            {
                return i;
            }
        }

        return -1;
    }

    #region Show/Hide
    protected override void Show(Action onComplete = null)
    {
        _panel.DOAnchorPos(_showPos, AnimationFunction.DefaultDuration).SetEase(_easeType).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    protected override void Hide(Action onComplete = null)
    {
        _panel.DOAnchorPos(_hidePos, AnimationFunction.DefaultDuration).SetEase(_easeType).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    #endregion
}

public class Slot
{
    public Transform slotTransform;
    public GambleDiceIcon gambleDiceIcon;

    public Slot(Transform slotTransform = null, GambleDiceIcon gambleDiceIcon = null)
    {
        this.slotTransform = slotTransform;
        this.gambleDiceIcon = gambleDiceIcon;
    }
}