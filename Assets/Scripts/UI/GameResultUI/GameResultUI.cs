using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultUI : Singleton<GameResultUI>
{
    [SerializeField] private RectTransform gameResultPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private AnimatedText resultText;
    [SerializeField] private List<GameResultUIPair> gameResultUIPairs;
    [SerializeField] private ButtonPanel mainMenuButton;
    [SerializeField] private ButtonPanel newGameButton;
    [SerializeField] private ButtonPanel infinityModeButton;
    [SerializeField] private GameObject infinityModeButtonRow;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;

    private bool _isGameClear;

    private void Start()
    {
        mainMenuButton.OnClick += OnClickMainMenuButton;
        newGameButton.OnClick += OnClickNewGameButton;
        infinityModeButton.OnClick += OnClickInfinityModeButton;
        gameObject.SetActive(false);
    }

    private void OnClickMainMenuButton()
    {
        Debug.Log("Go To Main Menu");
    }

    private void OnClickNewGameButton()
    {
        SceneManager.LoadScene(0);
    }

    private void OnClickInfinityModeButton()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.RoundClear);
    }

    public void ShowGameResult(bool isGameClear)
    {
        _isGameClear = isGameClear;

        SequenceManager.Instance.AddCoroutine(Show);
        resultText.SetText(_isGameClear ? "Game Clear" : "Game Over");

        Color color = _isGameClear ?
        DataContainer.Instance.DefaultColorSO.blue : DataContainer.Instance.DefaultColorSO.red;

        resultText.TMP_Text.color = color;
        infinityModeButtonRow.SetActive(_isGameClear);

        foreach (var pair in gameResultUIPairs)
        {
            string res = GameResultManager.Instance.GetResultValue(pair.type);
            pair.panel.SetValue(res);
        }
    }

    #region ShowHide
    private void Show()
    {
        gameObject.SetActive(true);
        gameResultPanel.anchoredPosition = hidePos;
        gameResultPanel
            .DOAnchorPos(Vector3.zero, AnimationFunction.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(AnimationFunction.DefaultDuration);

        AudioManager.Instance.PlaySFX(_isGameClear ? SFXType.Win : SFXType.Lose);
    }

    private void Hide(Action onComplete = null)
    {
        gameResultPanel.anchoredPosition = Vector3.zero;
        gameResultPanel
            .DOAnchorPos(hidePos, AnimationFunction.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
            });

        fadeCanvasGroup.FadeOut(AnimationFunction.DefaultDuration);
    }
    #endregion
}

[Serializable]
public class GameResultUIPair
{
    public GameResultValueType type;
    public LabeledValuePanel panel;
}