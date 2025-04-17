using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AvailityDiceToolTipUI : Singleton<AvailityDiceToolTipUI>
{
    [SerializeField] private TMP_Text diceNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Vector3 offset;

    private RectTransform rectTransform;
    private Transform targetTrasnform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        HideToolTip();
    }

    private void LateUpdate()
    {
        if (targetTrasnform == null) return;
        var targetPos = Camera.main.WorldToScreenPoint(targetTrasnform.position + offset);
        rectTransform.position = targetPos;
    }

    public void ShowToolTip(AvailityDice availityDice)
    {
        if (availityDice == null || availityDice.AvailityDiceSO == null) return;
        if (Functions.IsPointerOverUIElement()) return;
        AvailityDiceSO availityDiceSO = availityDice.AvailityDiceSO;

        targetTrasnform = availityDice.transform;

        diceNameText.text = availityDiceSO.diceName;
        descriptionText.text = availityDiceSO.GetDescriptionText();

        gameObject.SetActive(true);
    }


    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}