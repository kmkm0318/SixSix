using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private ButtonPanel _startButton;
    [SerializeField] private ButtonPanel _questButton;
    [SerializeField] private ButtonPanel _collectionButton;
    [SerializeField] private ButtonPanel _optionButton;
    [SerializeField] private ButtonPanel _exitButton;

    private void Start()
    {
        _startButton.OnClick += StartGame;
        _questButton.OnClick += ShowQuestUI;
        _collectionButton.OnClick += ShowCollectionUI;
        _optionButton.OnClick += ShowOptionUI;
        _exitButton.OnClick += ExitGame;
    }

    private void StartGame()
    {
        StartUIEvents.TriggerOnStartUIButtonClicked();
    }

    private void ShowQuestUI()
    {
        QuestUIEvents.TriggerOnQuestButtonClicked();
    }

    private void ShowCollectionUI()
    {
        CollectionUIEvents.TriggerOnCollectionButtonClicked();
    }

    private void ShowOptionUI()
    {
        OptionUIEvents.TriggerOnOptionButtonClicked();
    }

    private void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}