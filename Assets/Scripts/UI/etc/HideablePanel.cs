using DG.Tweening;
using UnityEngine;

public class HideablePanel : MonoBehaviour
{
    public enum Anchor { Top, Bottom, Left, Right }
    [SerializeField] private RectTransform anchorPanel;
    [SerializeField] private Anchor anchor;
    [SerializeField] private float duration;
    [SerializeField] private bool hideOnAwake;

    private RectTransform rectTransform;
    private Vector2 originalSize;
    private Vector2 hideSize;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;

        SetAnchor(anchor);

        if (anchor == Anchor.Top || anchor == Anchor.Bottom)
        {
            hideSize = new Vector2(originalSize.x, 0);
        }
        else
        {
            hideSize = new Vector2(0, originalSize.y);
        }

        if (hideOnAwake)
        {
            rectTransform.sizeDelta = hideSize;
        }
    }

    private void SetAnchor(Anchor pivot)
    {
        switch (pivot)
        {
            case Anchor.Top:
                anchorPanel.anchorMin = new Vector2(0.5f, 1f);
                anchorPanel.anchorMax = new Vector2(0.5f, 1f);
                anchorPanel.pivot = new Vector2(0.5f, 1f);
                break;

            case Anchor.Bottom:
                anchorPanel.anchorMin = new Vector2(0.5f, 0f);
                anchorPanel.anchorMax = new Vector2(0.5f, 0f);
                anchorPanel.pivot = new Vector2(0.5f, 0f);
                break;

            case Anchor.Left:
                anchorPanel.anchorMin = new Vector2(0f, 0.5f);
                anchorPanel.anchorMax = new Vector2(0f, 0.5f);
                anchorPanel.pivot = new Vector2(0f, 0.5f);
                break;

            case Anchor.Right:
                anchorPanel.anchorMin = new Vector2(1f, 0.5f);
                anchorPanel.anchorMax = new Vector2(1f, 0.5f);
                anchorPanel.pivot = new Vector2(1f, 0.5f);
                break;
        }
    }

    public void Show()
    {
        rectTransform.DOSizeDelta(originalSize, duration);
    }

    public void Hide()
    {
        rectTransform.DOSizeDelta(Vector2.zero, duration);
    }
}
