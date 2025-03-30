using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceManager : Singleton<PlayerDiceManager>
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Playboard playboard;
    [SerializeField] private int firstdiceCount = 5;
    [SerializeField] private float diceGenerateDelay = 0.25f;

    public event Action OnFirstDiceGenerated;

    private List<Dice> playDiceList = new();
    public List<Dice> PlayDiceList => playDiceList;
    private List<Dice> availityDiceList = new();
    public List<Dice> AvailityDiceList => availityDiceList;
    private bool isLoadingTaskComplete = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        GameManager.Instance.RegisterStateCheckTask(GameManager.GameState.Loading, LoadingCheckTask);
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Loading)
        {
            isLoadingTaskComplete = false;
            StartCoroutine(FirstDiceGenerate());
        }
    }

    private bool LoadingCheckTask()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Loading)
        {
            return isLoadingTaskComplete;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator FirstDiceGenerate()
    {
        for (int i = 0; i < firstdiceCount; i++)
        {
            yield return new WaitForSeconds(diceGenerateDelay);
            var dice = Instantiate(dicePrefab, playboard.DiceGeneratePosition, Quaternion.identity);
            dice.Init(6, playboard);

            Instance.AddPlayDice(dice);
        }
        isLoadingTaskComplete = true;

        OnFirstDiceGenerated?.Invoke();
    }

    private void AddPlayDice(Dice playDice)
    {
        playDiceList.Add(playDice);
    }

    private void AddAvailityDice(Dice availityDice)
    {
        availityDiceList.Add(availityDice);
    }

    public bool AreAllDiceStopped()
    {
        return playDiceList.TrueForAll(dice => !dice.IsRolling) && availityDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    public List<int> GetPlayDiceValues()
    {
        List<int> playDiceValues = new();
        foreach (Dice dice in playDiceList)
        {
            playDiceValues.Add(dice.FaceIndex + 1);
        }
        return playDiceValues;
    }
}
