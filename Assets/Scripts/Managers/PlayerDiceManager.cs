using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceManager : Singleton<PlayerDiceManager>
{
    [SerializeField] private PlayDice playDicePrefab;
    [SerializeField] private AvailityDice availityDicePrefab;
    [SerializeField] private Playboard playDicePlayboard;
    [SerializeField] private Playboard availityDicePlayboard;
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
            StartCoroutine(FirstDiceGenerate());
        }
    }

    private IEnumerator FirstDiceGenerate()
    {
        for (int i = 0; i < firstdiceCount; i++)
        {
            yield return new WaitForSeconds(diceGenerateDelay);
            var playDice = Instantiate(playDicePrefab, playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
            playDice.Init(6, DataContainer.Instance.DefaultDiceList, playDicePlayboard);

            AddPlayDice(playDice);
        }

        if (DataContainer.Instance.AvailityDiceListSO.availityDiceSOList.Count > 0)
        {
            var availityDiceSO = DataContainer.Instance.AvailityDiceListSO.availityDiceSOList[0];
            var availityDice = Instantiate(availityDicePrefab, availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
            availityDice.Init(availityDiceSO, availityDicePlayboard);
            AddAvailityDice(availityDice);
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

    #region ApplyDice
    public void ApplyPlayDices()
    {
        var playDiceList = Instance.GetOrderedPlayDiceList();

        foreach (var playDice in playDiceList)
        {
            var scorePiarList = playDice.GetScorePairList();
            foreach (var scorePair in scorePiarList)
            {
                if (scorePair.baseScore == 0 && scorePair.multiplier == 0) continue;

                ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(playDice, scorePair);

                ApplyAvailityDiceOnPlayDiceApplied(playDice);
            }
        }
    }

    private void ApplyAvailityDiceOnPlayDiceApplied(PlayDice playDice)
    {
        List<AvailityDice> triggeredAvailityDiceList = availityDiceList.FindAll(dice => dice.IsTriggeredByPlayDice(playDice));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.ApplyEffect();
        }
    }

    public void ApplyAvailityDiceOnHandCategoryApplied(HandCategorySO handCategorySO)
    {
        List<AvailityDice> triggeredAvailityDiceList = availityDiceList.FindAll(dice => dice.IsTriggeredByHandCategory(handCategorySO));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.ApplyEffect();
        }
    }
    #endregion
}
