using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private DiceVisual diceVisual;

    private DiceFace[] faces;
    private int faceIndex;
    private int faceIndexMax;

    private Playboard playboard;
    public Playboard Playboard => playboard;

    public void Init(int maxValue, Playboard playboard)
    {
        faces = new DiceFace[maxValue];
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, DataContainer.Instance.DefaultFaces[i]);
        }

        faceIndex = 0;
        faceIndexMax = maxValue - 1;

        this.playboard = playboard;

        SetFace(faceIndex);
    }

    public void SetFace(int faceIndex)
    {
        diceVisual.SetSprite(faces[faceIndex].FaceSpriteSO.sprite);
    }

    public void ChangeFace(int value = 1)
    {
        faceIndex += value;
        faceIndex %= faceIndexMax;
        SetFace(faceIndex);
    }
}
