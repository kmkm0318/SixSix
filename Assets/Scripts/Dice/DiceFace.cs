using System;

public class DiceFace
{
    private int faceValue;
    public int FaceValue => faceValue;
    private DiceFaceSpriteSO faceSpriteSO;
    public DiceFaceSpriteSO FaceSpriteSO => faceSpriteSO;
    private ScorePair enhanceValue = new();
    public ScorePair EnhanceValue => enhanceValue;

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

        if (enhanceValue.multiplier == 0) enhanceValue.multiplier = 1;
        enhanceValue.multiplier += value.multiplier;
    }

    public void ApplyDiceFaceValue(Dice dice, bool isAvailityDice)
    {
        if (enhanceValue.baseScore == 0 && enhanceValue.multiplier == 0) return;

        if (enhanceValue.baseScore != 0)
        {
            ScorePair scorePair = new(enhanceValue.baseScore, 0);
            ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(dice, scorePair, isAvailityDice);
        }

        if (enhanceValue.multiplier != 0)
        {
            ScorePair scorePair = new(0, enhanceValue.multiplier);
            ScoreManager.Instance.ApplyScorePairAndPlayDiceAnimation(dice, scorePair, isAvailityDice);
        }
    }

    public string GetDescriptionText()
    {
        string res = "";

        if (enhanceValue.baseScore != 0)
        {
            res += $"\nGet Score(+{enhanceValue.baseScore})";
        }

        if (enhanceValue.multiplier != 0)
        {
            res += $"\nGet Score(x{enhanceValue.multiplier})";
        }

        return res;
    }
}
