using System;
using System.Diagnostics;

public class PlayManager : Singleton<PlayManager>
{
    private int playMax;
    private int currentPlayMax;
    private int playRemain = 0;

    public int PlayRemain
    {
        get => playRemain;
        set
        {
            if (playRemain == value) return;
            playRemain = value;
            OnPlayRemainChanged?.Invoke(playRemain);
        }
    }

    public event Action<int> OnPlayRemainChanged;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        playMax = DataContainer.Instance.CurrentDiceStat.defaultPlayMax;
        currentPlayMax = playMax;
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        ScoreManager.Instance.OnCurrentRoundScoreUpdated += OnCurrentRoundScoreUpdated;
    }

    private void OnCurrentRoundScoreUpdated(float score)
    {
        EndPlay();
    }
    #endregion

    public void StartPlay(bool isInit = false)
    {
        if (isInit)
        {
            PlayRemain = currentPlayMax;
        }
        GameManager.Instance.ChangeState(GameState.Play);
    }

    public void EndPlay()
    {
        PlayRemain--;
        GameManager.Instance.ExitState(GameState.Play);
    }

    public void HandlePlayResult()
    {
        if (ScoreManager.Instance.CurrentRoundScore >= ScoreManager.Instance.TargetRoundScore)
        {
            if (RoundManager.Instance.CurrentRound == RoundManager.Instance.ClearRound)
            {
                GameManager.Instance.HandleGameResult(true);
            }
            else
            {
                RoundClearManager.Instance.StartRoundClear();
            }
        }
        else if (PlayRemain == 0)
        {
            GameManager.Instance.HandleGameResult(false);
        }
        else
        {
            StartPlay();
        }
    }

    public void IncreasePlayMax()
    {
        SetPlayMax(playMax + 1, false);
    }

    private void SetPlayMax(int value, bool resetPlayRemain = true)
    {
        playMax = value;
        currentPlayMax = playMax;

        if (resetPlayRemain)
        {
            PlayRemain = playMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }

    public void SetCurrentPlayMax(int value = -1, bool resetPlayRemain = true)
    {
        if (value == -1) value = playMax;
        currentPlayMax = value;

        if (resetPlayRemain)
        {
            PlayRemain = currentPlayMax;
            SequenceManager.Instance.ApplyParallelCoroutine();
        }
    }
}
