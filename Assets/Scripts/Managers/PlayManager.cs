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
    }

    private void Init()
    {
        playMax = DataContainer.Instance.CurrentDiceStat.defaultPlayMax;
        currentPlayMax = playMax;
    }

    public void ResetPlayRemain()
    {
        PlayRemain = currentPlayMax;
    }

    public void EndPlay()
    {
        PlayRemain--;
    }

    public void HandlePlayResult()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Round) return;

        if (ScoreManager.Instance.CurrentRoundScore >= ScoreManager.Instance.TargetRoundScore)
        {
            if (RoundManager.Instance.CurrentRound == RoundManager.Instance.ClearRound)
            {
                GameManager.Instance.ChangeState(GameState.GameResult);
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.RoundClear);
            }
        }
        else if (PlayRemain == 0)
        {
            GameManager.Instance.ChangeState(GameState.GameResult);
        }
        else
        {
            GameManager.Instance.ChangeState(GameState.Play);
        }
    }

    public void IncreasePlayMaxAndRemain(int value = 1)
    {
        playMax += value;
        PlayRemain += value;
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
