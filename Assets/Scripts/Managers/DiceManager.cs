using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Pool;

public class DiceManager : Singleton<DiceManager>
{
    [SerializeField] private PlayDice playDicePrefab;
    [SerializeField] private AbilityDice abilityDicePrefab;
    [SerializeField] private ChaosDice chaosDicePrefab;
    [SerializeField] private GambleDice gambleDicePrefab;
    [SerializeField] private Playboard playDicePlayboard;
    [SerializeField] private Playboard abilityDicePlayboard;
    [SerializeField] private float diceGenerateDelay = 0.25f;
    [SerializeField] private int defaultPlayDiceValueMax = 6;
    [SerializeField] private int defaultChaosDiceValueMax = 4;
    [SerializeField] private LocalizedString playDiceName;
    [SerializeField] private LocalizedString chaosDiceName;
    [SerializeField] private LocalizedString getScoreDescription;

    private List<PlayDice> playDiceList = new();
    private List<AbilityDice> abilityDiceList = new();
    private List<ChaosDice> chaosDiceList = new();
    private List<GambleDice> gambleDiceList = new();
    private ObjectPool<PlayDice> playDicePool;
    private ObjectPool<AbilityDice> abilityDicePool;
    private ObjectPool<ChaosDice> chaosDicePool;
    private ObjectPool<GambleDice> gambleDicePool;
    private bool isAbilityDiceAutoKeep = false;
    private int currentAbilityDiceMax = 0;
    private int currentGambleDiceMax = 0;

    public List<PlayDice> PlayDiceList => playDiceList;
    public List<AbilityDice> AbilityDiceList => abilityDiceList;
    public List<ChaosDice> ChaosDiceList => chaosDiceList;
    public List<GambleDice> GambleDiceList => gambleDiceList;
    public List<Dice> AllDiceList
    {
        get
        {
            List<Dice> allDiceList = new();
            allDiceList.AddRange(playDiceList);
            allDiceList.AddRange(abilityDiceList);
            allDiceList.AddRange(chaosDiceList);
            allDiceList.AddRange(gambleDiceList);
            return allDiceList;
        }
    }
    public int CurrentAbilityDiceMax
    {
        get => currentAbilityDiceMax;
        set
        {
            if (currentAbilityDiceMax == value) return;
            currentAbilityDiceMax = value;
            OnCurrentAbilityDiceMaxChanged?.Invoke(currentAbilityDiceMax);
        }
    }
    public int CurrentGambleDiceMax
    {
        get => currentGambleDiceMax;
        set
        {
            if (currentGambleDiceMax == value) return;
            currentGambleDiceMax = value;
            OnCurrentGambleDiceMaxChanged?.Invoke(currentGambleDiceMax);
        }
    }
    public bool IsAbilityDiceAutoKeep => isAbilityDiceAutoKeep;
    public List<int> UsableDiceValues { get; set; }
    public bool IsKeepable { get; set; } = true;
    public LocalizedString PlayDiceName => playDiceName;
    public LocalizedString ChaosDiceName => chaosDiceName;
    public LocalizedString GetScoreDescription => getScoreDescription;

    public event Action<PlayDice> OnPlayDiceClicked;
    public event Action<AbilityDice> OnAbilityDiceClicked;
    public event Action<ChaosDice> OnChaosDiceClicked;
    public event Action<GambleDice> OnGambleDiceClicked;
    public event Action<int> OnAbilityDiceCountChanged;
    public event Action<int> OnCurrentAbilityDiceMaxChanged;
    public event Action<int> OnGambleDiceCountChanged;
    public event Action<int> OnCurrentGambleDiceMaxChanged;

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
        OnAbilityDiceAutoKeepChanged(OptionManager.Instance.OptionData.abilityDiceAutoKeep);

