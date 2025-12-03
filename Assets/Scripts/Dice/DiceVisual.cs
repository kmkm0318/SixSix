using System;
using UnityEngine;

public class DiceVisual : MonoBehaviour
{
    [SerializeField] private DiceRenderer diceRenderer;
    [SerializeField] private FadeAnimator fadeAnimator;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(ShaderDataSO shaderDataSO)
    {
        diceRenderer.Initialize(shaderDataSO, spriteRenderer.material);
        spriteRenderer.material = new(shaderDataSO.spriteRendererMaterial);
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetColor(ScorePair enhancedValue)
    {
        int enhanceColorMax = 20;

        float blueIntensity = Mathf.Clamp01((float)enhancedValue.baseScore / 10f / enhanceColorMax);
        float redIntensity = Mathf.Clamp01((float)enhancedValue.multiplier * 20f / enhanceColorMax);

        float redValue = 1 - blueIntensity;
        float greenValue = 1 - redIntensity - blueIntensity;
        float blueValue = 1 - redIntensity;

        if (greenValue < 0)
        {
            redValue -= greenValue;
            blueValue -= greenValue;
            greenValue = 0;
        }

        spriteRenderer.color = new(redValue, greenValue, blueValue, spriteRenderer.color.a);
    }

    public void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    public void FadeIn(Action onComplete = null)
    {
        diceRenderer.SetRandomFadeOffset();
        fadeAnimator.FadeIn(onComplete);
    }

    public void FadeOut(Action onComplete = null)
    {
        diceRenderer.SetRandomFadeOffset();
        fadeAnimator.FadeOut(onComplete);
    }
}
