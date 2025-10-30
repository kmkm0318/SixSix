using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] private RectTransform optionPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private OptionSelectUI[] optionSelectUIs;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;

    private void Start()
    {
        RegisterEvents();
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        closeButton.onClick.AddListener(() => Hide());
        gameObject.SetActive(false);
    }

    private void GoToMainMenu()
    {
        Hide();

        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionButtonClicked += OnOptionButtonClicked;

        foreach (var optionSelectUI in optionSelectUIs)
        {
            optionSelectUI.OnOptionValueChanged += (value) => OptionUIEvents.TriggerOnOptionValueChanged(optionSelectUI.OptionTypeDataSO.optionType, value);
        }
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionButtonClicked -= OnOptionButtonClicked;
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
            .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(AnimationFunction.DefaultDuration);
    }

    private void Hide()
    {
        optionPanel.anchoredPosition = Vector3.zero;
        optionPanel
            .DOAnchorPos(hidePos, AnimationFunction.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });

        fadeCanvasGroup.FadeOut(AnimationFunction.DefaultDuration);
    }
    #endregion
}