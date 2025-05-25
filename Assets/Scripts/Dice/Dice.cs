using System;
using UnityEngine;

public abstract class Dice : MonoBehaviour, IHighlightable, IToolTipable
{
    [SerializeField] private DiceMovement diceMovement;
    [SerializeField] private DiceInteraction diceInteraction;
    [SerializeField] private DiceVisual diceVisual;

    private DiceFace[] faces;
    private bool isKeeped = false;
    private bool isEnabled = true;
    private int faceIndex;
    private int diceValueMax;

    public DiceFace[] Faces => faces;
    protected DiceInteractType DiceInteractType
    {
        get => diceInteraction.InteractType;
        set
        {
            if (value == DiceInteractType.Keep || value == DiceInteractType.Unkeep)
            {
                diceInteraction.InteractType = IsKeeped ? DiceInteractType.Unkeep : DiceInteractType.Keep;
            }
            else
            {
                diceInteraction.InteractType = value;
            }
        }
    }
    public bool IsKeeped
    {
        get => isKeeped;
        set
        {
            if (isKeeped == value) return;
            isKeeped = value;
            diceInteraction.InteractType = isKeeped ? DiceInteractType.Unkeep : DiceInteractType.Keep;
            OnIsKeepedChanged?.Invoke(isKeeped);
        }
    }
    public bool IsRolling => diceMovement.IsRolling;
    public bool IsInteractable
    {
        get => diceInteraction.IsInteractable;
        protected set => diceInteraction.IsInteractable = value;
    }
    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            if (isEnabled == value) return;
            isEnabled = value;

            diceVisual.SetAlpha(value ? 1 : 0.5f);
        }
    }
    public int FaceIndex => faceIndex;
    public int DiceValue => faces[faceIndex].FaceValue;
    public int DiceValueMax => diceValueMax;

    public event Action<bool> OnIsKeepedChanged;
    public event Action<bool> OnIsInteractableChanged;

    public virtual void Init(int maxValue, DiceSpriteListSO diceSpriteListSO, ShaderDataSO shaderDataSO, Playboard playboard)
    {
        if (maxValue > diceSpriteListSO.DiceFaceCount)
        {
            maxValue = diceSpriteListSO.DiceFaceCount;
            Debug.LogWarning($"Dice face count is {maxValue} but max value is {maxValue}. Set to {maxValue}.");
        }

        faces = new DiceFace[maxValue];

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, diceSpriteListSO.spriteList[i]);
        }

        faceIndex = 0;
        diceValueMax = maxValue;

        diceVisual.Initialize(shaderDataSO);
        SetFace(faceIndex);
        InitDiceInteractType();

        diceMovement.Init(playboard);
        FadeIn();
    }

    protected virtual void InitDiceInteractType()
    {
        DiceInteractType = DiceInteractType.Keep;
    }

    protected virtual void OnEnable()
    {
        RegisterEvents();
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance == null) return;

        UnregisterEvents();
    }

    #region Events
    protected virtual void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Round, OnRoundStarted, OnRoundEnded);
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
        GameManager.Instance.RegisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceCompleted);

        RollManager.Instance.OnRollPowerApplied += OnRollPowerApplied;
    }

    protected virtual void UnregisterEvents()
    {
        GameManager.Instance.UnregisterEvent(GameState.Round, OnRoundStarted, OnRoundEnded);
        GameManager.Instance.UnregisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.UnregisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
        GameManager.Instance.UnregisterEvent(GameState.Shop, OnShopStarted, OnShopEnded);
        GameManager.Instance.UnregisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceCompleted);

        RollManager.Instance.OnRollPowerApplied -= OnRollPowerApplied;
    }

    protected virtual void OnRoundStarted()
    {
        DiceInteractType = DiceInteractType.Keep;
    }

    protected virtual void OnRoundEnded()
    {

    }

    protected virtual void OnPlayStarted()
    {
        IsKeeped = false;
        diceInteraction.IsInteractable = false;
    }

    protected virtual void OnPlayEnded()
    {
        IsKeeped = false;
        diceInteraction.IsInteractable = false;
    }

    protected virtual void OnRollStarted()
    {
        diceInteraction.IsInteractable = false;
    }

    private void OnRollPowerApplied(float value)
    {
        diceMovement.OnRollPowerApplied(value);
    }

    protected virtual void OnRollCompleted()
    {
        diceInteraction.IsInteractable = DiceInteractType != DiceInteractType.Sell && (DiceInteractType == DiceInteractType.Enhance || RollManager.Instance.RollRemain > 0);
    }

    protected virtual void OnShopStarted()
    {

    }
    protected virtual void OnShopEnded()
    {

    }

    protected virtual void OnEnhanceStarted()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Dice)
        {
            OnDiceEnhanceStarted();
        }
        else if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            OnHandEnhanceStarted();
        }
    }

    protected virtual void OnEnhanceCompleted()
    {
        if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Dice)
        {
            OnDiceEnhanceCompleted();
        }
        else if (EnhanceManager.Instance.CurrentEnhanceType == EnhanceType.Hand)
        {
            OnHandEnhanceCompleted();
        }
    }

    protected virtual void OnDiceEnhanceStarted()
    {

    }

    protected virtual void OnDiceEnhanceCompleted()
    {

    }

    protected virtual void OnHandEnhanceStarted()
    {

    }

    protected virtual void OnHandEnhanceCompleted()
    {

    }
    #endregion

    #region Faces
    public virtual void SetFace(int faceIndex)
    {
        diceVisual.SetSprite(faces[faceIndex].CurrentSprite);
        diceVisual.SetColor(faces[faceIndex].EnhanceValue);
    }

    protected virtual void ChangeFace(int value = 1)
    {
        faceIndex += value;
        faceIndex %= diceValueMax;
        SetFace(faceIndex);
    }
    #endregion

    #region Handlers
    public virtual void HandleMouseClick()
    {
        if (!IsInteractable) return;

        if (DiceInteractType == DiceInteractType.Keep || DiceInteractType == DiceInteractType.Unkeep)
        {
            if (DiceManager.Instance.IsKeepable)
            {
                IsKeeped = !IsKeeped;
            }
        }

        DiceManager.Instance.HandleDiceClick(this);
    }

    public virtual void HandleIsInteractable(bool isInteractable)
    {
        OnIsInteractableChanged?.Invoke(isInteractable);
    }

    public virtual void HandleDiceCollided()
    {
        if (IsRolling && !IsKeeped)
        {
            ChangeFace(1);
        }
    }
    #endregion

    public void ShowHighlight()
    {
        if (UtilityFunctions.IsPointerOverUIElement()) return;
        DiceHighlightManager.Instance.ShowHighlight(this);
    }

    public virtual DiceInteractType GetHighlightType()
    {
        return DiceInteractType;
    }

    public virtual void EnhanceDice(ScorePair scorePair)
    {
        faces[faceIndex].Enhance(scorePair);
        SequenceManager.Instance.AddCoroutine(() => diceVisual.SetColor(faces[faceIndex].EnhanceValue), true);
        AnimationFunction.AddShakeAnimation(transform, false, true);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public abstract void ShowToolTip();

    public void FadeIn(Action onComplete = null)
    {
        diceVisual.FadeIn(onComplete);
    }

    public void FadeOut(Action onComplete = null)
    {
        diceVisual.FadeOut(onComplete);
    }
}
