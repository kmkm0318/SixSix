using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Functions
{
    public static bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}