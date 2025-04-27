using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerDiceManager : Singleton<PlayerDiceManager>
{
    [SerializeField] private PlayDice playDicePrefab;
    [SerializeField] private AvailityDice availityDicePrefab;
    [SerializeField] private ChaosDice chaosDicePrefab;
    [SerializeField] private Playboard playDicePlayboard;
    [SerializeField] private Playboard availityDicePlayboard;
    [SerializeField] private int firstdiceCount = 5;
    [SerializeField] private float diceGenerateDelay = 0.25f;
    [SerializeField] private int availityDiceCountMax = 5;
    public int AvailityDiceCountMax => availityDiceCountMax;
    [SerializeField] private int defaultChaosDiceFaceValueMax = 4;


    public event Action OnFirstDiceGenerated;
    public event Action<PlayDice> OnPlayDiceClicked;
    public event Action<AvailityDice> OnAvailityDiceClicked;
    public event Action<ChaosDice> OnChaosDiceClicked;

    private List<PlayDice> playDiceList = new();
    public List<PlayDice> PlayDiceList => playDiceList;
    private List<AvailityDice> availityDiceList = new();
    public List<AvailityDice> AvailityDiceList => availityDiceList;
    private List<ChaosDice> chaosDiceList = new();
    public List<ChaosDice> ChaosDiceList => chaosDiceList;
    private ObjectPool<PlayDice> playDicePool;
    private ObjectPool<AvailityDice> availityDicePool;
    private ObjectPool<ChaosDice> chaosDicePool;
    private bool isAvailityDiceAutoKeep = false;
    public bool IsAvailityDiceAutoKeep => isAvailityDiceAutoKeep;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        OnAvailityDiceAutoKeepChanged(OptionManager.Instance.OptionData.availityDiceAutoKeep);

        playDicePool = new ObjectPool<PlayDice>(() => Instantiate(playDicePrefab), playDice => { }, playDice => playDice.gameObject.SetActive(false), playDice => Destroy(playDice.gameObject), false);
        availityDicePool = new ObjectPool<AvailityDice>(() => Instantiate(availityDicePrefab), availityDice => { }, availityDice => availityDice.gameObject.SetActive(false), availityDice => Destroy(availityDice.gameObject), false);
        chaosDicePool = new ObjectPool<ChaosDice>(() => Instantiate(chaosDicePrefab), chaosDice => { }, chaosDice => chaosDice.gameObject.SetActive(false), chaosDice => Destroy(chaosDice.gameObject), false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        BonusManager.Instance.OnBonusAchieved += OnBonusAchieved;
        OptionUI.Instance.RegisterOnOptionValueChanged(OptionType.AvailityDiceAutoKeep, OnAvailityDiceAutoKeepChanged);
        ShopManager.Instance.OnAvailityDicePurchaseAttempted += OnPurchaseAttempted;
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

            var playDice = playDicePool.Get();
            playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
            playDice.Init(6, DataContainer.Instance.DefaultDiceList, playDicePlayboard);
            playDice.gameObject.SetActive(true);

            AddPlayDice(playDice);
        }

        yield return new WaitUntil(() => AreAllDiceStopped());

        OnFirstDiceGenerated?.Invoke();
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.Dice)
        {
            SequenceManager.Instance.AddCoroutine(AddSixthDice());
        }
    }

    private IEnumerator AddSixthDice()
    {
        var playDice = playDicePool.Get();
        playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        playDice.Init(6, DataContainer.Instance.DefaultDiceList, playDicePlayboard);
        playDice.gameObject.SetActive(true);

        AddPlayDice(playDice);
        yield return new WaitUntil(() => AreAllDiceStopped());
    }

    private void OnAvailityDiceAutoKeepChanged(int value)
    {
        isAvailityDiceAutoKeep = value == 1;
    }

    private void OnPurchaseAttempted(AvailityDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (result == PurchaseResult.Success)
        {
            var availityDice = availityDicePool.Get();
            availityDice.transform.SetPositionAndRotation(availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
            availityDice.Init(sO, availityDicePlayboard);
            availityDice.gameObject.SetActive(true);

            AddAvailityDice(availityDice);
        }
    }
    #endregion

    #region Dice List Function
    private void AddPlayDice(PlayDice playDice)
    {
        playDice.OnMouseClicked += () => OnPlayDiceClicked?.Invoke(playDice);
        playDiceList.Add(playDice);
    }

    private void AddAvailityDice(AvailityDice availityDice)
    {
        availityDice.OnMouseClicked += () => OnAvailityDiceClicked?.Invoke(availityDice);
        availityDiceList.Add(availityDice);
    }

    private void AddChaosDice(ChaosDice chaosDice)
    {
        chaosDice.OnMouseClicked += () => OnChaosDiceClicked?.Invoke(chaosDice);
        chaosDiceList.Add(chaosDice);
    }

    public void RemovePlayDice(PlayDice playDice)
    {
        playDice.ResetMouseClickEvent();
        playDiceList.Remove(playDice);
        playDicePool.Release(playDice);
    }

    public void RemoveAvailityDice(AvailityDice availityDice)
    {
        availityDice.ResetMouseClickEvent();
        availityDiceList.Remove(availityDice);
        availityDicePool.Release(availityDice);
    }

    public void RemoveChaosDice(ChaosDice chaosDice)
    {
        chaosDice.ResetMouseClickEvent();
        chaosDiceList.Remove(chaosDice);
        chaosDicePool.Release(chaosDice);
    }

    public void RespawnPlayDice(PlayDice playDice)
    {
        playDice.gameObject.SetActive(true);
        playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
    }

    public void RespawnAvailityDice(AvailityDice availityDice)
    {
        availityDice.gameObject.SetActive(true);
        availityDice.transform.SetPositionAndRotation(availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
    }

    public bool AreAllDiceStopped()
    {
        return playDiceList.TrueForAll(dice => !dice.IsRolling) && availityDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    private List<PlayDice> GetOrderedPlayDiceList()
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
            playDiceValues.Add(dice.FaceValue);
        }
        playDiceValues.Sort();
        return playDiceValues;
    }

    public PlayDice GetRandomPlayDice()
    {
        if (playDiceList.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, playDiceList.Count);
        return playDiceList[randomIndex];
    }

    public AvailityDice GetRandomAvailityDice()
    {
        if (availityDiceList.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, availityDiceList.Count);
        return availityDiceList[randomIndex];
    }
    #endregion

    #region ApplyDice
    public void ApplyPlayDices()
    {
        var playDiceList = Instance.GetOrderedPlayDiceList();

        foreach (var playDice in playDiceList)
        {
            playDice.ApplyScorePairs();
            ApplyAvailityDiceOnPlayDiceApplied(playDice);
        }
    }

    private void ApplyAvailityDiceOnPlayDiceApplied(PlayDice playDice)
    {
        List<AvailityDice> triggeredAvailityDiceList = availityDiceList.FindAll(dice => dice.IsTriggered(new(playDice: playDice)));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.ApplyEffect();
        }
    }

    public void ApplyAvailityDiceOnHandApplied(HandSO handSO)
    {
        List<AvailityDice> triggeredAvailityDiceList = availityDiceList.FindAll(dice => dice.IsTriggered(new(handSO: handSO)));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            availityDice.ApplyEffect();
        }
    }
    #endregion
}
