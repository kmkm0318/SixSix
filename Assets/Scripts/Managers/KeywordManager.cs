using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class KeywordManager : Singleton<KeywordManager>
{
    [SerializeField] private List<KeywordDataSO> keywordDataList;

    private Dictionary<string, KeywordDataSO> keywordDictionary;

    private static readonly Regex keywordRegex = new(@"<keyword=(\w+?)>", RegexOptions.Compiled);

    protected override void Awake()
    {
        base.Awake();
        InitDictionary();
    }

    private void InitDictionary()
    {
        keywordDictionary = new();
        foreach (var data in keywordDataList)
        {
            keywordDictionary[data.keyword] = data;
        }
    }

    public string GetKeywordString(string input)
    {
        return keywordRegex.Replace(input, match =>
        {
            string keyword = match.Groups[1].Value;
            KeywordDataSO data = GetKeyWordDataSO(keyword);

            if (data == null)
            {
                Debug.LogWarning($"Keyword '{keyword}' not found in dictionary.");
                return match.Value;
            }

            string localized = data.localizedName.GetLocalizedString();
            string color = ColorUtility.ToHtmlStringRGB(data.color);
            string spriteName = data.sprite != null ? data.sprite.name : "";

            return $"<sprite name=\"{spriteName}\"><color=#{color}>{localized}</color>";
        });
    }

    private KeywordDataSO GetKeyWordDataSO(string keyword)
    {
        return keywordDictionary.TryGetValue(keyword, out var data) ? data : null;
    }
}