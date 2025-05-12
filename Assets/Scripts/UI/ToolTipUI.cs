using Febucci.UI;
using TMPro;
using UnityEngine;

public class ToolTipUI : Singleton<ToolTipUI>
{
    [SerializeField] private RectTransform toolTipPanel;
    [SerializeField] private AnimatedText diceNameText;
    [SerializeField] private AnimatedText descriptionText;
    [SerializeField] private float offset;

    private RectTransform rectTransform;
    private Transform targetTransform;
    private Vector3 targetOffset;

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
        PlayerMouseManager.Instance.OnMouseExit += HideToolTip;
    }
    #endregion

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (targetTransform == null) return;
        var targetPos = Camera.main.WorldToScreenPoint(targetTransform.position + targetOffset);
        rectTransform.position = targetPos;
    }

    public void ShowToolTip(IToolTipable toolTipable, Transform transform, Vector3 direction, string name, string description)
    {
        if (toolTipable == null) return;
        if (transform == null) return;
        if (UtilityFunctions.IsPointerOverUIElement()) return;

        gameObject.SetActive(true);

        SetPanelAnchor(direction);
        targetTransform = transform;
        targetOffset = (transform.localScale.x / 2 + offset) * direction;
        diceNameText.ShowText(name, true);
        descriptionText.SetText(description, true);
    }

    private void SetPanelAnchor(Vector3 direction)
    {
        if (direction == Vector3.up)
        {
            toolTipPanel.anchorMin = new Vector2(0.5f, 0);
            toolTipPanel.anchorMax = new Vector2(0.5f, 0);
            toolTipPanel.pivot = new Vector2(0.5f, 0);
        }
        else if (direction == Vector3.down)
        {
            toolTipPanel.anchorMin = new Vector2(0.5f, 1);
            toolTipPanel.anchorMax = new Vector2(0.5f, 1);
            toolTipPanel.pivot = new Vector2(0.5f, 1);
        }
        else if (direction == Vector3.left)
        {
            toolTipPanel.anchorMin = new Vector2(1, 0.5f);
            toolTipPanel.anchorMax = new Vector2(1, 0.5f);
            toolTipPanel.pivot = new Vector2(1, 0.5f);
        }
        else if (direction == Vector3.right)
        {
            toolTipPanel.anchorMin = new Vector2(0, 0.5f);
            toolTipPanel.anchorMax = new Vector2(0, 0.5f);
            toolTipPanel.pivot = new Vector2(0, 0.5f);
        }

        toolTipPanel.anchoredPosition = Vector2.zero;
    }

    public void HideToolTip()
    {
        targetTransform = null;
        gameObject.SetActive(false);
    }
}

