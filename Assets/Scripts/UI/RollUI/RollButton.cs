using UnityEngine.EventSystems;

public class RollButton : ButtonPanel, IPointerDownHandler, IPointerUpHandler
{
    private bool isActive = false;

    #region RegisterEvents
    private void Start()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameManager.Instance.RegisterEvent(GameState.Play, OnPlayStarted, OnPlayEnded);
        GameManager.Instance.RegisterEvent(GameState.Roll, OnRollStarted, OnRollCompleted);
        GameManager.Instance.RegisterEvent(GameState.Enhance, OnEnhanceStarted, OnEnhanceCompleted);
    }

    private void OnPlayStarted()
    {
        isActive = true;
    }

    private void OnPlayEnded()
    {
        isActive = false;
    }

    private void OnRollStarted()
    {
        isActive = false;
    }

    private void OnRollCompleted()
    {
        isActive = RollManager.Instance.RollRemain > 0;
    }

    private void OnEnhanceStarted()
    {
        isActive = true;
    }

    private void OnEnhanceCompleted()
    {
        isActive = false;
    }
    #endregion

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isActive) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive) return;
        base.OnPointerUp(eventData);
    }
}