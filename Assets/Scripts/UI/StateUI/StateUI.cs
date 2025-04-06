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
        PlayManager.Instance.PlayRemainChanged += OnPlayRemainChanged;
        RollManager.Instance.RollRemainChanged += OnRollRemainChanged;
    }

    private void OnPlayRemainChanged(int playRemain)
    {
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(playRemainText, playRemain.ToString()));
    }

    private void OnRollRemainChanged(int rollRemain)
    {
        SequenceManager.Instance.AddCoroutine(UpdateTextAndPlayAnimation(rollRemainText, rollRemain.ToString()));
    }

    private IEnumerator UpdateTextAndPlayAnimation(TMP_Text targetText, string targetString)
    {
        targetText.text = targetString;

        yield return StartCoroutine(AnimationManager.Instance.PlayAnimation(targetText, AnimationType.Shake));
    }
}
