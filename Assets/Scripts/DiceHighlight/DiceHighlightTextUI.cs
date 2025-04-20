using TMPro;
using UnityEngine;

public class DiceHighlightTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float offset;

    private Transform targetTransform;
    private Vector3 targetOffset;

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
            transform.SetPositionAndRotation(targetTransform.position + targetOffset, Quaternion.identity);
        }
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}