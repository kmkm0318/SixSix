using UnityEngine;

public class DiceInteraction : MonoBehaviour, IClickable
{
    private Dice dice;

    private bool isInteractable = false;
    public bool IsInteractable
    {
        get => isInteractable;
        set
        {
            if (isInteractable == value) return;
            isInteractable = value;
            dice.HandleIsInteractable(isInteractable);
            if (value && isMouseOver)
            {
                dice.HandleMouseEnter();
            }
            else if (!value && isMouseOver)
            {
                dice.HandleMouseExit();
            }
        }
    }
    private bool isMouseOver = false;
    public bool IsMouseOver
    {
        get => isMouseOver;
        private set
        {
            if (isMouseOver == value) return;
            isMouseOver = value;
            dice.HandleMouseOver(isMouseOver);
        }
    }

    private void Awake()
    {
        dice = GetComponent<Dice>();
    }

    private void Start()
    {
        IsInteractable = GameManager.Instance.CurrentGameState == GameState.Shop;
    }

    private void OnDisable()
    {
        IsMouseOver = false;
    }

    void OnMouseEnter()
    {
        IsMouseOver = true;

        if (!IsInteractable) return;
        if (Functions.IsPointerOverUIElement()) return;

        dice.HandleMouseEnter();
    }

    void OnMouseExit()
    {
        IsMouseOver = false;

        if (!IsInteractable) return;

        dice.HandleMouseExit();
    }

    public void OnClick()
    {
        if (!IsInteractable) return;
        if (Functions.IsPointerOverUIElement()) return;

        dice.HandleMouseClick();
    }
}
