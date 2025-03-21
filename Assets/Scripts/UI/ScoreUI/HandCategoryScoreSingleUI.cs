using TMPro;
using UnityEngine;

public class HandCategoryScoreSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text baseScoreText;
    [SerializeField] private TMP_Text multiplierText;

    public void Init(HandCategorySO handCategorySO)
    {
        nameText.text = handCategorySO.handCategoryName;
        baseScoreText.text = "0";
        multiplierText.text = "0";
    }

    public void UpdateScore(ScorePair scorePair)
    {
        baseScoreText.text = scorePair.baseScore.ToString();
        multiplierText.text = scorePair.multiplier.ToString();
    }
}
