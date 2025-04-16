using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualHighlight : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DiceVisualHighlightTextUI textUI;
    [SerializeField] private List<DiceHighlightTypeData> highlightTypeDataList;

    private Dice dice;

    private void Update()
    {
        var targetScale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
        transform.localScale = new Vector3(targetScale, targetScale, 1);
    }

    public void Init(Dice dice)
    {
        if (this.dice != null)
        {
            UnregisterEvents();
        }
        this.dice = dice;

        RegisterEvents();

        Hide();
    }

    private void RegisterEvents()
    {
        dice.OnIsKeepedChanged += OnIsKeepedChanged;
        dice.DiceInteraction.OnMouseEntered += OnMouseEntered;
        dice.DiceInteraction.OnMouseExited += OnMouseExited;
    }

    private void UnregisterEvents()
    {
        dice.OnIsKeepedChanged -= OnIsKeepedChanged;
        dice.DiceInteraction.OnMouseEntered -= OnMouseEntered;
        dice.DiceInteraction.OnMouseExited -= OnMouseExited;
    }

    private void OnIsKeepedChanged(bool isKeeped)
    {
        SetHighlightType();
    }

    private void OnMouseEntered()
    {
        SetHighlightType();
        Show();
    }

    private void OnMouseExited()
    {
        Hide();
    }

    private void SetHighlightType()
    {
        DiceHighlightType type;

        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            type = dice.IsKeeped ? DiceHighlightType.Unkeep : DiceHighlightType.Keep;
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            if (dice is AvailityDice)
            {
                type = DiceHighlightType.Sell;
            }
            else
            {
                type = DiceHighlightType.Enhance;
            }
        }
        else
        {
            type = DiceHighlightType.None;
        }

        var highlightTypeData = highlightTypeDataList.Find(x => x.type == type);

        spriteRenderer.color = highlightTypeData.color;

        string text = highlightTypeData.text;
        if (dice is AvailityDice availityDice && type == DiceHighlightType.Sell)
        {
            text += "$" + availityDice.AvailityDiceSO.sellPrice.ToString();
        }

        textUI.SetText(text);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        textUI.gameObject.SetActive(true);
    }

    private void Hide()
    {
        textUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

public enum DiceHighlightType
{
    None,
    Keep,
    Unkeep,
    Enhance,
    Sell,
}

[Serializable]
public struct DiceHighlightTypeData
{
    public DiceHighlightType type;
    public Color color;
    public string text;

    public DiceHighlightTypeData(DiceHighlightType type, Color color, string text)
    {
        this.type = type;
        this.color = color;
        this.text = text;
    }
}