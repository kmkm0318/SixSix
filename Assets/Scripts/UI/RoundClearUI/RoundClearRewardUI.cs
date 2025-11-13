using System.Collections;
using UnityEngine;

public class RoundClearRewardUI : MonoBehaviour
{
    [SerializeField] private AnimatedText rewardNameText;
    [SerializeField] private AnimatedText rewardValueText;
    [SerializeField] private float showDelay = 0.25f;

    private RoundClearRewardType _type;

    private void Start()
    {
        RoundClearUIEvents.OnRoundClearUIHidden += OnRoundClearUIClosed;
    }

    private void OnDestroy()
    {
        RoundClearUIEvents.OnRoundClearUIHidden -= OnRoundClearUIClosed;
    }

    private void OnRoundClearUIClosed()
    {
        rewardNameText.ClearText();
        rewardValueText.ClearText();
    }

    public void ShowReward(RoundClearRewardType type)
    {
        this._type = type;

        SequenceManager.Instance.AddCoroutine(ShowRewardTextAnimation());
    }

    private IEnumerator ShowRewardTextAnimation()
    {
        int rewardValue = RoundClearManager.Instance.GetRewardValue(_type);

        if (rewardValue == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        string rewardName = GetRewardName();
        string rewardValueTextContent = GetRewardValueText(rewardValue);

        yield return new WaitForSeconds(showDelay);
        yield return StartCoroutine(rewardNameText.ShowTextCoroutine(rewardName));
        yield return new WaitForSeconds(showDelay);
        yield return StartCoroutine(rewardValueText.ShowTextCoroutine(rewardValueTextContent));

        MoneyManager.Instance.AddMoney(rewardValue, true);
    }

    private string GetRewardName()
    {
        return _type switch
        {
            RoundClearRewardType.RoundClear => "Round Clear",
            RoundClearRewardType.PlayRemain => "Play Remain",
            RoundClearRewardType.MoneyInterest => "Money Interest",
            RoundClearRewardType.BossRoundClear => "Boss Round Clear",
            _ => string.Empty,
        };
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
