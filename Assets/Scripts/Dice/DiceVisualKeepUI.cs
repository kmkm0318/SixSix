using System.ComponentModel;
using TMPro;
using UnityEngine;

public class DiceVisualKeepUI : MonoBehaviour
{
    [SerializeField] private TMP_Text keepText;

    public void Init(Dice dice)
    {
        dice.OnIsKeepedChanged += SetText;
        dice.DiceInteraction.OnMouseEntered += Show;
        dice.DiceInteraction.OnMouseExited += Hide;

        Hide();
    }

    private void SetText(bool isKeeped)
    {
        if (isKeeped)
        {
            keepText.text = "UNKEEP";
        }
        else
        {
            keepText.text = "KEEP";
        }
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
