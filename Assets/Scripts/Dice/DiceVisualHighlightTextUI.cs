using TMPro;
using UnityEngine;

public class DiceVisualHighlightTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Vector3 offset;

    Transform targetTransform;

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void LateUpdate()
    {
        HandleTransform();
    }

    private void HandleTransform()
    {
        if (gameObject.activeSelf && targetTransform != null)
        {
            transform.SetPositionAndRotation(targetTransform.position + Vector3.up * targetTransform.localScale.y + offset, Quaternion.identity);
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}