using System;
using System.Collections;
using UnityEngine;

public class RollManager : MonoBehaviour
{
    public static RollManager Instance { get; private set; }

    [SerializeField] private float rollPowerMax = 10f;
    public float RollPowerMax => rollPowerMax;
    [SerializeField] private float rollPowerMin = 1f;
    public float RollPowerMin => rollPowerMin;

    public event Action<float> OnRollPowerChangedEvent;
    public event Action<float> OnRollDiceEvent;

    private float rollPower;
    private float powerChangeSpeed;
    private Coroutine ChangingRollPowerCoroutine;

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

        powerChangeSpeed = rollPowerMax - rollPowerMin;
    }

    private void Start()
    {
        RollUI.Instance.OnRollButtonDownEvent += OnRollButtonDown;
        RollUI.Instance.OnRollButtonUpEvent += OnRollButtonUp;
    }

    private void OnRollButtonDown()
    {
        ChangingRollPowerCoroutine = StartCoroutine(ChangingRollPower());
    }

    private void OnRollButtonUp()
    {
        if (ChangingRollPowerCoroutine != null)
        {
            StopCoroutine(ChangingRollPowerCoroutine);
        }

        RollDice();
    }

    IEnumerator ChangingRollPower()
    {
        rollPower = rollPowerMin;
        while (true)
        {
            while (rollPower < rollPowerMax)
            {
                rollPower = Mathf.Clamp(rollPower + powerChangeSpeed * Time.deltaTime, 0f, rollPowerMax);
                OnRollPowerChangedEvent?.Invoke(rollPower);
                yield return null;
            }
            rollPower = rollPowerMax;

            while (rollPower > rollPowerMin)
            {
                rollPower = Mathf.Clamp(rollPower - powerChangeSpeed * Time.deltaTime, 0f, rollPowerMax);
                OnRollPowerChangedEvent?.Invoke(rollPower);
                yield return null;
            }
            rollPower = rollPowerMin;
        }
    }

    private void RollDice()
    {
        OnRollDiceEvent?.Invoke(rollPower);
    }
}
