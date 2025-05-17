using System;

public interface IShowHide
{
    void Show(Action onComplete = null);
    void Hide(Action onComplete = null);
}