using System;
using System.Collections;
using UnityEngine;

public class RollManager : MonoBehaviour
{
    [SerializeField] private float powerChangeSpeed = 1f;
    [SerializeField] private float rollPowerMax = 10f;

    public event Action<float> OnRollPowerChangedEvent;
    public event Action<float> OnRollDiceEvent;

    private float rollPower;
    private Coroutine ChangingRollPowerCoroutine;

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
        rollPower = 0f;
        while (true)
        {
            while (rollPower < rollPowerMax)
            {
                rollPower = Mathf.Clamp(rollPower + powerChangeSpeed * Time.deltaTime, 0f, rollPowerMax);
                OnRollPowerChangedEvent?.Invoke(rollPower);
                yield return null;
            }

            while (rollPower > 0f)
            {
                rollPower = Mathf.Clamp(rollPower - powerChangeSpeed * Time.deltaTime, 0f, rollPowerMax);
                OnRollPowerChangedEvent?.Invoke(rollPower);
                yield return null;
            }
            rollPower = 0f;
        }
    }

    private void RollDice()
    {
        OnRollDiceEvent?.Invoke(rollPower);
    }
}
