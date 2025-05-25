using System;
using UnityEngine;

public class GambleDiceIcon : UIMouseHandler, IHighlightable
{
    private GambleDiceSO gambleDiceSO;
    private bool isShowToolTip = false;
    private DiceInteractType interactType = DiceInteractType.None;

    public int SellPrice => gambleDiceSO?.SellPrice ?? 0;

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
        OnPointerClicked += () => GambleDiceSaveUI.Instance.HandleGambleDiceIconClicked(this);
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
                interactType = DiceInteractType.Use;
                break;
            case GameState.Roll:
                interactType = DiceInteractType.None;
                break;
            case GameState.Shop:
                interactType = DiceInteractType.Sell;
                break;
            default:
                interactType = DiceInteractType.None;
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
        interactType = DiceInteractType.Use;
    }

    private void OnPlayEnded()
    {
        interactType = DiceInteractType.None;
    }

    private void OnRollStarted()
    {
        interactType = DiceInteractType.None;
    }

    private void OnRollEnded()
    {
        interactType = DiceInteractType.Use;
    }

    private void OnShopStarted()
    {
        interactType = DiceInteractType.Sell;
    }

    private void OnShopEnded()
    {
        interactType = DiceInteractType.None;
    }
    #endregion

    public void Init(GambleDiceSO gambleDiceSO)
    {
        this.gambleDiceSO = gambleDiceSO;
        SetImage();
    }

    public void SetImage()
    {
        if (gambleDiceSO != null)
        {
            Image.material = new(Image.material);
            gambleDiceSO.shaderDataSO.SetMaterialProperties(Image.material);
            Image.sprite = gambleDiceSO.diceSpriteListSO.spriteList[gambleDiceSO.MaxDiceValue - 1];
        }
    }

    private void ShowToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (isShowToolTip) return;
        if (gambleDiceSO != null)
        {
            isShowToolTip = true;
            ToolTipUI.Instance.ShowToolTip(RectTransform, Vector2.left, gambleDiceSO.DiceName, gambleDiceSO.GetDescriptionText(), ToolTipTag.Gamble_Dice);
        }
    }

    private void HideToolTip()
    {
        if (ToolTipUI.Instance == null) return;
        if (!isShowToolTip) return;
        isShowToolTip = false;

        ToolTipUI.Instance.HideToolTip();
    }

    public void ShowHighlight()
    {
        DiceHighlightManager.Instance.ShowHighlight(this);
    }

    public DiceInteractType GetHighlightType()
    {
        return interactType;
    }
}