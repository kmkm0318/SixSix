using System;
using System.Collections;
using UnityEngine;

public class RollManager : Singleton<RollManager>
{
    [SerializeField] private int rollMax;
    [SerializeField] private float rollPowerMax = 10f;
    public float RollPowerMax => rollPowerMax;
    [SerializeField] private float rollPowerMin = 1f;
    public float RollPowerMin => rollPowerMin;

    public event Action<float> OnRollPowerChanged;
    public event Action<float> OnRollPowerApplied;
    public event Action OnRollStarted;
    public event Action OnRollCompleted;
    public event Action<int> OnRollRemainChanged;

    public bool IsRolling { get; private set; } = false;

    private int currentRollMax;
    private int rollRemain = 0;
    public int RollRemain
    {
        get => rollRemain;
        set
        {
            if (rollRemain == value) return;
            rollRemain = value;
            OnRollRemainChanged?.Invoke(rollRemain);
        }
    }
    private float rollPower;
    private float powerChangeSpeed;
    private Coroutine ChangingRollPowerCoroutine;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        rollMax = DataContainer.Instance.CurrentDiceStat.defaultMaxRoll;
        currentRollMax = rollMax;
        powerChangeSpeed = rollPowerMax - rollPowerMin;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        PlayManager.Instance.OnPlayStarted += OnPlayStarted;

        RollUI.Instance.OnRollButtonPressed += OnRollButtonPressed;
        RollUI.Instance.OnRollButtonReleased += OnRollButtonReleased;

        EnhanceManager.Instance.OnDiceEnhanceStarted += OnDiceEnhanceStarted;
        EnhanceManager.Instance.OnHandEnhanceStarted += OnHandEnhanceStarted;

        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
    }

    private void OnPlayStarted(int playRemain)
    {
        RollRemain = currentRollMax;
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

    private void OnDiceEnhanceStarted()
    {
        RollRemain = currentRollMax;
    }

    private void OnHandEnhanceStarted()
    {
        RollRemain = currentRollMax;
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.RollMax)
        {
            SetRollMax(rollMax + 1, false);
        }
    }
    #endregion

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
        yield return new WaitUntil(() => DiceManager.Instance.AreAllDiceStopped());
        OnRollCompleted?.Invoke();
    }

    public void SetRollMax(int value, bool resetRollRemain = true)
    {
        rollMax = value;
        currentRollMax = rollMax;

        if (resetRollRemain)
        {
            RollRemain = rollMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    public void SetCurrentRollMax(int value = -1, bool resetRollRemain = true)
    {
        if (value == -1) value = rollMax;
        currentRollMax = value;

        if (resetRollRemain)
        {
            RollRemain = currentRollMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }
}
