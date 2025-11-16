using UnityEngine.EventSystems;

public class RollButton : ButtonPanel
{
    private bool _isActive = false;
    public bool IsActive
    {
        get => _isActive;
        private set
        {
            if (_isActive == value) return;

            if (value)
            {
                SequenceManager.Instance.AddCoroutine(() =>
                {
                    _isActive = true;
                });
            }
            else
            {
                _isActive = false;
            }
        }
    }

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
        IsActive = true;
    }

    private void OnPlayEnded()
    {
        IsActive = false;
    }

    private void OnRollStarted()
    {
        IsActive = false;
    }

    private void OnRollCompleted()
    {
        IsActive = RollManager.Instance.RollRemain > 0;
    }

    private void OnEnhanceStarted()
    {
        IsActive = true;
    }

    private void OnEnhanceCompleted()
    {
        IsActive = false;
    }
    #endregion

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!IsActive) return;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!IsActive) return;
        base.OnPointerUp(eventData);
    }
}