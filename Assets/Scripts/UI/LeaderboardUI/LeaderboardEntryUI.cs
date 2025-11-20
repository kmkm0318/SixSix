using TMPro;
using UnityEngine;

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