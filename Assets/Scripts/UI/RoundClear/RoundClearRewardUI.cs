using System.Collections;
using TMPro;
using UnityEngine;

public class RoundClearRewardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rewardNameText;
    [SerializeField] private TMP_Text rewardValueText;

    private RoundClearRewardType type;
    private int rewardValue = 0;

    public void Show(RoundClearRewardType type)
    {
        this.type = type;

        Init();

        SequenceManager.Instance.AddCoroutine(ShowRewardTextAnimation());
    }

    private void Init()
    {
        rewardNameText.text = string.Empty;
        rewardValueText.text = string.Empty;
    }

    private IEnumerator ShowRewardTextAnimation()
    {
        SetRewardText();

        if (rewardValue == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        string rewardName = rewardNameText.text;
        string rewardValueTextContent = rewardValueText.text;

        rewardNameText.text = string.Empty;
        rewardValueText.text = string.Empty;

        yield return StartCoroutine(AnimationFunction.PlayTextAnimation(rewardNameText, rewardName));
        yield return StartCoroutine(AnimationFunction.PlayTextAnimation(rewardValueText, rewardValueTextContent));

        RoundClearUI.Instance.TriggerReward(rewardValue);
    }

    private void SetRewardText()
    {
        rewardValue = RoundClearManager.Instance.GetRewardValue(type);

        switch (type)
        {
            case RoundClearRewardType.RoundNum:
                rewardNameText.text = "Round Clear";
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            case RoundClearRewardType.PlayRemain:
                rewardNameText.text = "Play Remain";
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            case RoundClearRewardType.MoneyInterest:
                rewardNameText.text = "Money Interest";
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            case RoundClearRewardType.BossRound:
                rewardNameText.text = "Boss Round Clear";
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            default:
                break;
        }
    }

    private string GetRewardValueText(int value)
    {
        string text = string.Empty;
        for (int i = 0; i < value; i++)
        {
            text += "$";
        }
        return text;
    }
}
