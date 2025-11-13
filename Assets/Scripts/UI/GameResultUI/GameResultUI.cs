using System;
using System.Collections.Generic;
using UnityEngine;

public class GameResultUI : BaseUI
{
    [SerializeField] private AnimatedText _resultText;
    [SerializeField] private List<GameResultUIPair> _gameResultUIPairs;
    [SerializeField] private ButtonPanel _mainMenuButton;
    [SerializeField] private ButtonPanel _newGameButton;
    [SerializeField] private ButtonPanel _infinityModeButton;
    [SerializeField] private GameObject _infinityModeButtonRow;

    private bool _isGameClear;

    private void Start()
    {
        _mainMenuButton.OnClick += OnClickMainMenuButton;
        _newGameButton.OnClick += OnClickNewGameButton;
        _infinityModeButton.OnClick += OnClickInfinityModeButton;
        GameResultUIEvents.OnGameResultUIShowRequested += OnGameResultUIShowRequested;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameResultUIEvents.OnGameResultUIShowRequested -= OnGameResultUIShowRequested;
    }

    private void OnGameResultUIShowRequested(bool isClear)
    {
        ShowGameResult(isClear);
    }

    private void OnClickMainMenuButton()
    {
        Hide();

        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }

    private void OnClickNewGameButton()
    {
        Hide();

        SceneTransitionManager.Instance.ChangeScene(SceneType.Game);
    }

    private void OnClickInfinityModeButton()
    {
        Hide();
        GameManager.Instance.ChangeState(GameState.RoundClear);
    }

    private void ShowGameResult(bool isGameClear)
    {
        _isGameClear = isGameClear;

        _resultText.SetText(_isGameClear ? "Game Clear" : "Game Over");

        Color color = _isGameClear ?
        DataContainer.Instance.DefaultColorSO.blue : DataContainer.Instance.DefaultColorSO.red;

        _resultText.TMP_Text.color = color;
        _infinityModeButtonRow.SetActive(_isGameClear);

        foreach (var pair in _gameResultUIPairs)
        {
            string res = GameResultManager.Instance.GetResultText(pair.type);
            pair.panel.SetValue(res);
        }

        SequenceManager.Instance.AddCoroutine(() => Show());
    }
}

[Serializable]
public class GameResultUIPair
{
    public GameResultValueType type;
    public LabeledValuePanel panel;
}