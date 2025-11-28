using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 핸드 성공 UI 클래스
/// </summary>
public class HandSuccessUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private AnimatedText _successText;
    [SerializeField] private float _displayDuration = 2f;
    [SerializeField] private SFXType _successSFX = SFXType.HandSuccess;
    [SerializeField] private Transform _effectPos;

    private void Awake()
    {
        // 이벤트 구독
        HandSuccessUIEvents.OnHandSuccessed += ShowHandSuccess;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        HandSuccessUIEvents.OnHandSuccessed -= ShowHandSuccess;
    }

    private void Start()
    {
        _successText.ClearText();
    }

    private void ShowHandSuccess(Dictionary<Hand, ScorePair> handScoreDict)
    {
        //목표 핸드. 초이스로 초기화
        Hand targetHand = Hand.Choice;
        double highestScore = 0f;

        //가장 높은 점수를 가진 핸드 찾기
        foreach (var pair in handScoreDict)
        {
            var hand = pair.Key;
            var scorePair = pair.Value;

            //점수 계산은 단순 곱셈
            var score = scorePair.baseScore * scorePair.multiplier;

            if (score > highestScore)
            {
                highestScore = score;
                targetHand = hand;
            }
        }

        //초이스인 경우는 표시하지 않음
        if (targetHand == Hand.Choice) return;

        //타겟 핸드의 핸드SO 가져오기
        var targetHandSO = DataContainer.Instance.GetHandSO(targetHand);
        if (targetHandSO == null) return;

        //텍스트 설정
        string successMessage = targetHandSO.HandName;

        //코루틴 구독
        if (SequenceManager.Instance)
        {
            SequenceManager.Instance.AddCoroutine(ShowSuccessTextCoroutine(successMessage));
        }
    }

    /// <summary>
    /// 성공 텍스트를 표시하는 코루틴
    /// </summary>
    private IEnumerator ShowSuccessTextCoroutine(string message)
    {
        _successText.ShowText(message);
        ParticleEvents.TriggerOnHandSuccess(_effectPos.position);

        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySFX(_successSFX);
        }

        yield return new WaitForSeconds(_displayDuration);
        _successText.ClearText();
    }
}