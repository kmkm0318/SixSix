using System;
using UnityEngine;

public abstract class Dice : MonoBehaviour
{
    [SerializeField] private DiceMovement diceMovement;
    [SerializeField] private DiceInteraction diceInteraction;
    [SerializeField] private DiceVisual diceVisual;

    private DiceFace[] _faces;
    private bool _isKeeped = false;
    private bool _isEnabled = true;
    private int _faceIndex;
    private int _diceValueMax;

    public DiceFace[] Faces => _faces;
    protected DiceInteractionType DiceInteractionType
    {
        get => diceInteraction.InteractionType;
        set
        {
            if (value == DiceInteractionType.Keep || value == DiceInteractionType.Unkeep)
            {
                diceInteraction.InteractionType = IsKeeped ? DiceInteractionType.Unkeep : DiceInteractionType.Keep;
            }
            else
            {
                diceInteraction.InteractionType = value;
            }
        }
    }
    public bool IsKeeped
    {
        get => _isKeeped;
        set
        {
            if (_isKeeped == value) return;
            _isKeeped = value;
            diceInteraction.InteractionType = _isKeeped ? DiceInteractionType.Unkeep : DiceInteractionType.Keep;
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
        get => _isEnabled;
        set
        {
            if (_isEnabled == value) return;
            _isEnabled = value;

            diceVisual.SetAlpha(value ? 1 : 0.5f);
        }
    }
    public int FaceIndex => _faceIndex;
    public int DiceValue => _faces[_faceIndex].FaceValue;
    public int DiceValueMax => _diceValueMax;

    public virtual void Init(int maxValue, DiceSpriteListSO diceSpriteListSO, ShaderDataSO shaderDataSO, Playboard playboard)
    {
        if (maxValue > diceSpriteListSO.DiceFaceCount)
        {
            maxValue = diceSpriteListSO.DiceFaceCount;
            Debug.LogWarning($"Dice face count is {maxValue} but max value is {maxValue}. Set to {maxValue}.");
        }

        _faces = new DiceFace[maxValue];

        for (int i = 0; i < _faces.Length; i++)
        {
            _faces[i] = new();
            _faces[i].Init(i + 1, diceSpriteListSO.spriteList[i]);
        }

        _faceIndex = 0;
        _diceValueMax = maxValue;

        diceVisual.Initialize(shaderDataSO);
        SetFace(_faceIndex);
        InitDiceInteractType();

        diceMovement.Init(playboard);
        FadeIn();
    }

    protected virtual void InitDiceInteractType()
    {
        DiceInteractionType = DiceInteractionType.Keep;
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
        DiceInteractionType = DiceInteractionType.Keep;
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
        diceInteraction.IsInteractable = DiceInteractionType != DiceInteractionType.Sell && (DiceInteractionType == DiceInteractionType.Enhance || RollManager.Instance.RollRemain > 0);
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
        diceVisual.SetSprite(_faces[faceIndex].CurrentSprite);
        diceVisual.SetColor(_faces[faceIndex].EnhanceValue);
    }

    protected virtual void ChangeFace(int value = 1)
    {
        _faceIndex += value;
        _faceIndex %= _diceValueMax;
        SetFace(_faceIndex);
    }
    #endregion

    #region Handlers
    public virtual void HandleMouseClick()
    {
        if (!IsInteractable) return;

        if (DiceInteractionType == DiceInteractionType.Keep || DiceInteractionType == DiceInteractionType.Unkeep)
        {
            if (DiceManager.Instance.IsKeepable)
            {
                IsKeeped = !IsKeeped;
            }
        }

        DiceManager.Instance.HandleDiceClick(this);
    }

    public virtual void HandleDiceCollided()
    {
        if (IsRolling)
        {
            AudioManager.Instance.PlaySFX(SFXType.DiceCollide);
            if (!IsKeeped)
            {
                ChangeFace(1);
            }
        }
    }
    #endregion

    public virtual void EnhanceDice(ScorePair enhanceValue)
    {
        SequenceManager.Instance.AddCoroutine(() => _faces[_faceIndex].Enhance(enhanceValue), true);
        SequenceManager.Instance.AddCoroutine(() => diceVisual.SetColor(_faces[_faceIndex].EnhanceValue), true);
        TriggerAnimationManager.Instance.PlayTriggerScoreAnimation(transform, Vector3.up, enhanceValue);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    public abstract void ShowToolTip();

    public virtual void ShowInteractionInfo()
    {
        InteractionInfoUIEvents.TriggerOnShowInteractionInfoUI(transform, DiceInteractionType);
    }

    public void FadeIn(Action onComplete = null)
    {
        diceVisual.FadeIn(onComplete);
    }

    public void FadeOut(Action onComplete = null)
    {
        diceVisual.FadeOut(onComplete);
    }
}
