using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다이스 오브젝트 및 갬블 다이스 아이콘 UI의 상호작용 정보를 보여주는 UI
/// </summary>
public class InteractInfoUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _infoText;
    [SerializeField] private List<DiceInteractionTypeData> _interactTypeDatas;

    private Dictionary<DiceInteractionType, DiceInteractionTypeData> _interactTypeDataDict;

    private void Start()
    {
        InitDict();
        RegisterEvents();
    }

    private void InitDict()
    {
        _interactTypeDataDict = new();
        foreach (var data in _interactTypeDatas)
        {
            _interactTypeDataDict[data.type] = data;
        }
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InteractInfoUIEvents.OnShowInteractInfoUI += OnShowInteractInfoUI;
        InteractInfoUIEvents.OnHideInteractInfoUI += OnHideInteractInfoUI;
    }

    private void UnregisterEvents()
    {
        InteractInfoUIEvents.OnShowInteractInfoUI -= OnShowInteractInfoUI;
        InteractInfoUIEvents.OnHideInteractInfoUI -= OnHideInteractInfoUI;
    }

    private void OnShowInteractInfoUI(DiceInteractionType type, int value)
    {
        if (_interactTypeDataDict.TryGetValue(type, out var data))
        {
            string info = data.localizedText.GetLocalizedString(value);
            _infoText.SetText(info);
            gameObject.SetActive(true);
        }
    }

    private void OnHideInteractInfoUI()
    {
        _infoText.ClearText();
        gameObject.SetActive(false);
    }
}