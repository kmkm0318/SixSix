using System;
using UnityEngine;

public abstract class Dice : MonoBehaviour, IHighlightable, IToolTipable
{
    [SerializeField] private DiceVisual diceVisual;
    [SerializeField] private DiceMovement diceMovement;
    [SerializeField] private DiceInteraction diceInteraction;
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

    public event Action<bool> OnIsKeepedChanged;
    public event Action<bool> OnIsInteractableChanged;
    public event Action OnMouseClicked;

    private bool isKeeped = false;
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

    private bool isEnabled = true;
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

    private DiceFace[] faces;
    public DiceFace[] Faces => faces;
    private int faceIndex;
    public int FaceIndex => faceIndex;
    public int FaceValue => faceIndex + 1;

    private int faceValueMax;
    public int FaceValueMax => faceValueMax;

    public virtual void Init(int maxValue, DiceFaceSpriteListSO diceFaceSpriteListSO, Playboard playboard)
    {
        faces = new DiceFace[maxValue];

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, diceFaceSpriteListSO.diceFaceList[i]);
        }

        faceIndex = 0;
        faceValueMax = maxValue;

        SetFace(faceIndex);
        InitDiceInteractType();

        diceMovement.Init(playboard);
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
        if (PlayManager.Instance == null) return;
        if (RollManager.Instance == null) return;
        if (ShopManager.Instance == null) return;

        UnregisterEvents();
    }

    #region Events
    protected virtual void RegisterEvents()
    {
        RoundManager.Instance.OnRoundStarted += OnRoundStarted;
        RoundManager.Instance.OnRoundCleared += OnRoundCleared;
        RoundManager.Instance.OnRoundFailed += OnRoundFailed;
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        PlayManager.Instance.OnPlayEnded += OnPlayEnded;
        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollPowerApplied += OnRollPowerApplied;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
        ShopManager.Instance.OnShopStarted += OnShopStarted;
        ShopManager.Instance.OnShopEnded += OnShopEnded;
        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted += OnDiceEnhanceCompleted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted += OnHandEnhanceCompleted;
    }

    protected virtual void UnregisterEvents()
    {
        RoundManager.Instance.OnRoundStarted -= OnRoundStarted;
        RoundManager.Instance.OnRoundCleared -= OnRoundCleared;
        RoundManager.Instance.OnRoundFailed -= OnRoundFailed;
        PlayManager.Instance.OnPlayStarted -= OnPlayStarted;
        PlayManager.Instance.OnPlayEnded -= OnPlayEnded;
        RollManager.Instance.OnRollStarted -= OnRollStarted;
        RollManager.Instance.OnRollPowerApplied -= OnRollPowerApplied;
        RollManager.Instance.OnRollCompleted -= OnRollCompleted;
        ShopManager.Instance.OnShopStarted -= OnShopStarted;
        ShopManager.Instance.OnShopEnded -= OnShopEnded;
        EnhanceManager.Instance.OnDiceEnhanceStarted -= OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnDiceEnhanceCompleted -= OnDiceEnhanceCompleted;
        EnhanceManager.Instance.OnHandEnhanceStarted -= OnHandEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceCompleted -= OnHandEnhanceCompleted;
    }

    protected virtual void OnRoundStarted(int round)
    {
        DiceInteractType = DiceInteractType.Keep;
    }

    protected virtual void OnRoundCleared(int round)
    {

    }

    protected virtual void OnRoundFailed(int round)
    {

    }

    protected virtual void OnPlayStarted(int playRemain)
    {
        IsKeeped = false;
        diceInteraction.IsInteractable = false;
    }

    protected virtual void OnPlayEnded(int obj)
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

    public void ResetMouseClickEvent()
    {
        OnMouseClicked = null;
    }
    #endregion

    #region Faces
    public virtual void SetFace(int faceIndex)
    {
        diceVisual.SetSprite(faces[faceIndex].FaceSpriteSO.sprite);
        diceVisual.SetColor(faces[faceIndex].EnhanceValue);
    }

    protected virtual void ChangeFace(int value = 1)
    {
        faceIndex += value;
        faceIndex %= faceValueMax;
        SetFace(faceIndex);
    }
    #endregion

    #region Handlers
    public virtual void HandleMouseClick()
    {
        if (!IsInteractable) return;

        if (DiceInteractType == DiceInteractType.Keep || DiceInteractType == DiceInteractType.Unkeep)
        {
            if (PlayerDiceManager.Instance.IsKeepable)
            {
                IsKeeped = !IsKeeped;
            }
        }

        OnMouseClicked?.Invoke();
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
        if (Functions.IsPointerOverUIElement()) return;
        DiceHighlight.Instance.ShowHighlight(this);
    }

    public virtual DiceInteractType GetHighlightType()
    {
        return DiceInteractType;
    }

    public virtual void EnhanceDice(ScorePair scorePair)
    {
        faces[faceIndex].Enhance(scorePair);
        diceVisual.SetColor(faces[faceIndex].EnhanceValue);
        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayShakeAnimation(transform), true);
    }

    public abstract void ShowToolTip();
}
