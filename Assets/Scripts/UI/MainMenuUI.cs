using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private ButtonPanel _startButton;
    [SerializeField] private ButtonPanel _questButton;
    [SerializeField] private ButtonPanel _optionButton;
    [SerializeField] private ButtonPanel _exitButton;

    bool _isActive = false;

    private void Start()
    {
        _isActive = true;

        _startButton.OnClick += StartGame;
        _questButton.OnClick += ShowQuestUI;
        _optionButton.OnClick += ShowOptionUI;
        _exitButton.OnClick += ExitGame;
    }

    private void StartGame()
    {
        if (!_isActive) return;
        _isActive = false;

        SceneTransitionManager.Instance.ChangeScene(SceneType.Game);
    }

    private void ShowQuestUI()
    {
        if (!_isActive) return;

        QuestUIEvents.TriggerOnQuestButtonClicked();
    }

    private void ShowOptionUI()
    {
        if (!_isActive) return;

        OptionUIEvents.TriggerOnOptionButtonClicked();
    }

    private void ExitGame()
    {
        if (!_isActive) return;

        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}