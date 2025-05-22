using System;
using System.Collections;
using UnityEngine;

public class RollManager : Singleton<RollManager>
{
    [SerializeField] private int rollMax;
    [SerializeField] private float rollPowerMax = 10f;
    [SerializeField] private float rollPowerMin = 1f;

    private int currentRollMax;
    private int rollRemain = 0;
    private float rollPower;
    private float powerChangeSpeed;
    private Coroutine ChangingRollPowerCoroutine;

    public float RollPowerMax => rollPowerMax;
    public float RollPowerMin => rollPowerMin;
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

    public event Action<float> OnRollPowerChanged;
    public event Action<float> OnRollPowerApplied;
    public event Action<int> OnRollRemainChanged;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        rollMax = DataContainer.Instance.CurrentDiceStat.defaultRollMax;
        currentRollMax = rollMax;
        powerChangeSpeed = rollPowerMax - rollPowerMin;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RollUI.Instance.OnRollButtonPressed += OnRollButtonPressed;
        RollUI.Instance.OnRollButtonReleased += OnRollButtonReleased;
    }

    public void ResetRollRemain()
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
        GameManager.Instance.ChangeState(GameState.Roll);
        OnRollPowerApplied?.Invoke(rollPower);
        RollRemain--;

        StartCoroutine(WaitForAllDiceToStop(() =>
        {
            GameManager.Instance.ExitState(GameState.Roll);
        }));
    }

    private IEnumerator WaitForAllDiceToStop(Action onComplete = null)
    {
        yield return null;
        yield return new WaitUntil(() => DiceManager.Instance.AreAllDiceStopped());

        onComplete?.Invoke();
    }


    public void IncreaseRollMax()
    {
        SetRollMax(rollMax + 1, false);
    }

    private void SetRollMax(int value, bool resetRollRemain = true)
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
