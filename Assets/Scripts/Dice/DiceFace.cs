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
        enhanceValue.multiplier += value.multiplier;
    }
}
