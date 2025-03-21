using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private DiceVisual diceVisual;
    [SerializeField] private DiceMovement diceMovement;

    public Playboard Playboard { get; private set; }
    public bool IsKeeped { get; private set; } = false;
    public bool IsRolling { get; private set; } = false;

    private DiceFace[] faces;
    private int faceIndex;
    public int FaceIndex => faceIndex;
    private int faceIndexMax;

    void Start()
    {
        diceMovement.OnRollStarted += () => IsRolling = true;
        diceMovement.OnRollCompleted += () => IsRolling = false;
        diceMovement.OnDiceCollided += OnDiceCollided;
    }

    public void Init(int maxValue, Playboard playboard)
    {
        faces = new DiceFace[maxValue];
        var defaultDiceFaceList = DataContainer.Instance.DefaultDiceList.diceFaceList;

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i] = new();
            faces[i].Init(i + 1, defaultDiceFaceList[i]);
        }

        faceIndex = 0;
        faceIndexMax = maxValue - 1;

        Playboard = playboard;

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

    private void OnDiceCollided()
    {
        if (!IsKeeped)
        {
            ChangeFace(1);
        }
    }
}
