using UnityEngine;
using UnityEngine.UI;

public class RollUI : MonoBehaviour
{
    [SerializeField] private Slider rollPowerSlider;
    [SerializeField] private RollButton rollButton;

    private void Start()
    {
        rollButton.OnButtonDown += OnButtonPressed;
        rollButton.OnButtonUp += OnButtonReleased;

        RollManager.Instance.OnRollPowerChanged += OnRollPowerChanged;

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnButtonPressed()
    {
        RollUIEvents.TriggerOnRollButtonPressed();

        rollPowerSlider.gameObject.SetActive(true);
    }

    private void OnButtonReleased()
    {
        RollUIEvents.TriggerOnRollButtonReleased();

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnRollPowerChanged(float rollPower)
    {
        float rollPowerNormalized = (rollPower - RollManager.Instance.RollPowerMin) / (RollManager.Instance.RollPowerMax - RollManager.Instance.RollPowerMin);
        rollPowerSlider.value = rollPowerNormalized;
    }
}
