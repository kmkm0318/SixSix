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
    }

    private void OnRollButtonDown()
    {
        OnRollButtonDownEvent?.Invoke();
    }

    private void OnRollButtonUp()
    {
        OnRollButtonUpEvent?.Invoke();
    }
}
