using UnityEngine;
using UnityEngine.UI;

public class HideablePanel : MonoBehaviour
{
    [SerializeField] private Vector2 originalSize;
    [SerializeField] private float speed = 1f;

    private RectTransform rectTransform;
}
