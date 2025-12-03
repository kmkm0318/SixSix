using TMPro;
using UnityEngine;

/// <summary>
/// 리더보드에서 각 플레이어의 이름과 점수를 표현하는 하나의 UI
/// </summary>
public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _scoreText;

    public void Init(string playerName, double score)
    {
        _nameText.text = playerName;
        _scoreText.text = UtilityFunctions.FormatNumber(score);
    }
}