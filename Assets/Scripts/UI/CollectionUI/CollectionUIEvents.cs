using System;

public static class CollectionUIEvents
{
    public static event Action OnCollectionButtonClicked;

    public static void TriggerOnCollectionButtonClicked() => OnCollectionButtonClicked?.Invoke();
}