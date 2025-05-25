using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class DiceHighlightManager : Singleton<DiceHighlightManager>
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private DiceHighlightSpriteRenderer highlightSpriteRenderer;
    [SerializeField] private DiceHighlightImage highlightImage;
    [SerializeField] private DiceHighlightTextUI textUI;
    [SerializeField] private List<DiceInteractTypeData> highlightTypeDataList;

    private IHighlightable currentTarget;
    private Dice targetDice;
    private GambleDiceIcon targetGambleDiceIcon;
    private DiceInteractType currentHighlightType;
    private Color highlightColor;

    private void Start()
    {
        RegisterEvents();
        Hide();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        MouseManager.Instance.OnMouseExited += OnMouseExited;
        MouseManager.Instance.OnMouseClicked += Refresh;
        GameManager.Instance.OnGameStateChanged += Refresh;
    }

    private void OnMouseExited()
    {
        Hide();
        UnsetHighlightTarget();
    }

    private void Refresh()
    {
        IHighlightable tmp = currentTarget;
        UnsetHighlightTarget();
        Hide();
        if (tmp != null)
        {
            ShowHighlight(tmp);
        }
    }
    #endregion

    public void HideHighlight()
    {
        UnsetHighlightTarget();
        Hide();
    }

    public void ShowHighlight(IHighlightable highlightable)
    {
        UnsetHighlightTarget();
        Hide();

        currentTarget = highlightable;
        SetHighlightType();
        if (currentHighlightType == DiceInteractType.None) return;

        if (highlightable is Dice dice)
        {
            SetHighlightTarget(dice);
        }
        else if (highlightable is GambleDiceIcon gambleDiceIcon)
        {
            SetHighlightTarget(gambleDiceIcon);
        }
        else
        {
            Debug.LogWarning("Unsupported highlightable type: " + highlightable.GetType());
            UnsetHighlightTarget();
            currentTarget = null;
        }

        Show();
    }

    private void SetHighlightTarget(Dice dice)
    {
        targetDice = dice;

        float currentMinScale = minScale * targetDice.transform.localScale.x;
        float currentMaxScale = maxScale * targetDice.transform.localScale.x;

        highlightSpriteRenderer.SetTarget(targetDice.transform, highlightColor, currentMinScale, currentMaxScale, scaleSpeed);

        targetDice.OnIsInteractableChanged += OnIsInteractableChanged;
        targetDice.OnIsKeepedChanged += OnIsKeepedChanged;
    }

    private void SetHighlightTarget(GambleDiceIcon gambleDiceIcon)
    {
        targetGambleDiceIcon = gambleDiceIcon;
        highlightImage.SetTarget(gambleDiceIcon.GetComponent<RectTransform>(), highlightColor, minScale, maxScale, scaleSpeed);
    }

    public void UnsetHighlightTarget()
    {
        if (currentTarget == null) return;
        currentTarget = null;

        if (targetDice != null)
        {
            targetDice.OnIsInteractableChanged -= OnIsInteractableChanged;
            targetDice.OnIsKeepedChanged -= OnIsKeepedChanged;
            targetDice = null;
        }

        if (targetGambleDiceIcon != null)
        {
            targetGambleDiceIcon = null;
        }
    }

    private void OnIsInteractableChanged(bool value)
    {
        if (value)
        {
            SetHighlightType();

            if (currentHighlightType != DiceInteractType.None)
            {
                Show();
            }
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

    private DiceInteractTypeData? GetHighlightTypeData(DiceInteractType type)
    {
        return highlightTypeDataList.Find(x => x.type == type);
    }

    private void SetHighlightType()
    {
        currentHighlightType = currentTarget.GetHighlightType();
        if (currentHighlightType == DiceInteractType.None) return;

        var highlightTypeData = GetHighlightTypeData(currentHighlightType);

        if (highlightTypeData.HasValue)
        {
            highlightColor = highlightTypeData.Value.color;

            string text = highlightTypeData.Value.Text;
            SetHighlightText(text);
        }
        else
        {
            Debug.LogWarning($"No highlight type data found for type: {currentHighlightType}");
        }
    }

    private void SetHighlightText(string text)
    {
        if (currentHighlightType == DiceInteractType.Sell)
        {
            if (currentTarget is AbilityDice abilityDice)
            {
                text = string.Format(text, abilityDice.SellPrice);
            }
            else if (currentTarget is GambleDiceIcon gambleDiceIcon)
            {
                text = string.Format(text, gambleDiceIcon.SellPrice);
            }
            else
            {
                Debug.LogWarning("Dice does not have a sell price.");
                text = string.Empty;
            }
        }

        textUI.SetText(text);
    }

    private void Show()
    {
        if (targetDice != null)
        {
            if (!targetDice.IsInteractable) return;
            textUI.SetTargetAndOffset(targetDice.transform);
            textUI.Show();
            highlightSpriteRenderer.Show();
        }
        else if (targetGambleDiceIcon != null)
        {
            textUI.SetTargetAndOffset(targetGambleDiceIcon.RectTransform);
            textUI.Show();
            highlightImage.Show();
        }
    }

    private void Hide()
    {
        textUI.Hide();
        highlightSpriteRenderer.Hide();
        highlightImage.Hide();
    }
}


[Serializable]
public struct DiceInteractTypeData
{
    public DiceInteractType type;
    public Color color;
    public LocalizedString localizedText;
    public readonly string Text => localizedText.GetLocalizedString();
}