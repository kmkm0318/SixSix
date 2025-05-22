using System;
using System.Collections.Generic;
using UnityEngine;

public class OptionSelectUI : MonoBehaviour
{
    [SerializeField] private OptionTypeDataSO optionTypeDataSO;
    public OptionTypeDataSO OptionTypeDataSO => optionTypeDataSO;
    [SerializeField] private AnimatedText optionNameText;
    [SerializeField] private ArrowButtonPanel arrowButtonPanel;

    public event Action<int> OnOptionValueChanged;

    private List<string> optionValues;
    private int currentIndex = 0;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (optionTypeDataSO == null) return;

        SetOptionName(optionTypeDataSO.optionName);
        SetOptionValues(optionTypeDataSO.optionValues);
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

    private void SetCurrentIndex()
    {
        int index = 0;
        switch (optionTypeDataSO.optionType)
        {
            case OptionType.GameSpeed:
                index = OptionManager.Instance.OptionData.gameSpeed;
                break;
            case OptionType.AvailityDiceAutoKeep:
                index = OptionManager.Instance.OptionData.availityDiceAutoKeep;
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
            case OptionType.BGMVolume:
                index = OptionManager.Instance.OptionData.bgmVolume;
                break;
            case OptionType.SFXVolume:
                index = OptionManager.Instance.OptionData.sfxVolume;
                break;
        }
        if (index < 0 || index >= optionValues.Count) return;
        currentIndex = index;
        UpdateOptionValueText();
    }

    private void OnLeftArrowClicked()
    {
        if (optionValues == null || optionValues.Count == 0) return;

        currentIndex = (currentIndex - 1 + optionValues.Count) % optionValues.Count;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(currentIndex);
    }

    private void OnRightArrowClicked()
    {
        if (optionValues == null || optionValues.Count == 0) return;

        currentIndex = (currentIndex + 1) % optionValues.Count;
        UpdateOptionValueText();
        OnOptionValueChanged?.Invoke(currentIndex);
    }

    private void UpdateOptionValueText()
    {
        arrowButtonPanel.SetText(optionValues[currentIndex]);
    }
}
