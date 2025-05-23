using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Localization.Settings;

public class LocalizationManager : Singleton<LocalizationManager>
{
    public const string ENGLISH_CODE = "en";
    public const string KOREAN_CODE = "ko";

    private bool isInitializing = false;

    public bool IsInitialized { get; private set; } = false;
    public event Action OnInitializationComplete;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        OnLanguageChanged(OptionManager.Instance.OptionData.language);
        StartCoroutine(InitializeLocalization());
    }

    private IEnumerator InitializeLocalization()
    {
        if (isInitializing || IsInitialized) yield break;
        isInitializing = true;

        yield return LocalizationSettings.InitializationOperation;
        IsInitialized = true;
        OnInitializationComplete?.Invoke();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        OptionUI.Instance.RegisterOnOptionValueChanged(OptionType.Language, OnLanguageChanged);
    }

    private void OnLanguageChanged(int obj)
    {
        switch (obj)
        {
            case 0:
                SetLocale(ENGLISH_CODE);
                break;
            case 1:
                SetLocale(KOREAN_CODE);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), $"Unsupported language option: {obj}");
        }
    }
    #endregion

    public void SetLocale(string localeCode)
    {
        if (!IsInitialized)
        {
            OnInitializationComplete += () => SetLocale(localeCode);
            return;
        }

        UnityEngine.Debug.Log($"SetLocale: " + localeCode);

        if (localeCode == ENGLISH_CODE || localeCode == KOREAN_CODE)
        {
            var selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (selectedLocale == null)
            {
                UnityEngine.Debug.LogError($"Locale not found: {localeCode}");
                return;
            }
            UnityEngine.Debug.Log($"Selected Locale: {selectedLocale}");
            LocalizationSettings.SelectedLocale = selectedLocale;
        }
        else
        {
            throw new ArgumentException($"Unsupported locale code: {localeCode}");
        }
    }
}