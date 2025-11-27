using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 다이스의 상호작용을 처리하는 클래스
/// </summary>
[RequireComponent(typeof(Dice))]
public class DiceInteraction : MonoBehaviour, IFocusable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private DiceHighlight _diceHighlight;
    [SerializeField] private DiceInteractionTypeDataList _dataList;

    private Dice _dice;
    private bool _isFocusing = false;
    private DiceInteractionType _interactionType;
    public DiceInteractionType InteractionType
    {
        get => _interactionType;
        set
        {
            if (_interactionType == value) return;
            _interactionType = value;
            UpdateHighlightAndInteractionInfo();
        }
    }

    private bool isInteractable = false;
    public bool IsInteractable
    {
        get => isInteractable;
        set
        {
            if (isInteractable == value) return;
            isInteractable = value;
            UpdateHighlightAndInteractionInfo();
        }
    }

    private void Awake()
    {
        _dice = GetComponent<Dice>();
    }

    private void Start()
    {
        IsInteractable = GameManager.Instance.CurrentGameState == GameState.Shop;
    }

    private void OnDisable()
    {
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.SetFocus(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.UnsetFocus(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(this);
    }

    private void UpdateHighlightAndInteractionInfo()
    {
        if (!_isFocusing || !IsInteractable)
        {
            _diceHighlight.StopHighlightCoroutine();
            InteractionInfoUIEvents.TriggerOnHideInteractionInfoUI(_dice.transform);
            return;
        }

        if (_dataList.DataDict.TryGetValue(InteractionType, out var data))
        {
            _diceHighlight.SetColor(data.color);
            _diceHighlight.StartHighlightCoroutine();
            _dice.ShowInteractionInfo();
        }
    }

    public void OnFocus()
    {
        _isFocusing = true;

        _dice.ShowToolTip();

        UpdateHighlightAndInteractionInfo();
    }

    public void OnUnfocus()
    {
        _isFocusing = false;

        ToolTipUIEvents.TriggerOnToolTipHideRequested(_dice.transform);

        UpdateHighlightAndInteractionInfo();
    }

    public void OnInteract()
    {
        if (!IsInteractable) return;

        _dice.HandleMouseClick();
        DiceInteractionEvents.TriggerOnDiceClicked(_dice);
    }
}