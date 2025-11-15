using UnityEngine;

/// <summary>
/// 다이스 오브젝트 및 갬블 다이스 아이콘 UI의 상호작용 정보를 보여주는 UI
/// </summary>
public class InteractionInfoUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _infoText;
    [SerializeField] private DiceInteractionTypeDataList _interactionTypeDatas;

    private Transform _target;

    private void Start()
    {
        RegisterEvents();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InteractionInfoUIEvents.OnShowInteractionInfoUI += OnShowInteractionInfoUI;
        InteractionInfoUIEvents.OnHideInteractionInfoUI += OnHideInteractionInfoUI;
    }

    private void UnregisterEvents()
    {
        InteractionInfoUIEvents.OnShowInteractionInfoUI -= OnShowInteractionInfoUI;
        InteractionInfoUIEvents.OnHideInteractionInfoUI -= OnHideInteractionInfoUI;
    }

    private void OnShowInteractionInfoUI(Transform target, DiceInteractionType type, int value)
    {
        _target = target;

        if (_interactionTypeDatas.DataDict.TryGetValue(type, out var data))
        {
            gameObject.SetActive(true);
            string info = data.localizedText.GetLocalizedString(value);
            _infoText.SetText(info);
        }
    }

    private void OnHideInteractionInfoUI(Transform target)
    {
        if (_target != target) return;
        _target = null;

        _infoText.ClearText();
        gameObject.SetActive(false);
    }
}