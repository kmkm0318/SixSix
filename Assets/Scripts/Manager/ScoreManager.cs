using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum HandCategory
{
    Ones,
    Twos,
    Threes,
    Fours,
    Fives,
    Sixes,
    FourOfAKind,
    FullHouse,
    DoubleThreeOfAKind,
    SmallStraight,
    LargeStraight,
    FullStraight,
    Yacht,
    SixSix,
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<HandCategory, int> CalculateHandScore(List<int> numList)
    {
        if (numList == null || numList.Count == 0) return null;

        Dictionary<HandCategory, int> res = new();
        Dictionary<int, int> countMap = new();

        foreach (var num in numList)
        {
            if (countMap.ContainsKey(num))
            {
                countMap[num]++;
            }
            else
            {
                countMap[num] = 1;
            }
        }

        res[HandCategory.Ones] = countMap.GetValueOrDefault(1, 0);
        res[HandCategory.Twos] = countMap.GetValueOrDefault(2, 0) * 2;
        res[HandCategory.Threes] = countMap.GetValueOrDefault(3, 0) * 3;
        res[HandCategory.Fours] = countMap.GetValueOrDefault(4, 0) * 4;
        res[HandCategory.Fives] = countMap.GetValueOrDefault(5, 0) * 5;
        res[HandCategory.Sixes] = countMap.GetValueOrDefault(6, 0) * 6;

        if (countMap.ContainsValue(4))
        {
            res[HandCategory.FourOfAKind] = numList.Sum();
        }
        else
        {
            res[HandCategory.FourOfAKind] = 0;
        }

        if ((numList.Count == 6 && countMap.ContainsValue(3) && countMap.Count == 2) || (countMap.ContainsValue(3) && countMap.ContainsValue(2)))
        {
            res[HandCategory.FullHouse] = numList.Sum();
        }
        else
        {
            res[HandCategory.FullHouse] = 0;
        }

        if (numList.Count == 6 && countMap.ContainsValue(3) && countMap.Count == 2)
        {
            res[HandCategory.DoubleThreeOfAKind] = numList.Sum() * 2;
        }
        else
        {
            res[HandCategory.DoubleThreeOfAKind] = 0;
        }

        int straightCount = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (countMap.ContainsKey(i))
            {
                straightCount++;
            }
            else
            {
                straightCount = 0;
            }

            if (straightCount >= 4)
            {
                res[HandCategory.SmallStraight] = 15;
            }
            else if (straightCount >= 5)
            {
                res[HandCategory.LargeStraight] = 30;
            }
            else if (straightCount >= 6)
            {
                res[HandCategory.FullStraight] = 100;
            }
        }

        if (countMap.ContainsValue(5))
        {
            res[HandCategory.Yacht] = 50;
        }
        else
        {
            res[HandCategory.Yacht] = 0;
        }

        if (countMap.ContainsValue(6))
        {
            res[HandCategory.SixSix] = 100;
        }
        else
        {
            res[HandCategory.SixSix] = 0;
        }

        return res;
    }
}
