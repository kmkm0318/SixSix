using TMPro;
using UnityEngine;

public class DiceHighlightTextUI : MonoBehaviour
{
    [SerializeField] private AnimatedText text;
    [SerializeField] private float offset;
    [SerializeField] private Camera targetCamera;

    private RectTransform rectTransform;
    private Transform targetTransform;
    private Vector3 targetOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetTargetAndOffset(Transform target)
    {
        targetTransform = target;
        targetOffset = (target.localScale.x / 2 + offset) * Vector3.up;
    }

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (gameObject.activeSelf && targetTransform != null)
        {
            var targetPos = targetCamera.WorldToScreenPoint(targetTransform.position + targetOffset);
            rectTransform.position = targetPos;
        }
    }

    public void SetText(string text)
    {
        this.text.SetText(text);
    }
}