using UnityEngine;

public class DiceHighlightTextUI : MonoBehaviour
{
    [SerializeField] private AnimatedText text;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private Vector3 resetPosition;

    private RectTransform rectTransform;
    private Transform targetTransform;
    private Vector3 targetOffset;
    bool isUI = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTargetAndOffset(Transform target)
    {
        isUI = false;
        targetTransform = target;
        targetOffset = target.localScale.x / 2 * Vector3.up;
    }

    public void SetTargetAndOffset(RectTransform targetRect)
    {
        isUI = true;
        targetTransform = targetRect;
        targetOffset = targetRect.sizeDelta.y * targetRect.localScale.y / 2 * Vector3.up;
    }

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (targetTransform != null)
        {
            Vector3 targetPos;
            if (isUI)
            {
                targetPos = targetTransform.position + targetOffset;
            }
            else
            {
                targetPos = targetCamera.WorldToScreenPoint(targetTransform.position + targetOffset);
            }
            rectTransform.position = targetPos + offset;
        }
    }

    public void SetText(string text)
    {
        this.text.SetText(text);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        transform.position = resetPosition;
    }
}