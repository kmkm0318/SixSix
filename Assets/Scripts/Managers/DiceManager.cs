using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DiceManager : Singleton<DiceManager>
{
    [SerializeField] private PlayDice playDicePrefab;
    [SerializeField] private AvailityDice availityDicePrefab;
    [SerializeField] private ChaosDice chaosDicePrefab;
    [SerializeField] private GambleDice gambleDicePrefab;
    [SerializeField] private Playboard playDicePlayboard;
    [SerializeField] private Playboard availityDicePlayboard;
    [SerializeField] private float diceGenerateDelay = 0.25f;
    [SerializeField] private int defaultPlayDiceValueMax = 6;
    [SerializeField] private int defaultChaosDiceValueMax = 4;
    [SerializeField] private int gambleDiceNumMax = 3;

    private List<PlayDice> playDiceList = new();
    private List<AvailityDice> availityDiceList = new();
    private List<ChaosDice> chaosDiceList = new();
    private List<GambleDice> gambleDiceList = new();
    private ObjectPool<PlayDice> playDicePool;
    private ObjectPool<AvailityDice> availityDicePool;
    private ObjectPool<ChaosDice> chaosDicePool;
    private ObjectPool<GambleDice> gambleDicePool;
    private bool isAvailityDiceAutoKeep = false;
    private int currentAvailityDiceMax = 0;

    public List<PlayDice> PlayDiceList => playDiceList;
    public List<AvailityDice> AvailityDiceList => availityDiceList;
    public List<ChaosDice> ChaosDiceList => chaosDiceList;
    public List<GambleDice> GambleDiceList => gambleDiceList;
    public List<Dice> AllDiceList
    {
        get
        {
            List<Dice> allDiceList = new();
            allDiceList.AddRange(playDiceList);
            allDiceList.AddRange(availityDiceList);
            allDiceList.AddRange(chaosDiceList);
            allDiceList.AddRange(gambleDiceList);
            return allDiceList;
        }
    }
    public int CurrentAvailityDiceMax
    {
        get => currentAvailityDiceMax;
        set
        {
            if (currentAvailityDiceMax == value) return;
            currentAvailityDiceMax = value;
            OnCurrentAvailityDiceMaxChanged?.Invoke(currentAvailityDiceMax);
        }
    }
    public bool IsAvailityDiceAutoKeep => isAvailityDiceAutoKeep;
    public List<int> UsableDiceValues { get; set; }
    public bool IsKeepable { get; set; } = true;

    public event Action<PlayDice> OnPlayDiceClicked;
    public event Action<AvailityDice> OnAvailityDiceClicked;
    public event Action<ChaosDice> OnChaosDiceClicked;
    public event Action<GambleDice> OnGambleDiceClicked;
    public event Action<int> OnAvailityDiceCountChanged;
    public event Action<int> OnCurrentAvailityDiceMaxChanged;

    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void Init()
    {
        OnAvailityDiceAutoKeepChanged(OptionManager.Instance.OptionData.availityDiceAutoKeep);

        playDicePool = new(() => Instantiate(playDicePrefab), playDice => { playDice.gameObject.SetActive(true); }, playDice => playDice.gameObject.SetActive(false), playDice => Destroy(playDice.gameObject), false);
        availityDicePool = new(() => Instantiate(availityDicePrefab), availityDice => { availityDice.gameObject.SetActive(true); }, availityDice => availityDice.gameObject.SetActive(false), availityDice => Destroy(availityDice.gameObject), false);
        chaosDicePool = new(() => Instantiate(chaosDicePrefab), chaosDice => { chaosDice.gameObject.SetActive(true); }, chaosDice => chaosDice.gameObject.SetActive(false), chaosDice => Destroy(chaosDice.gameObject), false);
        gambleDicePool = new(() => Instantiate(gambleDicePrefab), gambleDice => { gambleDice.gameObject.SetActive(true); }, gambleDice => gambleDice.gameObject.SetActive(false), gambleDice => Destroy(gambleDice.gameObject), false);

        CurrentAvailityDiceMax = DataContainer.Instance.DefaultAvailityDiceMax;
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
            StartCoroutine(FirstPlayDiceGenerate());
        }
    }


    private void OnBonusAchieved(BonusType type)
    {
        if (type == BonusType.PlayDice)
        {
            SequenceManager.Instance.AddCoroutine(AddBonusPlayDice());
        }
        else if (type == BonusType.AvailityDiceCountMax)
        {
            SequenceManager.Instance.AddCoroutine(() =>
            {
                CurrentAvailityDiceMax++;
            });
        }
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
            GenerateAvailityDice(sO);
        }
    }
    #endregion

    #region Play Dice
    private void GeneratePlayDice()
    {
        var playDice = playDicePool.Get();
        playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        playDice.Init(defaultPlayDiceValueMax, DataContainer.Instance.DefaultDiceSpriteList, DataContainer.Instance.DefaultDiceMaterial, playDicePlayboard);

        AddPlayDice(playDice);
    }

    private void AddPlayDice(PlayDice playDice)
    {
        playDiceList.Add(playDice);
    }

    public void RemovePlayDice(PlayDice playDice)
    {
        playDiceList.Remove(playDice);
        playDice.FadeOut(() =>
        {
            playDicePool.Release(playDice);
        });
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

    private IEnumerator FirstPlayDiceGenerate()
    {
        for (int i = 0; i < DataContainer.Instance.DefaultPlayDiceCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(diceGenerateDelay);

            GeneratePlayDice();
        }

        yield return new WaitUntil(() => AreAllDiceStopped());

        GameManager.Instance.CurrentGameState = GameState.Round;
    }

    private IEnumerator AddBonusPlayDice()
    {
        GeneratePlayDice();

        yield return new WaitUntil(() => AreAllDiceStopped());
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
            playDiceValues.Add(dice.DiceValue);
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
    #endregion

    #region Availity Dice
    private void GenerateAvailityDice(AvailityDiceSO sO)
    {
        var availityDice = availityDicePool.Get();
        availityDice.transform.SetPositionAndRotation(availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        availityDice.Init(sO, availityDicePlayboard);

        AddAvailityDice(availityDice);
    }

    private void AddAvailityDice(AvailityDice availityDice)
    {
        availityDiceList.Add(availityDice);

        OnAvailityDiceCountChanged?.Invoke(availityDiceList.Count);
    }

    public void RemoveAvailityDice(AvailityDice availityDice)
    {
        availityDiceList.Remove(availityDice);
        availityDice.FadeOut(() =>
        {
            availityDicePool.Release(availityDice);
        });

        OnAvailityDiceCountChanged?.Invoke(availityDiceList.Count);
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
    #endregion

    #region Chaos Dice
    private void GenerateChaosDice()
    {
        var chaosDice = chaosDicePool.Get();
        chaosDice.transform.SetPositionAndRotation(availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        chaosDice.Init(defaultChaosDiceValueMax, DataContainer.Instance.DefaultDiceSpriteList, DataContainer.Instance.DefaultDiceMaterial, availityDicePlayboard);

        AddChaosDice(chaosDice);
    }

    private void AddChaosDice(ChaosDice chaosDice)
    {
        chaosDiceList.Add(chaosDice);
    }

    private void RemoveChaosDice(ChaosDice chaosDice)
    {
        chaosDiceList.Remove(chaosDice);
        chaosDice.FadeOut(() =>
        {
            chaosDicePool.Release(chaosDice);
        });
    }

    public void StartGenerateChaosDice(int chaosDiceCount)
    {
        SequenceManager.Instance.AddCoroutine(AddChaosDiceCoroutine(chaosDiceCount));
    }

    private IEnumerator AddChaosDiceCoroutine(int chaosDiceCount)
    {
        for (int i = 0; i < chaosDiceCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(diceGenerateDelay);
            GenerateChaosDice();
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

    #region Gamble Dice
    private void GenerateGambleDice(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceList.Count >= gambleDiceNumMax) return;

        var gambleDice = gambleDicePool.Get();
        gambleDice.transform.SetPositionAndRotation(availityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        gambleDice.Init(gambleDiceSO, availityDicePlayboard);

        AddGambleDice(gambleDice);
    }

    private void AddGambleDice(GambleDice gambleDice)
    {
        gambleDiceList.Add(gambleDice);
    }

    private void RemoveGambleDice(GambleDice gambleDice)
    {
        gambleDiceList.Remove(gambleDice);
        gambleDice.FadeOut(() =>
        {
            gambleDicePool.Release(gambleDice);
        });
    }
    #endregion

    #region Enhance
    public int GetEnhancePrice(int enhanceLevel)
    {
        return enhanceLevel * 6 - (enhanceLevel - 1);
    }

    public ScorePair GetEnhanceValue(int enhanceLevel, bool isRandom)
    {
        int totalEnhance = enhanceLevel * 5;
        int baseScoreEnhance = isRandom ? UnityEngine.Random.Range(1, totalEnhance) : totalEnhance / 2;
        int multiplierEnhance = totalEnhance - baseScoreEnhance;

        float baseScore = baseScoreEnhance * 5f;
        float multiplier = multiplierEnhance * 0.05f;

        return new ScorePair(baseScore, multiplier);
    }
    #endregion

    public bool AreAllDiceStopped()
    {
        return AllDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    public void HandleDiceClick(Dice dice)
    {
        if (dice is PlayDice playDice)
        {
            OnPlayDiceClicked?.Invoke(playDice);
        }
        else if (dice is AvailityDice availityDice)
        {
            OnAvailityDiceClicked?.Invoke(availityDice);
        }
        else if (dice is ChaosDice chaosDice)
        {
            OnChaosDiceClicked?.Invoke(chaosDice);
        }
        else if (dice is GambleDice gambleDice)
        {
            OnGambleDiceClicked?.Invoke(gambleDice);
        }
    }
}
