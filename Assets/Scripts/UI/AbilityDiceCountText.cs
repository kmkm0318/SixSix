using TMPro;
using UnityEngine;

public class AbilityDiceCountText : MonoBehaviour
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
        DiceManager.Instance.OnAbilityDiceCountChanged += OnAbilityDiceCountChanged;
        DiceManager.Instance.OnCurrentAbilityDiceMaxChanged += OnCurrentAbilityDiceMaxChanged;
    }

    private void OnAbilityDiceCountChanged(int count)
    {
        currentCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void OnCurrentAbilityDiceMaxChanged(int count)
    {
        maxCount = count;
        SetText();
        StartCoroutine(AnimationFunction.ShakeAnimation(transform, true));
    }

    private void Init()
    {
        currentCount = DiceManager.Instance.AbilityDiceList.Count;
        maxCount = DiceManager.Instance.CurrentAbilityDiceMax;
        SetText();
    }

    private void SetText()
    {
        text.text = $"{currentCount}/{maxCount}";
    }
}
