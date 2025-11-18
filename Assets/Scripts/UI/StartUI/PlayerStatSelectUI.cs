using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerStatSelectUI : UIFocusHandler
{
    [SerializeField] private AnimatedText _chipText;
    [SerializeField] private LocalizedString _chipString;

    private PlayerStatSO _playerStatSO;
    private bool _isAchieved = false;

    public PlayerStatSO PlayerStatSO => _playerStatSO;
    public bool IsAchieved => _isAchieved;

    public event Action<PlayerStatSelectUI> OnSelected;

    private void Start()
    {
        OnPointerClicked += () => OnSelected.Invoke(this);
        OnPointerEntered += ShowToolTip;
        OnPointerExited += HideToolTip;
    }

    public void Init(PlayerStatSO playerStatSO, bool isAchieved)
    {
        _playerStatSO = playerStatSO;
        _isAchieved = isAchieved;
        UpdateButton();
    }

    public void SetIsAchieved(bool isAchieved)
    {
        _isAchieved = isAchieved;
        UpdateButton();
    }

    private void UpdateButton()
    {
        _chipText.SetText(_chipString.GetLocalizedString(_playerStatSO.price));
        _chipText.gameObject.SetActive(!_isAchieved);

        Image.color = new Color(1, 1, 1, _isAchieved ? 1 : 0.5f);
        Image.sprite = _playerStatSO.diceSpriteListSO.spriteList.Last();
        Image.material = new(Image.material);
        _playerStatSO.shaderDataSO.SetMaterialProperties(Image.material);
    }

    public void ShowToolTip()
    {
        var name = _playerStatSO.playerStatName.GetLocalizedString();
        var description = _playerStatSO.playerStatDescription.GetLocalizedString();
        ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.left, name, description);
    }

    private void HideToolTip()
    {
        ToolTipUIEvents.TriggerOnToolTipHideRequested(transform);
    }
}