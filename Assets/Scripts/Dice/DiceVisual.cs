using System;
using UnityEngine;

public class DiceVisual : MonoBehaviour
{
    private const string fadeOutTriggerName = "FadeOut";
    private const string fadeInTriggerName = "FadeIn";

    private readonly int enhanceColorMax = 20;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Action onFadeInComplete;
    private Action onFadeOutComplete;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetSpriteMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public void SetColor(ScorePair enhancedValue)
    {
        float blueIntensity = Mathf.Clamp01(enhancedValue.baseScore / 10f / enhanceColorMax);
        float redIntensity = Mathf.Clamp01(enhancedValue.multiplier * 20f / enhanceColorMax);

        float redValue = 1 - blueIntensity;
        float greenValue = 1 - redIntensity - blueIntensity;
        float blueValue = 1 - redIntensity;

        if (greenValue < 0)
        {
            redValue -= greenValue;
            blueValue -= greenValue;
            greenValue = 0;
        }

        spriteRenderer.color = new Color(redValue, greenValue, blueValue, spriteRenderer.color.a);
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void FadeIn(Action onComplete = null)
    {
        onFadeInComplete = onComplete;
        animator.SetTrigger(fadeInTriggerName);
    }

    public void FadeOut(Action onComplete = null)
    {
        onFadeOutComplete = onComplete;
        animator.SetTrigger(fadeOutTriggerName);
    }

    public void OnFadeInComplete()
    {
        onFadeInComplete?.Invoke();
        onFadeInComplete = null;
    }

    public void OnFadeOutComplete()
    {
        onFadeOutComplete?.Invoke();
        onFadeOutComplete = null;
    }
}
