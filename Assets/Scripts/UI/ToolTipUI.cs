using TMPro;
using UnityEngine;

public class ToolTipUI : Singleton<ToolTipUI>
{
    [SerializeField] private TMP_Text diceNameText;
    [SerializeField] private TMP_Text descriptionText;
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
        if (Functions.IsPointerOverUIElement()) return;

        targetTransform = transform;
        targetOffset = (transform.localScale.x / 2 + offset) * direction;
        diceNameText.text = name;
        descriptionText.text = description;

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        targetTransform = null;
        gameObject.SetActive(false);
    }
}