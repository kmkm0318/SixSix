using System;
using System.Collections;
using UnityEngine;

public class RollManager : Singleton<RollManager>
{
    [SerializeField] private float rollPowerMax = 10f;
    public float RollPowerMax => rollPowerMax;
    [SerializeField] private float rollPowerMin = 1f;
    public float RollPowerMin => rollPowerMin;

    public event Action<float> OnRollPowerChanged;
    public event Action<float> OnRollPowerApplied;
    public event Action OnRollStarted;
    public event Action OnRollCompleted;

    public bool IsRolling { get; private set; } = false;

    private float rollPower;
    private float powerChangeSpeed;
    private Coroutine ChangingRollPowerCoroutine;

    protected override void Awake()
    {
        base.Awake();

        powerChangeSpeed = rollPowerMax - rollPowerMin;
    }

    private void Start()
    {
        RollUI.Instance.OnRollButtonPressed += OnRollButtonPressed;
        RollUI.Instance.OnRollButtonReleased += OnRollButtonReleased;
    }

    private void OnRollButtonPressed()
    {
        ChangingRollPowerCoroutine = StartCoroutine(ChangingRollPower());
    }

    private void OnRollButtonReleased()
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
                OnRollPowerChanged?.Invoke(rollPower);
                yield return null;
            }
            rollPower = rollPowerMax;

            while (rollPower > rollPowerMin)
            {
                rollPower = Mathf.Clamp(rollPower - powerChangeSpeed * Time.deltaTime, 0f, rollPowerMax);
                OnRollPowerChanged?.Invoke(rollPower);
                yield return null;
            }
            rollPower = rollPowerMin;
        }
    }

    private void RollDice()
    {
        OnRollPowerApplied?.Invoke(rollPower);
        OnRollStarted?.Invoke();
        StartCoroutine(WaitForAllDiceToStop());
    }

    private IEnumerator WaitForAllDiceToStop()
    {
        yield return null;
        yield return new WaitUntil(() => PlayerDiceManager.Instance.AreAllDiceStopped());
        OnRollCompleted?.Invoke();
    }
}
