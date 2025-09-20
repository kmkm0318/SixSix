using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class OptionSelectUI : MonoBehaviour
{
    [SerializeField] private OptionTypeDataSO optionTypeDataSO;
    public OptionTypeDataSO OptionTypeDataSO => optionTypeDataSO;
    [SerializeField] private AnimatedText optionNameText;
    [SerializeField] private LocalizeStringEvent optionNameLocalizedText;
    [SerializeField] private ArrowButtonPanel arrowButtonPanel;
    [SerializeField] private LocalizeStringEvent optionValueLocalizedText;

    public event Action<int> OnOptionValueChanged;

    private List<string> optionValues;
    private List<LocalizedString> optionValuesLocalized;
    private int valueCount = 0;
    private int currentIndex = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (optionTypeDataSO == null) return;

        if (optionTypeDataSO.isLocalizedName)
        {
            optionNameLocalizedText.StringReference = optionTypeDataSO.optionNameLocalized;
        }
        else
        {
            SetOptionName(optionTypeDataSO.optionName);
        }

        if (optionTypeDataSO.isLocalizedValue)
        {
            SetOptionValues(optionTypeDataSO.optionValuesLocalized);
        }
        else
        {
            SetOptionValues(optionTypeDataSO.optionValues);
        }

        SetCurrentIndex();

        arrowButtonPanel.OnLeftButtonClick += OnLeftArrowClicked;
        arrowButtonPanel.OnRightButtonClick += OnRightArrowClicked;
    }

    private void SetOptionName(string optionName)
    {
        optionNameText.SetText(optionName);
    }

    private void SetOptionValues(string[] optionValues)
    {
        this.optionValues = new List<string>(optionValues);
        valueCount = this.optionValues.Count;
        if (this.optionValues.Count > 0)
        {
            currentIndex = 0;
            UpdateOptionValueText();
        }
        else
        {
            arrowButtonPanel.SetText(string.Empty);
        }
    }

    private void SetOptionValues(LocalizedString[] optionValuesLocalized)
    {
        this.optionValuesLocalized = new List<LocalizedString>(optionValuesLocalized);
        valueCount = this.optionValuesLocalized.Count;
        if (this.optionValuesLocalized.Count > 0)
        {
            currentIndex = 0;
            UpdateOptionValueText();
        }
        else
        {
            arrowButtonPanel.SetText(string.Empty);
        }
    }

    private void SetCurrentIndex()
    {
        int index = 0;
        switch (optionTypeDataSO.optionType)
        {
            case OptionType.GameSpeed:
                index = OptionManager.Instance.OptionData.gameSpeed;
                break;
            case OptionType.AbilityDiceAutoKeep:
                index = OptionManager.Instance.OptionData.abilityDiceAutoKeep;
                break;
            case OptionType.Language:
                index = OptionManager.Instance.OptionData.language;
                break;
            case OptionType.Fullscreen:
                index = OptionManager.Instance.OptionData.fullscreen;
                break;
            case OptionType.Resolution:
                index = OptionManager.Instance.OptionData.resolution;
                break;
            case OptionType.MasterVolume:
                index = OptionManager.Instance.OptionData.masterVolume;
                break;
            case OptionType.BGMVolume:
                index = OptionManager.Instance.OptionData.bgmVolume;
                break;
            case OptionType.SFXVolume:
                index = OptionManager.Instance.OptionData.sfxVolume;
                break;
        }

        if (index < 0 || index >= valueCount) return;

        currentIndex = index;
        UpdateOptionValueText();
    }

    private void OnLeftArrowClicked()
    {
        if (valueCount == 0) return;

        currentIndex = (currentIndex - 1 + valueCount) % valueCount;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(currentIndex);
    }

    private void OnRightArrowClicked()
    {
        if (valueCount == 0) return;

        currentIndex = (currentIndex + 1) % valueCount;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(currentIndex);
    }

    private void UpdateOptionValueText()
    {
        if (optionTypeDataSO.isLocalizedValue)
        {
            optionValueLocalizedText.StringReference = optionValuesLocalized[currentIndex];
        }
        else
        {
            arrowButtonPanel.SetText(optionValues[currentIndex]);
        }
    }
}
