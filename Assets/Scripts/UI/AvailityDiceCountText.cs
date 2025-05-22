using TMPro;
using UnityEngine;

public class AvailityDiceCountText : MonoBehaviour
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
        DiceManager.Instance.OnAvailityDiceCountChanged += OnAvailityDiceCountChanged;
        DiceManager.Instance.OnCurrentAvailityDiceMaxChanged += OnCurrentAvailityDiceMaxChanged;
    }

    private void OnAvailityDiceCountChanged(int count)
    {
        currentCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void OnCurrentAvailityDiceMaxChanged(int count)
    {
        maxCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void Init()
    {
        currentCount = DiceManager.Instance.AvailityDiceList.Count;
        maxCount = DiceManager.Instance.CurrentAvailityDiceMax;
        SetText();
    }

    private void SetText()
    {
        text.text = $"{currentCount}/{maxCount}";
    }
}
