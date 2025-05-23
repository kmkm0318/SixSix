using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GambleDiceSaveUI : Singleton<GambleDiceSaveUI>
{
    [SerializeField] private UIMouseHandler panel;
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private Transform slotParent;
    [SerializeField] private Transform slotPrefab;
    [SerializeField] private GambleDiceIcon gambleDiceIconPrefab;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private Vector3 showPos;
    private List<Slot> slots = new();

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        showPos = panelRectTransform.anchoredPosition;
        panelRectTransform.anchoredPosition = hidePos;
        panel.OnPointerEntered += () => Show();
        panel.OnPointerExited += () => Hide();

        for (int i = 0; i < DataContainer.Instance.CurrentDiceStat.defaultGambleDiceSaveMax; i++)
        {
            AddSlot();
        }
    }

    #region Slots
    private void AddSlot()
    {
        Transform newSlot = Instantiate(slotPrefab, slotParent);
        slots.Add(new(newSlot));
        newSlot.gameObject.SetActive(true);
    }

    private void RemoveSlot()
    {
        if (slots.Count == 0) return;
        Transform lastSlot = slots[^1].slotTransform;
        slots.RemoveAt(slots.Count - 1);
        Destroy(lastSlot.gameObject);
    }
    #endregion

    private void RegisterEvents()
    {
        GambleDiceSaveManager.Instance.OnGambleDiceSaveMaxChanged += OnGambleDiceSaveMaxChanged;
        GambleDiceSaveManager.Instance.OnGambleDiceAdded += OnGambleDiceAdded;
        GambleDiceSaveManager.Instance.OnGambleDiceRemoved += OnGambleDiceRemoved;
    }

    private void OnGambleDiceSaveMaxChanged(int count)
    {
        while (slots.Count < count)
        {
            AddSlot();
        }

        while (slots.Count > count)
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

    #region GambleDiceIcon
    public void AddGambleDiceIcon(GambleDiceSO gambleDiceSO)
    {
        int idx = GetFirstEmptySlotIndex();
        if (idx == -1) return;

        var slot = slots[idx];
        var gambleDiceIcon = Instantiate(gambleDiceIconPrefab, slot.slotTransform);
        gambleDiceIcon.Init(gambleDiceSO);
        slot.gambleDiceIcon = gambleDiceIcon;
    }

    private void RemoveGambleDiceIcon(int idx)
    {
        if (idx < 0 || idx >= slots.Count) return;

        var slot = slots[idx];
        if (slot.gambleDiceIcon == null) return;

        Destroy(slot.gambleDiceIcon.gameObject);
        slot.gambleDiceIcon = null;

        for (int i = idx; i < slots.Count - 1; i++)
        {
            slots[i].gambleDiceIcon = slots[i + 1].gambleDiceIcon;
            slots[i + 1].gambleDiceIcon = null;
            if (slots[i].gambleDiceIcon == null) break;
            slots[i].gambleDiceIcon.transform.SetParent(slots[i].slotTransform);
            slots[i].gambleDiceIcon.transform.localPosition = Vector3.zero;
        }
    }
    #endregion

    private int GetFirstEmptySlotIndex()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gambleDiceIcon == null)
            {
                return i;
            }
        }

        return -1;
    }

    public void HandleGambleDiceIconClicked(GambleDiceIcon gambleDiceIcon)
    {
        int idx = -1;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].gambleDiceIcon == gambleDiceIcon)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1) return;

        GambleDiceSaveManager.Instance.TryGenerateGambleDice(idx);
    }

    #region Show/Hide
    private void Show(Action onComplete = null)
    {
        panelRectTransform.DOAnchorPos(showPos, AnimationFunction.DefaultDuration).SetEase(easeType).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    private void Hide(Action onComplete = null)
    {
        panelRectTransform.DOAnchorPos(hidePos, AnimationFunction.DefaultDuration).SetEase(easeType).OnComplete(() =>
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