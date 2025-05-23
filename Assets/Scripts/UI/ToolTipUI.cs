using System;
using System.Linq;
using UnityEngine;

public class ToolTipUI : Singleton<ToolTipUI>
{
    [SerializeField] private LabeledValuePanel labelPanel;
    [SerializeField] private RectTransform labelPanelRectTransform;
    [SerializeField] private TextPanel tagPanel;
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

    #region RegisterEvents
    private void RegisterEvents()
    {
        ShopUI.Instance.OnShopUIOpened += HideToolTip;
        RoundClearUI.Instance.OnRoundClearUIOpened += HideToolTip;
        MouseManager.Instance.OnMouseExit += OnMouseExit;
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

    public void ShowToolTip(Transform transform, Vector2 direction, string name, string description, ToolTipTag tag = ToolTipTag.None, AvailityDiceRarity rarity = AvailityDiceRarity.Normal)
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
    private void HandleTag(ToolTipTag tag, AvailityDiceRarity rarity)
    {
        if (tag == ToolTipTag.None)
        {
            tagPanel.gameObject.SetActive(false);
            return;
        }

        tagPanel.gameObject.SetActive(true);

        string tagText = string.Empty;
        if (tag == ToolTipTag.Availity_Dice)
        {
            HandleTagColor(rarity);
            tagText = rarity.ToString();
        }
        else
        {
            HandleTagColor(tag);
            tagText = tag.ToString();
        }
        tagText = tagText.ToUpper().Replace("_", " ");
        tagPanel.SetText(tagText);
    }

    private void HandleTagColor(ToolTipTag tag)
    {
        Color color = tag switch
        {
            ToolTipTag.Play_Dice => Color.white,
            ToolTipTag.Chaos_Dice => Color.black,
            ToolTipTag.Gamble_Dice => Color.yellow,
            _ => Color.white,
        };
        SetTagPanelColor(color);
    }

    private void HandleTagColor(AvailityDiceRarity rarity)
    {
        Color color = rarity switch
        {
            AvailityDiceRarity.Normal => Color.gray,
            AvailityDiceRarity.Rare => Color.blue,
            AvailityDiceRarity.Epic => Color.red,
            AvailityDiceRarity.Legendary => Color.magenta,
            _ => Color.white,
        };
        SetTagPanelColor(color);
    }

    private void SetTagPanelColor(Color color)
    {
        color.a = 0.75f;
        tagPanel.SetColor(color);
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
    Play_Dice,
    Chaos_Dice,
    Availity_Dice,
    Gamble_Dice,
}