using System;
using UnityEngine;

public abstract class Dice : MonoBehaviour, IHighlightable, IToolTipable
{
    [SerializeField] private DiceVisual diceVisual;
    [SerializeField] private DiceMovement diceMovement;
    [SerializeField] private DiceInteraction diceInteraction;

    public event Action<bool> OnIsKeepedChanged;
    public event Action<bool> OnIsInteractableChanged;
    public event Action OnMouseClicked;

    private bool isKeeped = false;
    public bool IsKeeped
    {
        get => isKeeped;
        protected set
        {
            if (isKeeped == value) return;
            isKeeped = value;
            OnIsKeepedChanged?.Invoke(isKeeped);
        }
    }
    public bool IsRolling => diceMovement.IsRolling;
    public bool IsInteractable
    {
        get => diceInteraction.IsInteractable;
        protected set => diceInteraction.IsInteractable = value;
    }

    private DiceFace[] faces;
    public DiceFace[] Faces => faces;
    private int faceIndex;
    public int FaceIndex => faceIndex;
    private int faceIndexMax;

    public virtual void Init(int maxValue, DiceFaceSpriteListSO diceFaceSpriteListSO, Playboard playboard)
    {
        faces = new DiceFace[maxValue];

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, diceFaceSpriteListSO.diceFaceList[i]);
        }

        faceIndex = 0;
        faceIndexMax = maxValue;

        SetFace(faceIndex);

        diceMovement.Init(playboard);
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
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        PlayManager.Instance.OnPlayEnded += OnPlayEnded;
        RollManager.Instance.OnRollStarted += OnRollStarted;
        RollManager.Instance.OnRollPowerApplied += OnRollPowerApplied;
        RollManager.Instance.OnRollCompleted += OnRollCompleted;
        ShopManager.Instance.OnShopStarted += OnShopStarted;
        ShopManager.Instance.OnShopEnded += OnShopEnded;
    }

    protected virtual void UnregisterEvents()
    {
        PlayManager.Instance.OnPlayStarted -= OnPlayStarted;
        PlayManager.Instance.OnPlayEnded -= OnPlayEnded;
        RollManager.Instance.OnRollStarted -= OnRollStarted;
        RollManager.Instance.OnRollPowerApplied -= OnRollPowerApplied;
        RollManager.Instance.OnRollCompleted -= OnRollCompleted;
        ShopManager.Instance.OnShopStarted -= OnShopStarted;
        ShopManager.Instance.OnShopEnded -= OnShopEnded;
    }

    protected virtual void OnPlayStarted(int playRemain)
    {
        IsKeeped = false;
        diceInteraction.IsInteractable = false;
    }

    protected virtual void OnPlayEnded(int obj)
    {
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
        diceInteraction.IsInteractable = RollManager.Instance.RollRemain > 0;
    }

    protected abstract void OnShopStarted();
    protected abstract void OnShopEnded();
    #endregion

    #region Faces
    public virtual void SetFace(int faceIndex)
    {
        diceVisual.SetSprite(faces[faceIndex].FaceSpriteSO.sprite);
        diceVisual.SetColor(faces[faceIndex].EnhanceValue);
    }

    public virtual void ChangeFace(int value = 1)
    {
        faceIndex += value;
        faceIndex %= faceIndexMax;
        SetFace(faceIndex);
    }
    #endregion

    #region Handlers
    public virtual void HandleMouseClick()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            IsKeeped = !IsKeeped;
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

    public virtual DiceHighlightType GetHighlightType()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            return IsKeeped ? DiceHighlightType.Unkeep : DiceHighlightType.Keep;
        }
        else
        {
            return DiceHighlightType.None;
        }
    }

    public virtual void EnhanceDice(ScorePair scorePair)
    {
        faces[faceIndex].Enhance(scorePair);
        diceVisual.SetColor(faces[faceIndex].EnhanceValue);
        SequenceManager.Instance.AddCoroutine(AnimationManager.Instance.PlayShakeAnimation(transform), true);
    }

    public abstract void ShowToolTip();
}
