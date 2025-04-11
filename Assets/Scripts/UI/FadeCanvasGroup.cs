using DG.Tweening;
using UnityEngine;

public class FadeCanvasGroup : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn(float duration)
    {
        canvasGroup.DOFade(1f, duration);
    }

    public void FadeOut(float duration)
    {
        canvasGroup.DOFade(0f, duration);
    }
}
