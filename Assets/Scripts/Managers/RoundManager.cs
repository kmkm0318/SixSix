using System;
using UnityEngine;

public class RoundManager : Singleton<RoundManager>
{
    [SerializeField] private int clearRound = 36;
    [SerializeField] private int bossRoundInterval = 6;
    private int currentRound = 0;

    public int ClearRound => clearRound;
    public bool IsBossRound => (CurrentRound % bossRoundInterval) == 0 && CurrentRound != 0;
    public int CurrentRound
    {
        get => currentRound;
        private set
        {
            if (currentRound == value) return;
            currentRound = value;
            OnCurrentRoundChanged?.Invoke(currentRound);
        }
    }

    public event Action<int> OnCurrentRoundChanged;

    public void StartNextRound()
    {
        CurrentRound++;
        GameManager.Instance.ChangeState(GameState.Round);
    }
}
