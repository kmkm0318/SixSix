using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCategoryScoreSingleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private Button button;
    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    public event Action<ScorePair> OnButtonPressed;

    private ScorePair scorePair;
    private bool isActive = true;
    private bool IsActive
    {
        get => isActive;
        set
        {
            if (isActive == value) return;
            isActive = value;

            if (!value)
            {
                OnUnfocused();
            }
        }
    }

    public void Init(HandCategorySO handCategorySO)
    {
        nameText.text = handCategorySO.handCategoryName;

        UpdateScore(new(0, 0));

        OnUnfocused();

        button.onClick.AddListener(() =>
        {
            HandCategoryScoreUI.Instance.SelectHandCategory(scorePair);
        });
    }

    public void UpdateScore(ScorePair scorePair)
    {
        this.scorePair = scorePair;

        baseScoreText.text = scorePair.baseScore.ToString();
        multiplierText.text = scorePair.multiplier.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActive)
        {
            OnFocused();
        }
    }

    private void OnFocused()
    {
        baseScoreText.color = focusedColor;
        multiplierText.color = focusedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive)
        {
            OnUnfocused();
        }
    }

    private void OnUnfocused()
    {
        baseScoreText.color = unfocusedColor;
        multiplierText.color = unfocusedColor;
    }
}
