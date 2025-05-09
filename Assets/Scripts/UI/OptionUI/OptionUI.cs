using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : Singleton<OptionUI>
{
    [SerializeField] private RectTransform optionPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private OptionSelectUI[] optionSelectUIs;
    [SerializeField] private Button closeButton;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;

    private void Start()
    {
        RegisterEvents();
        closeButton.onClick.AddListener(() => Hide());
        gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        StateUI.Instance.OnOptionButtonClicked += OnOptionButtonClicked;
    }

    private void OnOptionButtonClicked()
    {
        Show();
    }
    #endregion

    #region ShowHide
    private void Show()
    {
        gameObject.SetActive(true);
        optionPanel.anchoredPosition = hidePos;
        optionPanel
            .DOAnchorPos(Vector3.zero, DataContainer.Instance.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(DataContainer.Instance.DefaultDuration);
    }

    private void Hide()
    {
        optionPanel.anchoredPosition = Vector3.zero;
        optionPanel
            .DOAnchorPos(hidePos, DataContainer.Instance.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });

        fadeCanvasGroup.FadeOut(DataContainer.Instance.DefaultDuration);
    }
    #endregion

    public void RegisterOnOptionValueChanged(OptionType type, Action<int> action)
    {
        foreach (var optionSelectUI in optionSelectUIs)
        {
            if (optionSelectUI.OptionTypeDataSO.optionType == type)
            {
                optionSelectUI.OnOptionValueChanged += action;
            }
        }
    }
}