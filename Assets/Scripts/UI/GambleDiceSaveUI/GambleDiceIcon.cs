using System;
using UnityEngine;

/// <summary>
/// 갬블 다이스 아이콘 UI 클래스
/// 갬블 다이스를 저장하고 사용 및 판매할 수 있다
/// </summary>
public class GambleDiceIcon : UIMouseHandler
{
    [SerializeField] private DiceHighlightUI _highlight;
    [SerializeField] private DiceInteractionTypeDataList _dataList;

    private GambleDiceSO _gambleDiceSO;
    private DiceInteractionType _interactType = DiceInteractionType.None;
    public DiceInteractionType InteractType
    {
        get => _interactType;
        set
        {
            if (_interactType == value) return;
            _interactType = value;

            UpdateHighlightState();
        }
    }

    private Action<GambleDiceIcon> _onClicked;

    private void Start()
    {
        OnPointerEntered += () =>
        {
            if (_gambleDiceSO != null)
            {
                ToolTipUIEvents.TriggerOnToolTipShowRequested(RectTransform, Vector2.left, _gambleDiceSO.DiceName, _gambleDiceSO.GetDescriptionText(), ToolTipTag.GambleDice);
            }

            UpdateHighlightState();
        };
        OnPointerExited += () =>
        {
            ToolTipUIEvents.TriggerOnToolTipHideRequested(RectTransform);
            UpdateHighlightState();
        };
        OnPointerClicked += () => _onClicked?.Invoke(this);
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
                _interactType = DiceInteractionType.Use;
                break;
            case GameState.Roll:
                _interactType = DiceInteractionType.None;
                break;
            case GameState.Shop:
                _interactType = DiceInteractionType.Sell;
                break;
            default:
                _interactType = DiceInteractionType.None;
                break;
        }
    }

    private void OnDisable()
    {
        UnregisterEvents();
        ToolTipUIEvents.TriggerOnToolTipHideRequested(RectTransform);

        _highlight.StopHighlightCoroutine();
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
        _interactType = DiceInteractionType.Use;
    }

    private void OnPlayEnded()
    {
        _interactType = DiceInteractionType.None;
    }

    private void OnRollStarted()
    {
        _interactType = DiceInteractionType.None;
    }

    private void OnRollEnded()
    {
        _interactType = DiceInteractionType.Use;
    }

    private void OnShopStarted()
    {
        _interactType = DiceInteractionType.Sell;
    }

    private void OnShopEnded()
    {
        _interactType = DiceInteractionType.None;
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

            Image.material = new(Image.material);
            _gambleDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = diceSpriteListSO.spriteList[_gambleDiceSO.MaxDiceValue - 1];
        }
    }

    private void UpdateHighlightState()
    {
        if (!IsPointerOver || _interactType == DiceInteractionType.None)
        {
            _highlight.StopHighlightCoroutine();
            return;
        }

        if (_dataList.DataDict.TryGetValue(_interactType, out var typeData))
        {
            _highlight.SetColor(typeData.color);
            _highlight.StartHighlightCoroutine();
        }
    }
}