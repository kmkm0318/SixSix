using DG.Tweening;
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
        PlayerDiceManager.Instance.OnAvailityDiceCountChanged += OnAvailityDiceCountChanged;
        PlayerDiceManager.Instance.OnAvailityDiceCountMaxChanged += OnAvailityDiceCountMaxChanged;
    }

    private void OnAvailityDiceCountChanged(int count)
    {
        currentCount = count;
        SetText();
        StartCoroutine(AnimationFunction.PlayShakeAnimation(transform, true));
    }

    private void OnAvailityDiceCountMaxChanged(int count)
    {
        maxCount = count;
        SetText();
        StartCoroutine(AnimationFunction.PlayShakeAnimation(transform, true));
    }

    private void Init()
    {
        currentCount = PlayerDiceManager.Instance.AvailityDiceList.Count;
        maxCount = PlayerDiceManager.Instance.AvailityDiceCountMax;
        SetText();
    }

    private void SetText()
    {
        text.text = $"{currentCount}/{maxCount}";
    }
}
