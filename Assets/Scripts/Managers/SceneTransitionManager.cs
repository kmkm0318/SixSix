using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float audioFadeDuration = 0.8f;

    public bool IsTransitioning { get; private set; }

    public void ChangeScene(SceneType type)
    {
        if (IsTransitioning)
        {
            Debug.LogWarning($"Scene Transition Is In Progress");
            return;
        }

        string sceneName = GetSceneName(type);
        if (string.IsNullOrEmpty(sceneName)) return;

        StartCoroutine(TransitionSceneCoroutine(sceneName));
    }

    private string GetSceneName(SceneType type)
    {
        return type switch
        {
            SceneType.Loading => "LoadingScene",
            SceneType.MainMenu => "MainMenuScene",
            SceneType.Game => "GameScene",
            _ => throw new ArgumentException($"Scene Type Is Not Valid : {type}"),
        };
    }

    private IEnumerator TransitionSceneCoroutine(string sceneName)
    {
        IsTransitioning = true;

        try
        {
            AudioManager.Instance.StartFadeVolume(0f, audioFadeDuration);

            bool uiTransitionComplete = false;
            SceneTransitionUI.Instance.Show(transitionDuration, () => uiTransitionComplete = true);

            var asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;

            while (!uiTransitionComplete || asyncOp.progress < 0.9f) yield return null;

            asyncOp.allowSceneActivation = true;
            while (!asyncOp.isDone) yield return null;

            AudioManager.Instance.StartFadeVolume(1f, audioFadeDuration);
            SceneTransitionUI.Instance.Hide(transitionDuration);
        }
        finally
        {
            IsTransitioning = false;
        }
    }
}

public enum SceneType
{
    Loading,
    MainMenu,
    Game,
}