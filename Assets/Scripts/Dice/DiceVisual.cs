using UnityEngine;

public class DiceVisual : MonoBehaviour
{
    private readonly int enhanceColorMax = 20;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetColor(ScorePair enhancedValue)
    {
        if (enhancedValue.baseScore == 0 && enhancedValue.multiplier == 0)
        {
            return;
        }

        float blueIntensity = Mathf.Clamp01(enhancedValue.baseScore / enhanceColorMax);
        float redIntensity = Mathf.Clamp01(enhancedValue.multiplier / enhanceColorMax);

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
}
