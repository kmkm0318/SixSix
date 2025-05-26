using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class BossRoundUI : Singleton<BossRoundUI>
{
    [SerializeField] private LabeledValuePanel bossRoundPanel;
    [SerializeField] private RectTransform bossRoundPanelRectTransform;
    [SerializeField] private Vector3 hidePos;

    private BossRoundSO currentBossRoundSO = null;
    private Tween currentTween;

    private void Start()
    {
        RegisterEvents();
        bossRoundPanelRectTransform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += SelectedLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= SelectedLocaleChanged;
    }

    private void SelectedLocaleChanged(Locale locale)
    {
        RefreshLocalizedText();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        BossRoundManager.Instance.OnBossRoundEntered += OnBossRoundEntered;
        BossRoundManager.Instance.OnBossRoundExited += OnBossRoundExited;
    }

    private void OnBossRoundEntered()
    {
        Init(BossRoundManager.Instance.CurrentBossRoundSO);
        Show();
    }

    private void OnBossRoundExited()
    {
        Hide();
    }
    #endregion

    private void Init(BossRoundSO bossRoundSO)
    {
        currentBossRoundSO = bossRoundSO;
        bossRoundPanel.SetLabel(currentBossRoundSO.BossName);
        bossRoundPanel.SetValue(currentBossRoundSO.GetBossDescription());
    }

    private void RefreshLocalizedText()
    {
        if (currentBossRoundSO == null) return;

        bossRoundPanel.SetLabel(currentBossRoundSO.BossName);
        bossRoundPanel.SetValue(currentBossRoundSO.GetBossDescription());
    }

    private void Show()
    {
        currentTween?.Kill();

        bossRoundPanelRectTransform.gameObject.SetActive(true);
        bossRoundPanelRectTransform.anchoredPosition = hidePos;

        currentTween = bossRoundPanelRectTransform
        .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {

        });
    }

    private void Hide()
    {
        currentTween?.Kill();

        bossRoundPanelRectTransform.anchoredPosition = Vector3.zero;

        currentTween = bossRoundPanelRectTransform
        .DOAnchorPos(hidePos, AnimationFunction.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {
            bossRoundPanelRectTransform.gameObject.SetActive(false);
        });
    }
}