        playDicePool = new(() => Instantiate(playDicePrefab), playDice => { playDice.gameObject.SetActive(true); }, playDice => playDice.gameObject.SetActive(false), playDice => Destroy(playDice.gameObject), false);
        abilityDicePool = new(() => Instantiate(abilityDicePrefab), abilityDice => { abilityDice.gameObject.SetActive(true); }, abilityDice => abilityDice.gameObject.SetActive(false), abilityDice => Destroy(abilityDice.gameObject), false);
        chaosDicePool = new(() => Instantiate(chaosDicePrefab), chaosDice => { chaosDice.gameObject.SetActive(true); }, chaosDice => chaosDice.gameObject.SetActive(false), chaosDice => Destroy(chaosDice.gameObject), false);
        gambleDicePool = new(() => Instantiate(gambleDicePrefab), gambleDice => { gambleDice.gameObject.SetActive(true); }, gambleDice => gambleDice.gameObject.SetActive(false), gambleDice => Destroy(gambleDice.gameObject), false);

        CurrentAbilityDiceMax = DataContainer.Instance.CurrentPlayerStat.defaultAbilityDiceMax;
        CurrentGambleDiceMax = DataContainer.Instance.CurrentPlayerStat.defaultGambleDiceMax;
    }

    #region Events
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged += OnOptionValueChanged;
        ShopManager.Instance.OnAbilityDicePurchaseAttempted += OnPurchaseAttempted;
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged -= OnOptionValueChanged;
        ShopManager.Instance.OnAbilityDicePurchaseAttempted -= OnPurchaseAttempted;
    }

    private void OnOptionValueChanged(OptionType type, int value)
    {
        if (type == OptionType.AbilityDiceAutoKeep)
        {
            OnAbilityDiceAutoKeepChanged(value);
        }
    }

    public void StartAddBonusPlayDice()
    {
        StartCoroutine(AddBonusPlayDice());
    }

    public void IncreaseCurrentAbilityDiceMax()
    {
        CurrentAbilityDiceMax++;
    }

    private void OnAbilityDiceAutoKeepChanged(int value)
    {
        isAbilityDiceAutoKeep = value == 1;
    }

    private void OnPurchaseAttempted(AbilityDiceSO sO, PurchaseResult result)
    {
        if (sO == null) return;

        if (result == PurchaseResult.Success)
        {
            GenerateAbilityDice(sO);
        }
    }
    #endregion

    #region Play Dice
    private void GeneratePlayDice()
    {
        var playDice = playDicePool.Get();
        playDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        playDice.Init(defaultPlayDiceValueMax, DataContainer.Instance.DefaultDiceSpriteList, DataContainer.Instance.DefaultShaderData, playDicePlayboard);

        AudioManager.Instance.PlaySFX(SFXType.DiceGenerate);

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

        AudioManager.Instance.PlaySFX(SFXType.DiceRemove);
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

    public void StartFirstPlayDiceGenerate(Action onComplete = null)
    {
        SequenceManager.Instance.AddCoroutine(FirstPlayDiceGenerate(onComplete));
    }

    private IEnumerator FirstPlayDiceGenerate(Action onComplete = null)
    {
        for (int i = 0; i < DataContainer.Instance.CurrentPlayerStat.defaultPlayDiceCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(diceGenerateDelay);

            GeneratePlayDice();
        }

        yield return new WaitUntil(() => AreAllDiceStopped());

        onComplete?.Invoke();
    }

    private IEnumerator AddBonusPlayDice(Action onComplete = null)
    {
        GeneratePlayDice();

        yield return new WaitUntil(() => AreAllDiceStopped());

        onComplete?.Invoke();
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

    #region Ability Dice
    private void GenerateAbilityDice(AbilityDiceSO sO)
    {
        if (abilityDiceList.Count >= currentAbilityDiceMax) return;

        var abilityDice = abilityDicePool.Get();
        abilityDice.transform.SetPositionAndRotation(abilityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        abilityDice.Init(sO, abilityDicePlayboard);

        AudioManager.Instance.PlaySFX(SFXType.DiceGenerate);

        AddAbilityDice(abilityDice);
    }

    private void AddAbilityDice(AbilityDice abilityDice)
    {
        abilityDiceList.Add(abilityDice);

        OnAbilityDiceCountChanged?.Invoke(abilityDiceList.Count);
    }

    public void RemoveAbilityDice(AbilityDice abilityDice)
    {
        abilityDiceList.Remove(abilityDice);
        OnAbilityDiceCountChanged?.Invoke(abilityDiceList.Count);

        abilityDice.FadeOut(() =>
        {
            abilityDicePool.Release(abilityDice);
        });

        AudioManager.Instance.PlaySFX(SFXType.DiceRemove);
    }

    public void EnableAbilityDice(AbilityDice abilityDice)
    {
        abilityDice.IsEnabled = true;

        abilityDiceList.Add(abilityDice);
    }

    public void DisableAbilityDice(AbilityDice abilityDice)
    {
        abilityDice.IsEnabled = false;

        abilityDiceList.Remove(abilityDice);
    }

    public void StartGenerateRandomNormalAbilityDice(int abilityDiceCount)
    {
        SequenceManager.Instance.AddCoroutine(GenerateAbilityRandomNormalDiceCoroutine(abilityDiceCount));
    }

    private IEnumerator GenerateAbilityRandomNormalDiceCoroutine(int abilityDiceCount)
    {
        for (int i = 0; i < abilityDiceCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(diceGenerateDelay);
            var randomAbilityDice = DataContainer.Instance.NormalAbilityDiceListSO.abilityDiceSOList.GetRandomElement();
            GenerateAbilityDice(randomAbilityDice);
        }

        yield return new WaitUntil(() => AreAllDiceStopped());
    }
    #endregion

    #region Chaos Dice
    private void GenerateChaosDice()
    {
        var chaosDice = chaosDicePool.Get();
        chaosDice.transform.SetPositionAndRotation(playDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        chaosDice.Init(defaultChaosDiceValueMax, DataContainer.Instance.DefaultDiceSpriteList, DataContainer.Instance.ChaosShaderData, playDicePlayboard);

        AudioManager.Instance.PlaySFX(SFXType.DiceGenerate);

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

        AudioManager.Instance.PlaySFX(SFXType.DiceRemove);
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
    public void GenerateGambleDice(GambleDiceSO gambleDiceSO)
    {
        if (gambleDiceList.Count >= currentGambleDiceMax) return;

        var gambleDice = gambleDicePool.Get();
        gambleDice.transform.SetPositionAndRotation(abilityDicePlayboard.DiceGeneratePosition, Quaternion.identity);
        gambleDice.Init(gambleDiceSO, abilityDicePlayboard);

        AudioManager.Instance.PlaySFX(SFXType.DiceGenerate);

        AddGambleDice(gambleDice);
    }

    private void AddGambleDice(GambleDice gambleDice)
    {
        gambleDiceList.Add(gambleDice);
        OnGambleDiceCountChanged?.Invoke(gambleDiceList.Count);
    }

    private void RemoveGambleDice(GambleDice gambleDice)
    {
        gambleDiceList.Remove(gambleDice);
        OnGambleDiceCountChanged?.Invoke(gambleDiceList.Count);

        gambleDice.FadeOut(() =>
        {
            gambleDicePool.Release(gambleDice);
        });

        AudioManager.Instance.PlaySFX(SFXType.DiceRemove);
    }

    public void ClearGambleDices()
    {
        while (gambleDiceList.Count > 0)
        {
            RemoveGambleDice(gambleDiceList[0]);
        }
    }

    public void StartGenerateGambleDice(int gambleDiceCount)
    {
        SequenceManager.Instance.AddCoroutine(GenerateGambleDiceCoroutine(gambleDiceCount));
    }

    private IEnumerator GenerateGambleDiceCoroutine(int gambleDiceCount)
    {
        for (int i = 0; i < gambleDiceCount; i++)
        {
            if (i != 0) yield return new WaitForSeconds(diceGenerateDelay);
            GenerateGambleDice(DataContainer.Instance.NormalGambleDiceListSO.gambleDiceSOList.GetRandomElement());
        }

        yield return new WaitUntil(() => AreAllDiceStopped());
    }
    #endregion

    public bool AreAllDiceStopped()
    {
        return AllDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    public void HandleDiceClick(Dice dice)
    {
        AudioManager.Instance.PlaySFX(SFXType.DiceClick);

        if (dice is PlayDice playDice)
        {
            OnPlayDiceClicked?.Invoke(playDice);
        }
        else if (dice is AbilityDice abilityDice)
        {
            OnAbilityDiceClicked?.Invoke(abilityDice);
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
