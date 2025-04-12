using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualHighlight : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DiceHighlightTextUI diceHighlightTextUI;
    [SerializeField] private List<DiceHighlightTypeData> highlightTypeData;

    private Dice dice;

    private void Update()
    {
        var targetScale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
        transform.localScale = new Vector3(targetScale, targetScale, 1);
    }

    public void Init(Dice dice)
    {
        this.dice = dice;

        dice.OnIsKeepedChanged += OnIsKeepedChanged;
        dice.DiceInteraction.OnMouseEntered += OnMouseEntered;
        dice.DiceInteraction.OnMouseExited += OnMouseExited;

        Hide();
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
            type = DiceHighlightType.Enhance;
        }
        else
        {
            type = DiceHighlightType.None;
        }

        var highlightTypeColor = highlightTypeData.Find(x => x.type == type);

        spriteRenderer.color = highlightTypeColor.color;
        diceHighlightTextUI.SetText(highlightTypeColor.text);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        diceHighlightTextUI.gameObject.SetActive(true);
    }

    private void Hide()
    {
        diceHighlightTextUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

public enum DiceHighlightType
{
    None,
    Keep,
    Unkeep,
    Enhance,
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