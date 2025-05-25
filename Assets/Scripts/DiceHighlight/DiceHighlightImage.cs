using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceHighlightImage : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Image> borderImages;

    private RectTransform rectTransform;
    private RectTransform targetTransform;
    private float minScale = 1f;
    private float maxScale = 1.125f;
    private float scaleSpeed = 1f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (gameObject.activeSelf && targetTransform != null)
        {
            rectTransform.position = targetTransform.position;

            float targetScale = Mathf.PingPong(Time.time * scaleSpeed, 1) * (maxScale - minScale) + minScale;
            rectTransform.localScale = new Vector3(targetScale, targetScale, 1);
        }
    }

    private void SetColor(Color color)
    {
        foreach (var image in borderImages)
        {
            if (image != null)
            {
                image.color = color;
            }
            else
            {
                Debug.LogWarning("Border image is null. Cannot set color for DiceHighlightImage.");
            }
        }
    }

    private void UpdateScale(float minScale, float maxScale, float scaleSpeed)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
        this.scaleSpeed = scaleSpeed;
    }

    private void UpdateTransform()
    {
        if (targetTransform != null)
        {
            Vector3 targetPos = mainCamera.WorldToScreenPoint(targetTransform.position);
            rectTransform.position = targetPos;
            rectTransform.localScale = new Vector3(minScale, minScale, 1);
        }
    }

    public void SetTarget(RectTransform target, Color color, float minScale, float maxScale, float scaleSpeed)
    {
        if (target == null)
        {
            Debug.LogWarning("Target transform is null. Cannot set target for DiceHighlightImage.");
            return;
        }

        targetTransform = target.GetComponent<RectTransform>();
        SetColor(color);
        SetSize(target);
        UpdateScale(minScale, maxScale, scaleSpeed);
        UpdateTransform();
    }

    private void SetSize(RectTransform target)
    {
        if (target != null)
        {
            rectTransform.sizeDelta = target.sizeDelta;
        }
        else
        {
            Debug.LogWarning("Target RectTransform is null. Cannot set size for DiceHighlightImage.");
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}