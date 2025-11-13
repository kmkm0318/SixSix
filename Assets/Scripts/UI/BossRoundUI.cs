using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class BossRoundUI : BaseUI
{
    [SerializeField] private LabeledValuePanel bossRoundPanel;

    private BossRoundSO currentBossRoundSO = null;

    private void Start()
    {
        RegisterEvents();
        _panel.gameObject.SetActive(false);
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
}