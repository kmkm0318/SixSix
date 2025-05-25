using UnityEngine;

public class DiceFace
{
    private int faceValue;
    public int FaceValue => faceValue;
    private Sprite currentSprite;
    public Sprite CurrentSprite => currentSprite;
    private ScorePair enhanceValue = new(0, 0);
    public ScorePair EnhanceValue => enhanceValue;
    private ScorePair ApplyValue => new(enhanceValue.baseScore, enhanceValue.multiplier + 1f);

    public void Init(int faceValue, Sprite sprite)
    {
        this.faceValue = faceValue;
        currentSprite = sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        currentSprite = sprite;
    }

    public void Enhance(ScorePair value)
    {
        enhanceValue.baseScore += value.baseScore;
        enhanceValue.multiplier += value.multiplier;
    }

    public void ApplyFaceValue(Dice dice, bool isAbilityDice)
    {
        TriggerManager.Instance.ApplyTriggerEffect(dice.transform, isAbilityDice ? Vector3.down : Vector3.up, ApplyValue);
    }

    public string GetDescriptionText()
    {
        if (enhanceValue.baseScore == 0f && enhanceValue.multiplier == 0f)
        {
            return string.Empty;
        }
        else
        {
            var getScoreDescription = DiceManager.Instance.GetScoreDescription;
            getScoreDescription.Arguments = new object[] { ApplyValue };
            getScoreDescription.RefreshString();
            return getScoreDescription.GetLocalizedString();
        }
    }
}
