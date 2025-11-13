using UnityEngine.EventSystems;

public class RollButton : ButtonPanel
{
    private bool _isActive = false;

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
        _isActive = true;
    }

    private void OnPlayEnded()
    {
        _isActive = false;
    }

    private void OnRollStarted()
    {
        _isActive = false;
    }

    private void OnRollCompleted()
    {
        _isActive = RollManager.Instance.RollRemain > 0;
    }

    private void OnEnhanceStarted()
    {
        _isActive = true;
    }

    private void OnEnhanceCompleted()
    {
        _isActive = false;
    }
    #endregion

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!_isActive) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!_isActive) return;
        base.OnPointerUp(eventData);
    }
}