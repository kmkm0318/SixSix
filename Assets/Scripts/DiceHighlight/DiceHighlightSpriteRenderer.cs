using System;
using UnityEngine;

public class DiceHighlightSpriteRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Transform targetTransform;
    private float minScale = 1;
    private float maxScale = 1.125f;
    private float scaleSpeed = 1f;

    private void Update()
    {
        HandleScale();
    }

    private void HandleScale()
    {
        if (targetTransform == null) return;

        var targetScale = Mathf.PingPong(Time.time * scaleSpeed, 1) * (maxScale - minScale) + minScale;
        transform.localScale = new Vector3(targetScale, targetScale, 1);
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            UpdateTransform();
        }
    }

    public void SetTarget(Transform target, Color color, float minScale, float maxScale, float scaleSpeed)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
        this.scaleSpeed = scaleSpeed;

        if (target == null)
        {
            Debug.LogWarning("Target transform is null. Cannot set target for DiceHighlightSpriteRenderer.");
            return;
        }
        targetTransform = target;
        SetColor(color);
        UpdateScale();
        UpdateTransform();
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer is not assigned. Cannot set color.");
        }
    }

    private void UpdateScale()
    {
        transform.localScale = targetTransform.localScale;
    }

    private void UpdateTransform()
    {
        transform.SetLocalPositionAndRotation(targetTransform.position, targetTransform.rotation);
    }

    public void Hide()
    {
        targetTransform = null;
        gameObject.SetActive(false);
    }

    internal void Show()
    {
        gameObject.SetActive(true);
    }
}