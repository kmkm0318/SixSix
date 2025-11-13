using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GambleDiceCountText : MonoBehaviour
{
    private TMP_Text _text;

    private int _currentCount;
    private int _maxCount;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        RegisterEvents();
        Init();
    }

    private void RegisterEvents()
    {
        DiceManager.Instance.OnGambleDiceCountChanged += OnGambleDiceCountChanged;
        DiceManager.Instance.OnCurrentGambleDiceMaxChanged += OnCurrentGambleDiceMaxChanged;
    }

    private void OnGambleDiceCountChanged(int count)
    {
        _currentCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void OnCurrentGambleDiceMaxChanged(int count)
    {
        _maxCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void Init()
    {
        _currentCount = DiceManager.Instance.GambleDiceList.Count;
        _maxCount = DiceManager.Instance.CurrentGambleDiceMax;
        SetText();
    }

    private void SetText()
    {
        _text.text = $"{_currentCount}/{_maxCount}";
    }
}
