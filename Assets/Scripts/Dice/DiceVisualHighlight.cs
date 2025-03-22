using UnityEngine;

public class DiceVisualHighlight : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float scaleSpeed;

    private bool isKeeped = false;

    private void Update()
    {
        if (isKeeped)
        {
            transform.localScale = new Vector3(minScale, minScale, 1);
        }
        else
        {
            var targetScale = Mathf.PingPong(Time.time * scaleSpeed, maxScale - minScale) + minScale;
            transform.localScale = new Vector3(targetScale, targetScale, 1);
        }
    }

    public void Init(Dice dice)
    {
        dice.OnIsKeepedChanged += isKeeped => this.isKeeped = isKeeped;

        dice.DiceInteraction.OnMouseEntered += OnMouseEntered;
        dice.DiceInteraction.OnMouseExited += OnMouseExited;

        Hide();
    }

    private void OnMouseEntered()
    {
        Show();
    }

    private void OnMouseExited()
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
