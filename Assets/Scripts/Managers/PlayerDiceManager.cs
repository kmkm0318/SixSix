using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceManager : Singleton<PlayerDiceManager>
{
    [SerializeField] private PlayDice playDicePrefab;
    [SerializeField] private AvailityDice availityDicePrefab;
    [SerializeField] private Playboard playboard;
    [SerializeField] private int firstdiceCount = 5;
    [SerializeField] private float diceGenerateDelay = 0.25f;

    public event Action OnFirstDiceGenerated;

    private List<PlayDice> playDiceList = new();
    public List<PlayDice> PlayDiceList => playDiceList;
    private List<AvailityDice> availityDiceList = new();
    public List<AvailityDice> AvailityDiceList => availityDiceList;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Loading)
        {
            SequenceManager.Instance.AddCoroutine(FirstDiceGenerate());
        }
    }

    private IEnumerator FirstDiceGenerate()
    {
        for (int i = 0; i < firstdiceCount; i++)
        {
            yield return new WaitForSeconds(diceGenerateDelay);
            var dice = Instantiate(playDicePrefab, playboard.DiceGeneratePosition, Quaternion.identity);
            dice.Init(6, DataContainer.Instance.DefaultDiceList, playboard);

            Instance.AddPlayDice(dice);
        }

        yield return new WaitUntil(() => AreAllDiceStopped());

        OnFirstDiceGenerated?.Invoke();
    }

    private void AddPlayDice(PlayDice playDice)
    {
        playDiceList.Add(playDice);
    }

    private void AddAvailityDice(AvailityDice availityDice)
    {
        availityDiceList.Add(availityDice);
    }

    public bool AreAllDiceStopped()
    {
        return playDiceList.TrueForAll(dice => !dice.IsRolling) && availityDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    public List<PlayDice> GetOrderedPlayDiceList()
    {
        List<PlayDice> orderedList = new(playDiceList);
        orderedList.Sort((a, b) => a.FaceIndex.CompareTo(b.FaceIndex));
        return orderedList;
    }

    public List<int> GetOrderedPlayDiceValues()
    {
        List<int> playDiceValues = new();
        foreach (Dice dice in playDiceList)
        {
            playDiceValues.Add(dice.FaceIndex + 1);
        }
        playDiceValues.Sort();
        return playDiceValues;
    }
}
