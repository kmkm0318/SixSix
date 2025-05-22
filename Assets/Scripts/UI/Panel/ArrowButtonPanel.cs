using System;
using UnityEngine;

public class ArrowButtonPanel : MonoBehaviour
{
    [SerializeField] private ButtonPanel leftButton;
    [SerializeField] private ButtonPanel rightButton;
    [SerializeField] private TextPanel textPanel;

    public event Action OnLeftButtonClick;
    public event Action OnRightButtonClick;

    private void Awake()
    {
        leftButton.OnClick += () => OnLeftButtonClick?.Invoke();
        rightButton.OnClick += () => OnRightButtonClick?.Invoke();
    }

    public void SetText(string value, bool isShow = false)
    {
        textPanel.SetText(value, isShow);
    }
}