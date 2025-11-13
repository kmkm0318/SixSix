using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class OptionSelectUI : MonoBehaviour
{
    [SerializeField] private OptionTypeDataSO _optionTypeDataSO;
    public OptionTypeDataSO OptionTypeDataSO => _optionTypeDataSO;
    [SerializeField] private AnimatedText _optionNameText;
    [SerializeField] private LocalizeStringEvent _optionNameLocalizedText;
    [SerializeField] private ArrowButtonPanel _arrowButtonPanel;
    [SerializeField] private LocalizeStringEvent _optionValueLocalizedText;

    public event Action<int> OnOptionValueChanged;

    private List<string> _optionValues;
    private List<LocalizedString> _optionValuesLocalized;
    private int _valueCount = 0;
    private int _currentIndex = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_optionTypeDataSO == null) return;

        if (_optionTypeDataSO.isLocalizedName)
        {
            _optionNameLocalizedText.StringReference = _optionTypeDataSO.optionNameLocalized;
        }
        else
        {
            SetOptionName(_optionTypeDataSO.optionName);
        }

        if (_optionTypeDataSO.isLocalizedValue)
        {
            SetOptionValues(_optionTypeDataSO.optionValuesLocalized);
        }
        else
        {
            SetOptionValues(_optionTypeDataSO.optionValues);
        }

        SetCurrentIndex();

        _arrowButtonPanel.OnLeftButtonClick += OnLeftArrowClicked;
        _arrowButtonPanel.OnRightButtonClick += OnRightArrowClicked;
    }

    private void SetOptionName(string optionName)
    {
        _optionNameText.SetText(optionName);
    }

    private void SetOptionValues(string[] optionValues)
    {
        _optionValues = new List<string>(optionValues);
        _valueCount = _optionValues.Count;
        if (_optionValues.Count > 0)
        {
            _currentIndex = 0;
            UpdateOptionValueText();
        }
        else
        {
            _arrowButtonPanel.SetText(string.Empty);
        }
    }

    private void SetOptionValues(LocalizedString[] optionValuesLocalized)
    {
        _optionValuesLocalized = new List<LocalizedString>(optionValuesLocalized);
        _valueCount = _optionValuesLocalized.Count;
        if (_optionValuesLocalized.Count > 0)
        {
            _currentIndex = 0;
            UpdateOptionValueText();
        }
        else
        {
            _arrowButtonPanel.SetText(string.Empty);
        }
    }

    private void SetCurrentIndex()
    {
        int index = 0;
        switch (_optionTypeDataSO.optionType)
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

        if (index < 0 || index >= _valueCount) return;

        _currentIndex = index;
        UpdateOptionValueText();
    }

    private void OnLeftArrowClicked()
    {
        if (_valueCount == 0) return;

        _currentIndex = (_currentIndex - 1 + _valueCount) % _valueCount;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(_currentIndex);
    }

    private void OnRightArrowClicked()
    {
        if (_valueCount == 0) return;

        _currentIndex = (_currentIndex + 1) % _valueCount;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(_currentIndex);
    }

    private void UpdateOptionValueText()
    {
        if (_optionTypeDataSO.isLocalizedValue)
        {
            _optionValueLocalizedText.StringReference = _optionValuesLocalized[_currentIndex];
        }
        else
        {
            _arrowButtonPanel.SetText(_optionValues[_currentIndex]);
        }
    }
}
