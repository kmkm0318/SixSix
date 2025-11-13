using System;
using UnityEngine;

public class ArrowButtonPanel : MonoBehaviour
{
    [SerializeField] private ButtonPanel _leftButton;
    [SerializeField] private ButtonPanel _rightButton;
    [SerializeField] private TextPanel _textPanel;

    public event Action OnLeftButtonClick;
    public event Action OnRightButtonClick;

    private void Awake()
    {
        _leftButton.OnClick += () => OnLeftButtonClick?.Invoke();
        _rightButton.OnClick += () => OnRightButtonClick?.Invoke();
    }

    public void SetText(string value, bool isShow = false)
    {
        _textPanel.SetText(value, isShow);
    }
}