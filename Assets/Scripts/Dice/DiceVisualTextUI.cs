using TMPro;
using UnityEngine;

public class DiceHighlightTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}