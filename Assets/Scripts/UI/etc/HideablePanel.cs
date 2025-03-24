using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HideablePanel : MonoBehaviour
{
    public enum Anchor { Top, Bottom, Left, Right }

    [SerializeField] private RectTransform anchorPanel;
    [SerializeField] private RectTransform layoutPanel;
    [SerializeField] private Anchor anchor;
    [SerializeField] private float duration;
    [SerializeField] private bool hideOnAwake;

    private RectTransform rectTransform;
    private Vector2 originalSize;
    private Vector2 hideSize;
    private Tween currentTween;

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        currentTween?.Kill();
    }

    private void Init()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;

        SetAnchor();
        SetHideSize();

        if (hideOnAwake)
        {
            rectTransform.sizeDelta = hideSize;
        }
    }

    private void SetAnchor()
    {
        var anchorVal = GetAnchor(anchor);
        var pivotVal = GetPivot(anchor);

        anchorPanel.anchorMin = anchorVal;
        anchorPanel.anchorMax = anchorVal;
        anchorPanel.pivot = anchorVal;
        rectTransform.pivot = pivotVal;
    }

    private Vector2 GetAnchor(Anchor value)
    {
        return value switch
        {
            Anchor.Top => new Vector2(0.5f, 1f),
            Anchor.Bottom => new Vector2(0.5f, 0f),
            Anchor.Left => new Vector2(0f, 0.5f),
            Anchor.Right => new Vector2(1f, 0.5f),
            _ => Vector2.zero,
        };
    }

    private Vector2 GetPivot(Anchor value)
    {
        return value switch
        {
            Anchor.Top => new Vector2(0.5f, 0f),
            Anchor.Bottom => new Vector2(0.5f, 1f),
            Anchor.Left => new Vector2(1f, 0.5f),
            Anchor.Right => new Vector2(0f, 0.5f),
            _ => Vector2.zero,
        };
    }

    private void SetHideSize()
    {
        hideSize = anchor switch
        {
            Anchor.Top or Anchor.Bottom => new Vector2(originalSize.x, 0),
            _ => new Vector2(0, originalSize.y),
        };
    }

    public void Show()
    {
        AnimatePanel(originalSize);
    }

    public void Hide()
    {
        AnimatePanel(hideSize);
    }

    private void AnimatePanel(Vector2 targetSize)
    {
        currentTween?.Kill();
        currentTween = rectTransform.DOSizeDelta(targetSize, duration).OnUpdate(() =>
        {
            if (layoutPanel != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
            }
        });
    }
}
