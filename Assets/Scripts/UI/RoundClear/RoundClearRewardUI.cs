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

        var nameAnimation = StartCoroutine(AnimationManager.Instance.PlayAnimation(rewardNameText, AnimationType.Text));
        var valueAnimation = StartCoroutine(AnimationManager.Instance.PlayAnimation(rewardValueText, AnimationType.Text));

        yield return nameAnimation;
        yield return valueAnimation;

        RoundClearUI.Instance.TriggerReward(rewardValue);
    }

    private void SetRewardText()
    {
        switch (type)
        {
            case RoundClearRewardType.RoundNum:
                rewardNameText.text = "Round Clear";
                rewardValue = PlayerMoneyManager.Instance.RoundClearReward;
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            case RoundClearRewardType.PlayRemain:
                rewardNameText.text = "Play Remain";
                rewardValue = PlayerMoneyManager.Instance.PlayRemainReward;
                rewardValueText.text = GetRewardValueText(rewardValue);
                break;
            case RoundClearRewardType.MoneyInterest:
                rewardNameText.text = "Money Interest";
                rewardValue = PlayerMoneyManager.Instance.MoneyInterestReward;
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
