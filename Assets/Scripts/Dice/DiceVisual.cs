using UnityEngine;

public class DiceVisual : MonoBehaviour
{
    [SerializeField] private DiceVisualHighlight highlight;
    [SerializeField] private DiceVisualKeepUI keepUI;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Dice dice)
    {
        highlight.Init(dice);
        keepUI.Init(dice);
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
