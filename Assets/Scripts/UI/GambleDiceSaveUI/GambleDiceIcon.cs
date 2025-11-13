using System;
using UnityEngine;

public class GambleDiceIcon : UIMouseHandler, IHighlightable
{
    private GambleDiceSO _gambleDiceSO;
    private bool _isShowToolTip = false;
    private DiceInteractType _interactType = DiceInteractType.None;
    private Action<GambleDiceIcon> _onClicked;

    public int SellPrice => _gambleDiceSO == null ? 0 : _gambleDiceSO.SellPrice;

    private void Start()
    {
        OnPointerEntered += () =>
        {
            ShowToolTip();
            DiceHighlightManager.Instance.ShowHighlight(this);
        };
        OnPointerExited += () =>
        {
            HideToolTip();
            DiceHighlightManager.Instance.HideHighlight();
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
                _interactType = DiceInteractType.Use;
                break;
            case GameState.Roll:
                _interactType = DiceInteractType.None;
                break;
            case GameState.Shop:
                _interactType = DiceInteractType.Sell;
                break;
            default:
                _interactType = DiceInteractType.None;
                break;
        }
    }

    private void OnDisable()
    {
        UnregisterEvents();
        HideToolTip();
        if (DiceHighlightManager.Instance == null) return;
        DiceHighlightManager.Instance.HideHighlight();
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
        _interactType = DiceInteractType.Use;
    }

    private void OnPlayEnded()
    {
        _interactType = DiceInteractType.None;
    }

    private void OnRollStarted()
    {
        _interactType = DiceInteractType.None;
    }

    private void OnRollEnded()
    {
        _interactType = DiceInteractType.Use;
    }

    private void OnShopStarted()
    {
        _interactType = DiceInteractType.Sell;
    }

    private void OnShopEnded()
    {
        _interactType = DiceInteractType.None;
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

    private void ShowToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (_isShowToolTip) return;
        if (_gambleDiceSO != null)
        {
            _isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(RectTransform, Vector2.left, _gambleDiceSO.DiceName, _gambleDiceSO.GetDescriptionText(), ToolTipTag.GambleDice);
        }
    }

    private void HideToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (!_isShowToolTip) return;
        _isShowToolTip = false;

        ToolTipUI.Instance.HideToolTip();
    }

    public void ShowHighlight()
    {
        DiceHighlightManager.Instance.ShowHighlight(this);
    }

    public DiceInteractType GetHighlightType()
    {
        return _interactType;
    }
}