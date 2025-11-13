using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    [SerializeField] private OptionSelectUI[] _optionSelectUIs;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _closeButton;

    private void Start()
    {
        RegisterEvents();
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
        _closeButton.onClick.AddListener(() => Hide());
        gameObject.SetActive(false);
    }

    private void GoToMainMenu()
    {
        Hide();

        SceneTransitionManager.Instance.ChangeScene(SceneType.MainMenu);
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionButtonClicked += OnOptionButtonClicked;

        foreach (var optionSelectUI in _optionSelectUIs)
        {
            optionSelectUI.OnOptionValueChanged += (value) => OptionUIEvents.TriggerOnOptionValueChanged(optionSelectUI.OptionTypeDataSO.optionType, value);
        }
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionButtonClicked -= OnOptionButtonClicked;
    }

    private void OnOptionButtonClicked()
    {
        Show();
    }
    #endregion
}