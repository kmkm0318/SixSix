using System.Collections;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour
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

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Init()
    {
        OnGameSpeedChanged(OptionManager.Instance.OptionData.gameSpeed);
    }

    #region Events
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged += OnOptionValueChanged;
        SequenceManager.Instance.OnAnimationStarted += OnAnimationStarted;
        SequenceManager.Instance.OnAnimationFinished += OnAnimationFinished;
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged -= OnOptionValueChanged;
        SequenceManager.Instance.OnAnimationStarted -= OnAnimationStarted;
        SequenceManager.Instance.OnAnimationFinished -= OnAnimationFinished;
    }

    private void OnOptionValueChanged(OptionType type, int value)
    {
        if (type == OptionType.GameSpeed)
        {
            OnGameSpeedChanged(value);
        }
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
        SetSFXPitchBias();
    }

    private void SetTimeScale()
    {
        Time.timeScale = gameSpeed * animationSpeed;
    }

    private void SetSFXPitchBias()
    {
        float bias = Mathf.Lerp(1, 1.5f, animationSpeed / animationSpeedMax);
        AudioManager.Instance.SetSFXPitchBias(bias);
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
                ChangeAnimationSpeed(animationSpeed + speedUpAmount);
            }
            yield return new WaitForSeconds(speedUpInterval);
        }
    }
}