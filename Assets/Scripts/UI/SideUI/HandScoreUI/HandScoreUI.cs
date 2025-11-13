using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HandScoreUI : MonoBehaviour
{
    [SerializeField] private Transform handScoreSingleUIPrefab;
    [SerializeField] private RectTransform layoutPanel;
    [SerializeField] private float scrollDuration;
    [SerializeField] private Vector2 targetScrollAnchoredPosition;

    private Dictionary<Hand, HandScoreSingleUI> handScoreSingleUIDict = new();

    private void Start()
    {
        RegisterEvents();
        Init();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region Events
    private void RegisterEvents()
    {
        if (BonusManager.Instance) BonusManager.Instance.OnAllBonusAchieved += OnAllBonusAchieved;

        HandScoreUIEvents.OnHandScoreUIUpdateRequested += UpdateHandScores;
        HandScoreUIEvents.OnHandScoreUIResetRequested += ResetHandScoreUI;
        HandScoreUIEvents.OnHandScoreUIPlayHandTriggerAnimationRequested += PlayHandTriggerAnimation;
    }

    private void UnregisterEvents()
    {
        if (BonusManager.Instance) BonusManager.Instance.OnAllBonusAchieved -= OnAllBonusAchieved;

        HandScoreUIEvents.OnHandScoreUIUpdateRequested -= UpdateHandScores;
        HandScoreUIEvents.OnHandScoreUIResetRequested -= ResetHandScoreUI;
        HandScoreUIEvents.OnHandScoreUIPlayHandTriggerAnimationRequested -= PlayHandTriggerAnimation;
    }

    private void OnAllBonusAchieved()
    {
        SequenceManager.Instance.AddCoroutine(() => ScrollLayoutPanel());
    }

    #endregion

    private void Init()
    {
        foreach (var handSO in DataContainer.Instance.TotalHandListSO.handList)
        {
            var handScoreSingleUITransform = Instantiate(handScoreSingleUIPrefab, layoutPanel);
            var handScoreSingleUI = handScoreSingleUITransform.GetComponent<HandScoreSingleUI>();
            handScoreSingleUI.Init(handSO, HandleSelectHand);
            handScoreSingleUIDict.Add(handSO.hand, handScoreSingleUI);
        }

        RebuildLayout();
    }

    private void HandleSelectHand(HandSO handSO)
    {
        HandManager.Instance.HandleSelectHand(handSO);
    }

    public void UpdateHandScores(Dictionary<Hand, ScorePair> handScoreDict)
    {
        foreach (var pair in handScoreDict)
        {
            if (handScoreSingleUIDict.TryGetValue(pair.Key, out var handScoreSingleUI))
            {
                handScoreSingleUI.UpdateScoreText(pair.Value);
            }
        }
    }

    public void PlayHandTriggerAnimation(Hand hand, int enhanceLevel, ScorePair scorePair)
    {
        if (handScoreSingleUIDict.TryGetValue(hand, out var handScoreSingleUI))
        {
            handScoreSingleUI.PlayTriggerAnimation(enhanceLevel, scorePair);
        }
    }

    private void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutPanel);
    }

    public void ResetHandScoreUI()
    {
        foreach (var pair in handScoreSingleUIDict)
        {
            pair.Value.ResetScoreText();
        }
    }

    private void ScrollLayoutPanel(bool isImmediate = false)
    {
        if (isImmediate)
        {
            layoutPanel.anchoredPosition = targetScrollAnchoredPosition;
        }
        else
        {
            layoutPanel.DOAnchorPos(targetScrollAnchoredPosition, scrollDuration).SetEase(Ease.InOutQuint);
        }
    }
}
