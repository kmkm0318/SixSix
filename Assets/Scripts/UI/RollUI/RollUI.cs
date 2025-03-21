using System;
using UnityEngine;
using UnityEngine.UI;

public class RollUI : Singleton<RollUI>
{
    [SerializeField] private Slider rollPowerSlider;
    [SerializeField] private RollButton rollButton;

    public event Action OnRollButtonPressed;
    public event Action OnRollButtonReleased;

    private void Start()
    {
        rollButton.OnButtonPressed += OnButtonPressed;
        rollButton.OnButtonReleased += OnButtonReleased;

        RollManager.Instance.OnRollPowerChanged += OnRollPowerChanged;

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnButtonPressed()
    {
        OnRollButtonPressed?.Invoke();

        rollPowerSlider.gameObject.SetActive(true);
    }

    private void OnButtonReleased()
    {
        OnRollButtonReleased?.Invoke();

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnRollPowerChanged(float rollPower)
    {
        float rollPowerNormalized = (rollPower - RollManager.Instance.RollPowerMin) / (RollManager.Instance.RollPowerMax - RollManager.Instance.RollPowerMin);
        rollPowerSlider.value = rollPowerNormalized;
    }
}
