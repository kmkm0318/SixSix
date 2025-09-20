using UnityEngine;

public class VideoOptionManager : Singleton<VideoOptionManager>
{
    private void Start()
    {
        Init();
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Init()
    {
        OnFullscreenChanged(OptionManager.Instance.OptionData.fullscreen);
        OnResolutionChanged(OptionManager.Instance.OptionData.resolution);
    }

    #region Events
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged += OnOptionValueChanged;
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged -= OnOptionValueChanged;
    }

    private void OnOptionValueChanged(OptionType type, int value)
    {
        switch (type)
        {
            case OptionType.Fullscreen:
                OnFullscreenChanged(value);
                break;
            case OptionType.Resolution:
                OnResolutionChanged(value);
                break;
            default:
                break;
        }
    }

    private void OnFullscreenChanged(int value)
    {
        Screen.fullScreenMode = value == 1 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    private void OnResolutionChanged(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreenMode);
                break;
            case 1:
                Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
                break;
            case 2:
                Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
                break;
            case 3:
                Screen.SetResolution(3840, 2160, Screen.fullScreenMode);
                break;
        }
    }
    #endregion
}