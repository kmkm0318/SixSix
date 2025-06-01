using System;
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

    public static string FormatNumber(double value)
    {
        value = Math.Floor(value);
        if (value >= 1e12f)
        {
            return value.ToString("0.00e0");
        }
        return value.ToString("N0");
    }

    #region Arithmatic
    public static double SafeAdd(double value1, double value2)
    {
        double res = value1 + value2;
        if (double.IsInfinity(res) || double.IsNaN(res))
        {
            return double.PositiveInfinity;
        }
        else if (res < 0)
        {
            return 0;
        }
        else
        {
            return res;
        }
    }

    public static double SafeMultiply(double value1, double value2)
    {
        double res = value1 * value2;
        if (double.IsInfinity(res) || double.IsNaN(res))
        {
            return double.PositiveInfinity;
        }
        else if (res < 0)
        {
            return 0;
        }
        else
        {
            return res;
        }
    }
    #endregion
}