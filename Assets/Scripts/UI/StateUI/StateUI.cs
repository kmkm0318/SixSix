using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviour
{
    [SerializeField] private Button optionButton;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text playRemainText;
    [SerializeField] private TMP_Text rollRemainText;

    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        MoneyManager.Instance.OnMoneyChanged += OnMoneyChanged;
        PlayManager.Instance.OnPlayRemainChanged += OnPlayRemainChanged;
        RollManager.Instance.OnRollRemainChanged += OnRollRemainChanged;
    }

    private void OnMoneyChanged(int money)
    {
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(moneyText, "$" + money.ToString()));
    }

    private void OnPlayRemainChanged(int playRemain)
    {
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(playRemainText, playRemain.ToString()), true);
    }

    private void OnRollRemainChanged(int rollRemain)
    {
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(rollRemainText, rollRemain.ToString()), true);
        SequenceManager.Instance.ApplyParallelCoroutine();
    }

    private IEnumerator UpdateTextAndPlayAnimation(TMP_Text targetText, string targetString)
    {
        targetText.text = targetString;

        yield return StartCoroutine(AnimationManager.Instance.PlayAnimation(targetText, AnimationType.Shake));
    }
}
