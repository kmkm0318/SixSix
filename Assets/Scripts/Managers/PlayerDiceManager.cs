using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    [SerializeField] private int defaultPlayDiceFaceValueMax = 6;
    [SerializeField] private int defaultChaosDiceFaceValueMax = 4;


    public event Action OnFirstDiceGenerated;
    public event Action<PlayDice> OnPlayDiceClicked;
    public event Action<AvailityDice> OnAvailityDiceClicked;
    public event Action<ChaosDice> OnChaosDiceClicked;
    public event Action<int> OnAvailityDiceCountChanged;
    public event Action<int> OnAvailityDiceCountMaxChanged;

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
    public List<int> UsableFaceValues { get; set; }
    public bool IsKeepable { get; set; } = true;

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
            GeneratePlayDice();
        }

        yield return new WaitUntil(() => AreAllDiceStopped());

        OnFirstDiceGenerated?.Invoke();
    }

    private void GeneratePlayDice()
    {
        var playDice = playDicePool.Get();
        playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        playDice.Init(defaultPlayDiceFaceValueMax, DataContainer.Instance.DefaultDiceList, DataContainer.Instance.DefaultDiceMaterial, playDicePlayboard);
        playDice.gameObject.SetActive(true);

        AddPlayDice(playDice);
    }

    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.PlayDice)
        {
            SequenceManager.Instance.AddCoroutine(AddSixthDice());
        }
        else if (type == BonusType.AvailityDiceCountMax)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                availityDiceCountMax += 1;
                OnAvailityDiceCountMaxChanged?.Invoke(availityDiceCountMax);
            });
        }
    }

    private IEnumerator AddSixthDice()
    {
        GeneratePlayDice();

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

        OnAvailityDiceCountChanged?.Invoke(availityDiceList.Count);
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

        OnAvailityDiceCountChanged?.Invoke(availityDiceList.Count);
    }

    private void RemoveChaosDice(ChaosDice chaosDice)
    {
        chaosDice.ResetMouseClickEvent();
        chaosDiceList.Remove(chaosDice);
        chaosDicePool.Release(chaosDice);
    }

    public void EnablePlayDice(PlayDice playDice)
    {
        playDice.IsEnabled = true;

        playDiceList.Add(playDice);
    }

    public void DisablePlayDice(PlayDice playDice)
    {
        playDice.IsEnabled = false;

        playDiceList.Remove(playDice);
    }

    public void EnableAvailityDice(AvailityDice availityDice)
    {
        availityDice.IsEnabled = true;

        availityDiceList.Add(availityDice);
    }

    public void DisableAvailityDice(AvailityDice availityDice)
    {
        availityDice.IsEnabled = false;

        availityDiceList.Remove(availityDice);
    }

    public bool AreAllDiceStopped()
    {
        List<Dice> diceList = new();

        diceList.AddRange(playDiceList);
        diceList.AddRange(availityDiceList);
        diceList.AddRange(chaosDiceList);

        return diceList.TrueForAll(dice => !dice.IsRolling);
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

    public List<PlayDice> GetRandomPlayDiceList(int count)
    {
        List<PlayDice> res = new();

        if (PlayDiceList.Count <= count)
        {
            res.AddRange(PlayDiceList);
            return res;
        }
        else
        {
            var cloneList = new List<PlayDice>(playDiceList);
            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, cloneList.Count);
                res.Add(cloneList[randomIndex]);
                cloneList.RemoveAt(randomIndex);
            }
            return res;
        }
    }

    public List<AvailityDice> GetRandomAvailityDiceList(int count)
    {
        List<AvailityDice> res = new();

        if (AvailityDiceList.Count <= count)
        {
            res.AddRange(AvailityDiceList);
            return res;
        }
        else
        {
            var cloneList = new List<AvailityDice>(availityDiceList);
            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, cloneList.Count);
                res.Add(cloneList[randomIndex]);
                cloneList.RemoveAt(randomIndex);
            }
            return res;
        }
    }

    public void GenerateChaosDices(int chaosDiceCount)
    {
        SequenceManager.Instance.AddCoroutine(AddChaosDiceCoroutine(chaosDiceCount));
    }

    private IEnumerator AddChaosDiceCoroutine(int chaosDiceCount)
    {
        for (int i = 0; i < chaosDiceCount; i++)
        {
            yield return new WaitForSeconds(diceGenerateDelay);
            var chaosDice = chaosDicePool.Get();
            chaosDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
            chaosDice.Init(defaultChaosDiceFaceValueMax, DataContainer.Instance.DefaultDiceList, DataContainer.Instance.ChaosDiceMaterial, playDicePlayboard);
            chaosDice.gameObject.SetActive(true);

            AddChaosDice(chaosDice);
        }

        yield return new WaitUntil(() => AreAllDiceStopped());
    }

    public void ClearChaosDices()
    {
        while (chaosDiceList.Count > 0)
        {
            RemoveChaosDice(chaosDiceList[0]);
        }
    }
    #endregion

    #region ApplyDice
    public void ApplyPlayDices()
    {
        var playDiceList = Instance.GetOrderedPlayDiceList();

        foreach (var playDice in playDiceList)
        {
            if (UsableFaceValues != null && !UsableFaceValues.Contains(playDice.FaceValue)) continue;

            playDice.ApplyScorePairs();
            ApplyAvailityDice(playDice);
        }
    }

    private void ApplyAvailityDice(AvailityTriggerType triggerType, AvailityDiceContext context)
    {
        List<AvailityDice> triggeredAvailityDiceList = availityDiceList.FindAll(dice => dice.IsTriggered(triggerType, context));

        foreach (var availityDice in triggeredAvailityDiceList)
        {
            if (UsableFaceValues != null && !UsableFaceValues.Contains(availityDice.FaceValue)) continue;

            availityDice.ApplyEffect();
        }
    }

    private void ApplyAvailityDice(AvailityTriggerType triggerType)
    {
        ApplyAvailityDice(triggerType, new());
    }

    private void ApplyAvailityDice(PlayDice playDice)
    {
        ApplyAvailityDice(AvailityTriggerType.PlayDice, new(playDice: playDice));
    }

    public void ApplyAvailityDice(HandSO handSO)
    {
        ApplyAvailityDice(AvailityTriggerType.Hand, new(handSO: handSO));
    }

    public void ApplyChaosDices()
    {
        foreach (var chaosDice in chaosDiceList)
        {
            chaosDice.ApplyScorePairs();
        }
    }
    #endregion
}
