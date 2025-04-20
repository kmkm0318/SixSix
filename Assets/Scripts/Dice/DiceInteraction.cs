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

    public void OnClick()
    {
        if (!IsInteractable) return;
        if (Functions.IsPointerOverUIElement()) return;

        dice.HandleMouseClick();
    }
}
