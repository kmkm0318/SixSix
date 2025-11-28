using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class RollUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider rollPowerSlider;
    [SerializeField] private RollButton rollButton;

    [Header("Cinemachine Impulse")]
    [SerializeField] private CinemachineImpulseSource _rollImpulse;
    [SerializeField] private CinemachineImpulseSource _rumbleImpulse;
    [SerializeField] private float _rollForce = .5f;
    [SerializeField] private float _rumbleForce = 0.05f;

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

        _rollImpulse.GenerateImpulse(Random.insideUnitSphere * _rollForce);
    }

    private void OnRollPowerChanged(float rollPower)
    {
        float rollPowerNormalized = (rollPower - RollManager.Instance.RollPowerMin) / (RollManager.Instance.RollPowerMax - RollManager.Instance.RollPowerMin);
        rollPowerSlider.value = rollPowerNormalized;

        _rumbleImpulse.GenerateImpulse(_rumbleForce * rollPowerNormalized * Random.insideUnitSphere);
    }
}
