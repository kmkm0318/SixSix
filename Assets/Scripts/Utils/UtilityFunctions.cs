using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UtilityFunctions
{
    public static bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    public static string FormatNumber(float value)
    {
        if (value >= 1e12f)
        {
            return value.ToString("0.00e0");
        }
        return value.ToString("N0");
    }
}