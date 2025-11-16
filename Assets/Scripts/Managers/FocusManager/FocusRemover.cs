using UnityEngine;
using UnityEngine.EventSystems;

public class FocusRemover : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(null);
    }

    private void OnEnable()
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(null);
    }

    private void OnDisable()
    {
        if (FocusManager.Instance) FocusManager.Instance.OnClick(null);
    }
}