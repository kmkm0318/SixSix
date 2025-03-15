using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private DiceVisual diceVisual;

    private DiceFace[] faces;
    private int faceIndex;
    private int faceIndexMax;

    public void Init(int maxValue)
    {
        faces = new DiceFace[maxValue];
        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, DataContainer.Instance.DefaultFaces[i]);
        }

        faceIndex = 0;
        faceIndexMax = maxValue - 1;
    }
}
