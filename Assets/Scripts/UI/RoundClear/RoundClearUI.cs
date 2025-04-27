using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class RoundClearUI : Singleton<RoundClearUI>
{
    [SerializeField] private RectTransform roundClearPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Button closeButton;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text roundScoreText;
    [SerializeField] private Transform roundClearRewardParent;
    [SerializeField] private RoundClearRewardUI roundClearRewardUI;

    public event Action<int> OnRewardTriggered;
    public event Action OnRoundClearUIOpened;
    public event Action OnRoundClearUIClosed;

    private ObjectPool<RoundClearRewardUI> rewardPool;
    private bool isClosable = false;

    protected override void Awake()
    {
        base.Awake();

        rewardPool = new ObjectPool<RoundClearRewardUI>(
            () => Instantiate(roundClearRewardUI, roundClearRewardParent),
            rewardUI =>
            {
                rewardUI.gameObject.SetActive(true);
                rewardUI.transform.SetAsLastSibling();
            },
            rewardUI => rewardUI.gameObject.SetActive(false),
            rewardUI => Destroy(rewardUI.gameObject),
            maxSize: 10
        );
    }

    private void Start()
    {
        RegisterEvents();
        closeButton.onClick.AddListener(() =>
        {
            if (!isClosable) return;
            SequenceManager.Instance.AddCoroutine(Hide());
        });
        gameObject.SetActive(false);
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        RoundClearManager.Instance.OnRoundClearStarted += OnRoundClearStarted;
    }

    private void OnRoundClearStarted()
    {
        ClearTexts();
        ClearRewardTexts();
        SequenceManager.Instance.AddCoroutine(Show());
        SequenceManager.Instance.AddCoroutine(ShowTextAnimation());
        CreateRewardUIs();
        SequenceManager.Instance.AddCoroutine(() => isClosable = true);
    }
    #endregion

    #region ShowHide
    private IEnumerator Show()
    {
        gameObject.SetActive(true);
        roundClearPanel.anchoredPosition = hidePos;

        var myTween = roundClearPanel
            .DOAnchorPos(Vector3.zero, moveDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(moveDuration);

        yield return myTween.WaitForCompletion();
        OnRoundClearUIOpened?.Invoke();
    }

    private IEnumerator Hide()
    {
        if (!isClosable) yield break;
        isClosable = false;

        roundClearPanel.anchoredPosition = Vector3.zero;

        var myTween = roundClearPanel
             .DOAnchorPos(hidePos, moveDuration)
             .SetEase(Ease.InOutBack)
             .OnComplete(() =>
             {
                 gameObject.SetActive(false);
             });

        fadeCanvasGroup.FadeOut(moveDuration);

        yield return myTween.WaitForCompletion();
        OnRoundClearUIClosed?.Invoke();
    }
    #endregion

    #region TextAnimation
    private void ClearTexts()
    {
        roundText.text = string.Empty;
        roundScoreText.text = string.Empty;
    }

    private void ClearRewardTexts()
    {
        foreach (Transform child in roundClearRewardParent)
        {
            if (child.TryGetComponent<RoundClearRewardUI>(out var rewardUI))
            {
                rewardPool.Release(rewardUI);
            }
        }
    }

    private IEnumerator ShowTextAnimation()
    {
        SetTexts();

        string roundTextValue = roundText.text;
        string roundScoreTextValue = roundScoreText.text;

        roundText.text = string.Empty;
        roundScoreText.text = string.Empty;

        yield return StartCoroutine(AnimationManager.Instance.PlayTextAnimation(roundText, roundTextValue));
        yield return StartCoroutine(AnimationManager.Instance.PlayTextAnimation(roundScoreText, roundScoreTextValue));
    }

    private void SetTexts()
    {
        roundText.text = "Cleared Round : " + RoundManager.Instance.CurrentRound.ToString();
        roundScoreText.text = "Target Score : " + Functions.FormatNumber(ScoreManager.Instance.TargetRoundScore);
    }
    #endregion

    private void CreateRewardUIs()
    {
        foreach (var type in Enum.GetValues(typeof(RoundClearRewardType)))
        {
            if (type is RoundClearRewardType rewardType && rewardType != RoundClearRewardType.None)
            {
                var rewardUI = rewardPool.Get();
                rewardUI.Show(rewardType);
            }
        }
    }

    public void TriggerReward(int value)
    {
        OnRewardTriggered?.Invoke(value);
    }
}