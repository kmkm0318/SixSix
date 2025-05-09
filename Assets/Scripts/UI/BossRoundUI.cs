using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BossRoundUI : Singleton<BossRoundUI>
{
    [SerializeField] private RectTransform bossRoundUIPanel;
    [SerializeField] private TMP_Text bossRoundName;
    [SerializeField] private TMP_Text bossRoundDescription;
    [SerializeField] private Vector3 hidePos;

    private BossRoundSO currentBossRoundSO = null;
    private Tween currentTween;

    private void Start()
    {
        RegisterEvents();
        bossRoundUIPanel.gameObject.SetActive(false);
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
        bossRoundName.text = currentBossRoundSO.BossName;
        bossRoundDescription.text = currentBossRoundSO.BossDescription;
    }

    private void Show()
    {
        currentTween?.Kill();

        bossRoundUIPanel.gameObject.SetActive(true);
        bossRoundUIPanel.anchoredPosition = hidePos;

        currentTween = bossRoundUIPanel
        .DOAnchorPos(Vector3.zero, DataContainer.Instance.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {

        });
    }

    private void Hide()
    {
        currentTween?.Kill();

        bossRoundUIPanel.anchoredPosition = Vector3.zero;

        currentTween = bossRoundUIPanel
        .DOAnchorPos(hidePos, DataContainer.Instance.DefaultDuration)
        .SetEase(Ease.InOutBack)
        .OnComplete(() =>
        {
            bossRoundUIPanel.gameObject.SetActive(false);
        });
    }
}