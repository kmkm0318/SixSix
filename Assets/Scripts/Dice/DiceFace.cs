using System;
using System.Diagnostics;
using UnityEngine.SocialPlatforms.Impl;

public class DiceFace
{
    private int faceValue;
    public int FaceValue => faceValue;
    private DiceFaceSpriteSO faceSpriteSO;
    public DiceFaceSpriteSO FaceSpriteSO => faceSpriteSO;
    private ScorePair enhanceValue = new(0, 0);
    public ScorePair EnhanceValue => enhanceValue;
    private ScorePair ApplyValue => new(enhanceValue.baseScore, enhanceValue.multiplier + 1f);

    public void Init(int faceValue, DiceFaceSpriteSO faceSpriteSO)
    {
        this.faceValue = faceValue;
        this.faceSpriteSO = faceSpriteSO;
    }

    public void SetFaceSpriteSO(DiceFaceSpriteSO faceSpriteSO)
    {
        this.faceSpriteSO = faceSpriteSO;
    }

    public void Enhance(ScorePair value)
    {
        enhanceValue.baseScore += value.baseScore;
        enhanceValue.multiplier += value.multiplier;
    }

    public void ApplyDiceFaceValue(Dice dice, bool isAvailityDice)
    {
        ScoreManager.Instance.ApplyDiceScorePairEffectAndPlayAnimation(dice, ApplyValue, isAvailityDice);
    }

    public string GetDescriptionText()
    {
        if (enhanceValue.baseScore == 0f && enhanceValue.multiplier == 0f)
        {
            return string.Empty;
        }
        else
        {
            return $"\nGet {ApplyValue}";
        }
    }
}
