using UnityEngine;
using UnityEngine.Localization;

public class PlayerChipText : MonoBehaviour
{
    [SerializeField] private AnimatedText _chipText;
    [SerializeField] private LocalizedString _chipString;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        UpdateChipText(PlayerDataManager.Instance.PlayerData.chip);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        PlayerDataManager.Instance.OnChipChanged += UpdateChipText;
    }

    private void UnregisterEvents()
    {
        PlayerDataManager.Instance.OnChipChanged -= UpdateChipText;
    }

    private void UpdateChipText(int chip)
    {
        _chipText.SetText(_chipString.GetLocalizedString(chip.ToString("N0")));
    }
}