using System;
using UnityEngine;
using UnityEngine.UI;

public class RollUI : MonoBehaviour
{
    public static RollUI Instance { get; private set; }

    [SerializeField] private RollButton rollButton;
    [SerializeField] private Slider rollPowerSlider;

    public event Action OnRollButtonDownEvent;
    public event Action OnRollButtonUpEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        rollButton.OnPointerDownEvent += OnRollButtonDown;
        rollButton.OnPointerUpEvent += OnRollButtonUp;

        RollManager.Instance.OnRollPowerChangedEvent += OnRollPowerChanged;

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnRollButtonDown()
    {
        OnRollButtonDownEvent?.Invoke();

        rollPowerSlider.gameObject.SetActive(true);
    }

    private void OnRollButtonUp()
    {
        OnRollButtonUpEvent?.Invoke();

        rollPowerSlider.gameObject.SetActive(false);
    }

    private void OnRollPowerChanged(float rollPower)
    {
        float rollPowerNormalized = (rollPower - RollManager.Instance.RollPowerMin) / (RollManager.Instance.RollPowerMax - RollManager.Instance.RollPowerMin);
        rollPowerSlider.value = rollPowerNormalized;
    }
}
