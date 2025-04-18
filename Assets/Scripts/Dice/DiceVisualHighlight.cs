using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceVisualHighlight : Singleton<DiceVisualHighlight>
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private Transform maskPanel;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DiceVisualHighlightTextUI textUI;
    [SerializeField] private List<DiceHighlightTypeData> highlightTypeDataList;

    private Dice targetDice;
    private float currentMaxScale;
    private float currentMinScale;


    private void Update()
    {
        HandleScale();
    }

    private void HandleScale()
    {
        if (!gameObject.activeSelf || targetDice == null) return;

        var targetScale = Mathf.PingPong(Time.time * scaleSpeed, currentMaxScale - currentMinScale) + currentMinScale;
        maskPanel.transform.localScale = new Vector3(targetScale, targetScale, 1);
    }

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (!gameObject.activeSelf || targetDice == null) return;

        transform.SetPositionAndRotation(targetDice.transform.position, targetDice.transform.rotation);
    }

    private void Start()
    {
        Hide();
        RegisterEvents();
    }

    #region Register Events
    private void RegisterEvents()
    {
        PlayerMouseManager.Instance.OnMouseOverDice += OnMouseOverDice;
        PlayerMouseManager.Instance.OnMouseExit += OnMouseExit;
    }

    private void OnMouseOverDice(Dice dice)
    {
        targetDice = dice;
        currentMaxScale = maxScale * targetDice.transform.localScale.x;
        currentMinScale = minScale * targetDice.transform.localScale.x;

        targetDice.OnIsInteractableChanged += OnIsInteractableChanged;
        targetDice.OnIsKeepedChanged += OnIsKeepedChanged;

        if (targetDice.IsInteractable)
        {
            SetHighlightType();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnMouseExit()
    {
        if (targetDice == null) return;
        targetDice.OnIsInteractableChanged -= OnIsInteractableChanged;
        targetDice.OnIsKeepedChanged -= OnIsKeepedChanged;
        targetDice = null;
        Hide();
    }

    private void OnIsInteractableChanged(bool value)
    {
        if (value)
        {
            SetHighlightType();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnIsKeepedChanged(bool isKeeped)
    {
        SetHighlightType();
    }
    #endregion

    private void SetHighlightType()
    {
        DiceHighlightType type;

        if (GameManager.Instance.CurrentGameState == GameState.Round)
        {
            type = targetDice.IsKeeped ? DiceHighlightType.Unkeep : DiceHighlightType.Keep;
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Shop)
        {
            if (targetDice is AvailityDice)
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
        if (targetDice is AvailityDice availityDice && type == DiceHighlightType.Sell)
        {
            text += "$" + availityDice.AvailityDiceSO.sellPrice.ToString();
        }

        textUI.SetText(text);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        textUI.SetTarget(targetDice.transform);
    }

    private void Hide()
    {
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