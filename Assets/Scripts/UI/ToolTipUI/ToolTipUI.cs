using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(RectTransform))]
public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private LabeledValuePanel labelPanel;
    [SerializeField] private RectTransform labelPanelRectTransform;
    [SerializeField] private TextPanel tagPanel;
    [SerializeField] private List<TagData> tagDataList;
    [SerializeField] private float offset;

    private RectTransform _rectTransform;
    private Transform _target;
    private Vector3 _targetOffset;
    private Vector3 _offsetDirection;
    bool isUI = false;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        RegisterEvents();
        HideToolTip(null);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        ToolTipUIEvents.OnToolTipShowRequested += ShowToolTip;
        ToolTipUIEvents.OnToolTipHideRequested += HideToolTip;
    }

    private void UnregisterEvents()
    {
        ToolTipUIEvents.OnToolTipShowRequested -= ShowToolTip;
        ToolTipUIEvents.OnToolTipHideRequested -= HideToolTip;
    }
    #endregion

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (_target == null) return;

        Vector3 targetPos;
        if (isUI)
        {
            targetPos = _target.position + _targetOffset;
        }
        else
        {
            targetPos = Camera.main.WorldToScreenPoint(_target.position + _targetOffset);
        }

        _rectTransform.position = targetPos + _offsetDirection * offset;
    }

    public void ShowToolTip(Transform target, Vector2 direction, string name, string description, ToolTipTag tag = ToolTipTag.None, AbilityDiceRarity rarity = AbilityDiceRarity.Normal)
    {
        if (target == null) return;
        if (target is RectTransform rect)
        {
            isUI = true;
            _targetOffset = rect.rect.width / 2 * direction;
        }
        else
        {
            isUI = false;
            _targetOffset = target.localScale.x / 2 * direction;
        }
        _offsetDirection = direction.normalized;
        gameObject.SetActive(true);
        SetPanelAnchor(direction);
        _target = target;

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

    public void HideToolTip(Transform target)
    {
        if (_target != target) return;
        _target = null;
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