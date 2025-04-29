using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DiceHighlight : Singleton<DiceHighlight>
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private Transform maskPanel;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DiceHighlightTextUI textUI;
    [SerializeField] private List<DiceInteractTypeData> highlightTypeDataList;

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
        RegisterEvents();
        Hide();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        PlayerMouseManager.Instance.OnMouseExit += HideHighlight;
    }
    #endregion

    public void ShowHighlight(Dice dice)
    {
        if (targetDice != null)
        {
            HideHighlight();
        }

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

    public void HideHighlight()
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

    private void SetHighlightType()
    {
        DiceInteractType type = targetDice.GetHighlightType();

        var highlightTypeData = highlightTypeDataList.Find(x => x.type == type);

        spriteRenderer.color = highlightTypeData.color;

        string text = highlightTypeData.text;
        if (type == DiceInteractType.Sell)
        {
            if (targetDice.TryGetComponent(out AvailityDice availityDice))
            {
                text += $"(${availityDice.SellPrice})";
            }
        }

        textUI.SetText(text);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        textUI.SetTargetAndOffset(targetDice.transform);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}