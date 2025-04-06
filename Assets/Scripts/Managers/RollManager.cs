using System;
using System.Collections;
using UnityEngine;

public class RollManager : Singleton<RollManager>
{
    [SerializeField] private int rollMax = 3;
    [SerializeField] private float rollPowerMax = 10f;
    public float RollPowerMax => rollPowerMax;
    [SerializeField] private float rollPowerMin = 1f;
    public float RollPowerMin => rollPowerMin;

    public event Action<float> OnRollPowerChanged;
    public event Action<float> OnRollPowerApplied;
    public event Action OnRollStarted;
    public event Action OnRollCompleted;
    public event Action<int> RollRemainChanged;

    public bool IsRolling { get; private set; } = false;

    private int rollRemain = 0;
    public int RollRemain
    {
        get => rollRemain;
        private set
        {
            if (rollRemain == value) return;
            rollRemain = value;
            RollRemainChanged?.Invoke(rollRemain);
        }
    }
    private float rollPower;
    private float powerChangeSpeed;
    private Coroutine ChangingRollPowerCoroutine;

    private void Start()
    {
        powerChangeSpeed = rollPowerMax - rollPowerMin;


        PlayManager.Instance.OnPlayStarted += OnPlayStarted;
        RollUI.Instance.OnRollButtonPressed += OnRollButtonPressed;
        RollUI.Instance.OnRollButtonReleased += OnRollButtonReleased;
    }

    private void OnPlayStarted(int playRemain)
    {
        RollRemain = rollMax;
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
        RollRemain--;

        StartCoroutine(WaitForAllDiceToStop());
    }

    private IEnumerator WaitForAllDiceToStop()
    {
        yield return null;
        yield return new WaitUntil(() => PlayerDiceManager.Instance.AreAllDiceStopped());
        OnRollCompleted?.Invoke();
    }
}
