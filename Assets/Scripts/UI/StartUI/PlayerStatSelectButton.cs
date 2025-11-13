using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerStatSelectButton : MonoBehaviour
{
    [SerializeField] private ButtonPanel _buttonPanel;
    [SerializeField] private Image _diceImage;
    [SerializeField] private AnimatedText _chipText;
    [SerializeField] private LocalizedString _chipString;

    private PlayerStatSO _playerStatSO;
    private bool _isAchieved = false;

    public PlayerStatSO PlayerStatSO => _playerStatSO;
    public bool IsAchieved => _isAchieved;

    public event Action<PlayerStatSelectButton> OnSelected;

    private void Awake()
    {
        _buttonPanel.OnClick += () => OnSelected.Invoke(this);
        _buttonPanel.OnPointerEntered += ShowToolTip;
        _buttonPanel.OnPointerExited += HideToolTip;
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

        _diceImage.color = new Color(1, 1, 1, _isAchieved ? 1 : 0.5f);
        _diceImage.sprite = _playerStatSO.diceSpriteListSO.spriteList.First();
        _diceImage.material = new(_diceImage.material);
        _playerStatSO.shaderDataSO.SetMaterialProperties(_diceImage.material);
    }

    public void ShowToolTip()
    {
        var name = _playerStatSO.playerStatName.GetLocalizedString();
        var description = _playerStatSO.playerStatDescription.GetLocalizedString();
        ToolTipUIEvents.TriggerOnToolTipShowRequested(transform, Vector2.left, name, description);
    }

    private void HideToolTip()
    {
        ToolTipUIEvents.TriggerOnToolTipHideRequested();
    }
}