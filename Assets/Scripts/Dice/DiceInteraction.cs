using System;
using UnityEngine;

public class DiceInteraction : MonoBehaviour, IClickable
{
    private Dice dice;
    private DiceInteractType interactType;
    public DiceInteractType InteractType
    {
        get => interactType;
        set
        {
            if (interactType == value) return;
            interactType = value;
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

public enum DiceInteractType
{
    None,
    Keep,
    Unkeep,
    Enhance,
    Sell,
}

[Serializable]
public struct DiceInteractTypeData
{
    public DiceInteractType type;
    public Color color;
    public string text;

    public DiceInteractTypeData(DiceInteractType type, Color color, string text)
    {
        this.type = type;
        this.color = color;
        this.text = text;
    }
}