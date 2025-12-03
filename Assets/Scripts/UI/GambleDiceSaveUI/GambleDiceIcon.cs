using System;
using UnityEngine;

/// <summary>
/// 갬블 다이스 아이콘 UI 클래스
/// 갬블 다이스를 저장하고 사용 및 판매할 수 있다
/// </summary>
public class GambleDiceIcon : UIFocusHandler
{
    [SerializeField] private DiceHighlightUI _highlight;
    [SerializeField] private DiceInteractionTypeDataList _dataList;

    private GambleDiceSO _gambleDiceSO;
    private DiceInteractionType _interactionType = DiceInteractionType.None;
    public DiceInteractionType InteractionType
    {
        get => _interactionType;
        set
        {
            if (_interactionType == value) return;
            _interactionType = value;

            UpdateHighlightAndInfo();
        }
    }

    private Action<GambleDiceIcon> _onClicked;

    private void Start()
    {
        OnFocused += () =>
        {
            if (_gambleDiceSO != null)
            {
                ToolTipUIEvents.TriggerOnToolTipShowRequested(RectTransform, Vector2.left, _gambleDiceSO.DiceName, _gambleDiceSO.GetDescriptionText(), ToolTipTag.GambleDice);
            }

            UpdateHighlightAndInfo();
        };
        OnUnfocused += () =>
        {
            ToolTipUIEvents.TriggerOnToolTipHideRequested(RectTransform);
            UpdateHighlightAndInfo();
        };
        OnInteracted += () => _onClicked?.Invoke(this);
    }

    private void OnEnable()
    {
        InitInteractType();
        RegisterEvents();
        StartCoroutine(AnimationFunction.ShakeAnimation(RectTransform));
    }

    private void InitInteractType()
    {
        if (GameManager.Instance == null) return;

        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.Play:
                InteractionType = DiceInteractionType.Use;
                break;
            case GameState.Roll:
                InteractionType = DiceInteractionType.None;
                break;
            case GameState.Shop:
                InteractionType = DiceInteractionType.Sell;
                break;
            default:
                InteractionType = DiceInteractionType.None;
                break;
        }
    }

    private void OnDisable()
    {
        UnregisterEvents();

        OnPointerExit(null);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollEnded);
        GameManager.Instance.RegisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
    }

    private void UnregisterEvents()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.UnregisterEvent(GameState.Roll, OnRollStarted, OnRollEnded);
        GameManager.Instance.UnregisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
    }
    private void OnPlayStarted()
    {
        InteractionType = DiceInteractionType.Use;
    }

    private void OnPlayEnded()
    {
        InteractionType = DiceInteractionType.None;
    }

    private void OnRollStarted()
    {
        InteractionType = DiceInteractionType.None;
    }

    private void OnRollEnded()
    {
        InteractionType = DiceInteractionType.Use;
    }

    private void OnShopStarted()
    {
        InteractionType = DiceInteractionType.Sell;
    }

    private void OnShopEnded()
    {
        InteractionType = DiceInteractionType.None;
    }
    #endregion

    public void Init(GambleDiceSO gambleDiceSO, Action<GambleDiceIcon> onClicked = null)
    {
        _gambleDiceSO = gambleDiceSO;
        _onClicked = onClicked;
        SetImage();
    }

    public void SetImage()
    {
        if (_gambleDiceSO != null)
        {
            var diceSpriteListSO = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO;

            Image.material = _gambleDiceSO.shaderDataSO.imageMaterial;
            Image.sprite = diceSpriteListSO.spriteList[_gambleDiceSO.MaxDiceValue - 1];
        }
    }

    private void UpdateHighlightAndInfo()
    {
        if (!IsFocusing || InteractionType == DiceInteractionType.None)
        {
            _highlight.StopHighlightCoroutine();
            InteractionInfoUIEvents.TriggerOnHideInteractionInfoUI(RectTransform);
            return;
        }

        if (_dataList.DataDict.TryGetValue(InteractionType, out var typeData))
        {
            _highlight.SetColor(typeData.color);
            _highlight.StartHighlightCoroutine();
            InteractionInfoUIEvents.TriggerOnShowInteractionInfoUI(RectTransform, InteractionType, _gambleDiceSO.SellPrice);
        }
    }
}