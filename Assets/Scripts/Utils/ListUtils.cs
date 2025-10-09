using System.Collections.Generic;
using UnityEngine;

public static class ListUtils
{
    public static bool IsNullOrEmpty<T>(List<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) return default;

        return list[Random.Range(0, list.Count)];
    }

    public static List<T> GetRandomElements<T>(this List<T> list, int count)
    {
        List<T> res = new();
        if (list == null || list.Count == 0) return res;

        if (list.Count <= count)
        {
            res.AddRange(list);
            return res;
        }

        List<T> tmp = new(list);
        for (int i = tmp.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (tmp[i], tmp[j]) = (tmp[j], tmp[i]);
        }

        for (int i = 0; i < count; i++) res.Add(tmp[i]);

        return res;
    }

    public static T GetWeightedRandomElement<T>(this List<WeightedItem<T>> list)
    {
        if (list == null || list.Count == 0) return default;

        int totalWeight = 0;
        foreach (var pair in list) totalWeight += pair.weight;

        if (totalWeight <= 0) return default;

        int rand = Random.Range(0, totalWeight);
        int sum = 0;
        foreach (var pair in list)
        {
            sum += pair.weight;
            if (sum > rand) return pair.item;
        }

        return default;
    }
}