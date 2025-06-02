using System.Collections;
using UnityEngine;

public class GameSpeedManager : Singleton<GameSpeedManager>
{
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float animationSpeedMax = 10f;
    [SerializeField] private float speedUpInterval = 1f;
    [SerializeField] private float speedUpAmount = 0.01f;

    private float gameSpeed = 1f;
    private Coroutine speedupCoroutine;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        OnGameSpeedChanged(OptionManager.Instance.OptionData.gameSpeed);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        OptionUI.Instance.RegisterOnOptionValueChanged(OptionType.GameSpeed, OnGameSpeedChanged);
        SequenceManager.Instance.OnAnimationStarted += OnAnimationStarted;
        SequenceManager.Instance.OnAnimationFinished += OnAnimationFinished;
    }

    private void OnGameSpeedChanged(int value)
    {
        float newSpeed = 1f + value * 0.25f;
        ChangeGameSpeed(newSpeed);
    }

    private void OnAnimationStarted()
    {
        StartSpeedUpCoroutine();
    }

    private void OnAnimationFinished()
    {
        StopSpeedUpCoroutine();
    }
    #endregion

    private void ChangeGameSpeed(float newSpeed)
    {
        gameSpeed = newSpeed;
        SetTimeScale();
    }

    private void ChangeAnimationSpeed(float newSpeed)
    {
        animationSpeed = newSpeed;
        SetTimeScale();
    }

    private void SetTimeScale()
    {
        Time.timeScale = gameSpeed * animationSpeed;
    }

    private void StartSpeedUpCoroutine()
    {
        speedupCoroutine = StartCoroutine(SpeedUpCoroutine());
    }

    private void StopSpeedUpCoroutine()
    {
        if (speedupCoroutine != null)
        {
            StopCoroutine(speedupCoroutine);
            speedupCoroutine = null;
        }
        ChangeAnimationSpeed(1f);
    }

    private IEnumerator SpeedUpCoroutine()
    {
        while (true)
        {
            if (animationSpeed < animationSpeedMax)
            {
                animationSpeed += speedUpAmount;
                SetTimeScale();
            }
            yield return new WaitForSeconds(speedUpInterval);
        }
    }
}