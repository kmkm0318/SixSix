using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 다이스의 상호작용을 처리하는 클래스
/// </summary>
[RequireComponent(typeof(Dice))]
public class DiceInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private DiceHighlight _diceHighlight;
    [SerializeField] private DiceInteractionTypeDataList _dataList;

    private Dice _dice;
    private bool _isPointerOver = false;
    private DiceInteractionType _interactType;
    public DiceInteractionType InteractType
    {
        get => _interactType;
        set
        {
            if (_interactType == value) return;
            _interactType = value;
            UpdateInteractableState();
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
            UpdateInteractableState();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;

        _dice.ShowToolTip();

        UpdateInteractableState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;

        ToolTipUIEvents.TriggerOnToolTipHideRequested(_dice.transform);

        UpdateInteractableState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        _dice.HandleMouseClick();
        DiceInteractionEvents.TriggerOnDiceClicked(_dice);
    }

    private void UpdateInteractableState()
    {
        if (!_isPointerOver || !IsInteractable)
        {
            _diceHighlight.StopHighlightCoroutine();
            return;
        }

        if (_dataList.DataDict.TryGetValue(InteractType, out var data))
        {
            _diceHighlight.SetColor(data.color);
            _diceHighlight.StartHighlightCoroutine();
        }
    }
}