public class DiceFace
{
    private int faceValue;
    public int FaceValue => faceValue;
    private DiceFaceSpriteSO faceSpriteSO;
    public DiceFaceSpriteSO FaceSpriteSO => faceSpriteSO;

    public void Init(int faceValue, DiceFaceSpriteSO faceSpriteSO)
    {
        this.faceValue = faceValue;
        this.faceSpriteSO = faceSpriteSO;
    }

    public void SetFaceSpriteSO(DiceFaceSpriteSO faceSpriteSO)
    {
        this.faceSpriteSO = faceSpriteSO;
    }
}
