using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultUI : Singleton<GameResultUI>
{
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private Vector3 hidePos;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private string gameClearText = "Game Clear";
    [SerializeField] private string gameOverText = "Game Over";
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button infinityModeButton;
    [SerializeField] private GameObject infinityModeButtonRow;
    [SerializeField] private FadeCanvasGroup fadeCanvasGroup;

    public event Action OnInfinityModeButtonClicked;

    private void Start()
    {
        RegisterEvents();
        mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
        newGameButton.onClick.AddListener(OnClickNewGameButton);
        infinityModeButton.onClick.AddListener(OnClickInfinityModeButton);
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
        Hide(() => OnInfinityModeButtonClicked?.Invoke());
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.GameClear)
        {
            Show();
            resultText.text = gameClearText;
            highScoreText.text = "HighestRoundScore: " + UtilityFunctions.FormatNumber(ScoreManager.Instance.HighestRoundScore);
            infinityModeButtonRow.SetActive(true);
        }
        else if (state == GameState.GameOver)
        {
            Show();
            resultText.text = gameOverText;
            highScoreText.text = "HighestRoundScore: " + UtilityFunctions.FormatNumber(ScoreManager.Instance.HighestRoundScore);
            infinityModeButtonRow.SetActive(false);
        }
    }
    #endregion

    #region ShowHide
    private void Show()
    {
        gameObject.SetActive(true);
        gameOverPanel.anchoredPosition = hidePos;
        gameOverPanel
            .DOAnchorPos(Vector3.zero, DataContainer.Instance.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {

            });

        fadeCanvasGroup.FadeIn(DataContainer.Instance.DefaultDuration);
    }

    private void Hide(Action onComplete = null)
    {
        gameOverPanel.anchoredPosition = Vector3.zero;
        gameOverPanel
            .DOAnchorPos(hidePos, DataContainer.Instance.DefaultDuration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
            });

        fadeCanvasGroup.FadeOut(DataContainer.Instance.DefaultDuration);
    }
    #endregion
}