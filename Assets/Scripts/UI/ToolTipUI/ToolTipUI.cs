using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private LabeledValuePanel labelPanel;
    [SerializeField] private RectTransform labelPanelRectTransform;
    [SerializeField] private TextPanel tagPanel;
    [SerializeField] private List<TagData> tagDataList;
    [SerializeField] private float offset;

    private RectTransform rectTransform;
    private Transform targetTransform;
    private Vector3 targetOffset;
    private Vector3 offsetDirection;
    bool isUI = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        RegisterEvents();
        HideToolTip();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        if (MouseManager.Instance) MouseManager.Instance.OnMouseExited += OnMouseExit;

        ShopUIEvents.OnShopUIShown += HideToolTip;
        RoundClearUIEvents.OnRoundClearUIShown += HideToolTip;

        ToolTipUIEvents.OnToolTipShowRequested += ShowToolTip;
        ToolTipUIEvents.OnToolTipHideRequested += HideToolTip;
    }

    private void UnregisterEvents()
    {
        if (MouseManager.Instance) MouseManager.Instance.OnMouseExited += OnMouseExit;

        ShopUIEvents.OnShopUIShown -= HideToolTip;
        RoundClearUIEvents.OnRoundClearUIShown -= HideToolTip;

        ToolTipUIEvents.OnToolTipShowRequested -= ShowToolTip;
        ToolTipUIEvents.OnToolTipHideRequested -= HideToolTip;
    }

    private void OnMouseExit()
    {
        if (isUI) return;
        HideToolTip();
    }
    #endregion

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (targetTransform == null) return;

        Vector3 targetPos;
        if (isUI)
        {
            targetPos = targetTransform.position + targetOffset;
        }
        else
        {
            targetPos = Camera.main.WorldToScreenPoint(targetTransform.position + targetOffset);
        }

        rectTransform.position = targetPos + offsetDirection * offset;
    }

    public void ShowToolTip(Transform transform, Vector2 direction, string name, string description, ToolTipTag tag = ToolTipTag.None, AbilityDiceRarity rarity = AbilityDiceRarity.Normal)
    {
        if (transform == null) return;
        if (transform is RectTransform rect)
        {
            isUI = true;
            targetOffset = rect.rect.width / 2 * direction;
        }
        else
        {
            if (UtilityFunctions.IsPointerOverUIElement()) return;
            isUI = false;
            targetOffset = transform.localScale.x / 2 * direction;
        }
        offsetDirection = direction.normalized;
        gameObject.SetActive(true);
        SetPanelAnchor(direction);
        targetTransform = transform;

        labelPanel.SetLabel(name, true);
        labelPanel.SetValue(description);
        HandleTag(tag, rarity);
    }

    #region HandleTag
    private void HandleTag(ToolTipTag tag, AbilityDiceRarity rarity)
    {
        if (tag == ToolTipTag.None)
        {
            tagPanel.gameObject.SetActive(false);
            return;
        }

        tagPanel.gameObject.SetActive(true);

        TagData tagData;

        if (tag == ToolTipTag.AbilityDice)
        {
            tagData = tagDataList.Find(data => data.Tag == tag && data.Rarity == rarity);
        }
        else
        {
            tagData = tagDataList.Find(data => data.Tag == tag);
        }

        if (tagData != null)
        {
            tagPanel.SetText(tagData.LocalizedString.GetLocalizedString());
            tagPanel.SetColor(tagData.color);
        }
        else
        {
            Debug.LogWarning($"Tag data not found for tag: {tag} with rarity: {rarity}");
            tagPanel.gameObject.SetActive(false);
        }
    }
    #endregion

    private void SetPanelAnchor(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            labelPanelRectTransform.anchorMin = new Vector2(0.5f, 0);
            labelPanelRectTransform.anchorMax = new Vector2(0.5f, 0);
            labelPanelRectTransform.pivot = new Vector2(0.5f, 0);
        }
        else if (direction == Vector3.down)
        {
            labelPanelRectTransform.anchorMin = new Vector2(0.5f, 1);
            labelPanelRectTransform.anchorMax = new Vector2(0.5f, 1);
            labelPanelRectTransform.pivot = new Vector2(0.5f, 1);
        }
        else if (direction == Vector3.left)
        {
            labelPanelRectTransform.anchorMin = new Vector2(1, 0.5f);
            labelPanelRectTransform.anchorMax = new Vector2(1, 0.5f);
            labelPanelRectTransform.pivot = new Vector2(1, 0.5f);
        }
        else if (direction == Vector3.right)
        {
            labelPanelRectTransform.anchorMin = new Vector2(0, 0.5f);
            labelPanelRectTransform.anchorMax = new Vector2(0, 0.5f);
            labelPanelRectTransform.pivot = new Vector2(0, 0.5f);
        }

        labelPanelRectTransform.anchoredPosition = Vector2.zero;
    }

    public void HideToolTip()
    {
        targetTransform = null;
        gameObject.SetActive(false);
        labelPanel.SetLabel(string.Empty, false);
        labelPanel.SetValue(string.Empty);
    }
}

public enum ToolTipTag
{
    None,
    PlayDice,
    ChaosDice,
    AbilityDice,
    GambleDice,
}

[Serializable]
public class TagData
{
    public ToolTipTag Tag;
    public AbilityDiceRarity Rarity;
    public Color color;
    public LocalizedString LocalizedString;
}