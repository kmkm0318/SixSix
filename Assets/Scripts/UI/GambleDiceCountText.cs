using TMPro;
using UnityEngine;

public class GambleDiceCountText : MonoBehaviour
{
    private TMP_Text text;

    private int currentCount;
    private int maxCount;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
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
        currentCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void OnCurrentGambleDiceMaxChanged(int count)
    {
        maxCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void Init()
    {
        currentCount = DiceManager.Instance.GambleDiceList.Count;
        maxCount = DiceManager.Instance.CurrentGambleDiceMax;
        SetText();
    }

    private void SetText()
    {
        text.text = $"{currentCount}/{maxCount}";
    }
}
