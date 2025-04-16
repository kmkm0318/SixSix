using TMPro;
using UnityEngine;

public class DiceVisualHighlightTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}