using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private DiceVisual diceVisual;
    [SerializeField] private DiceMovement diceMovement;
    [SerializeField] private DiceInteraction diceInteraction;
    public DiceInteraction DiceInteraction => diceInteraction;

    public event Action<bool> OnIsKeepedChanged;

    private bool isKeeped = false;
    public bool IsKeeped
    {
        get => isKeeped;
        private set
        {
            if (isKeeped == value) return;
            isKeeped = value;
            OnIsKeepedChanged?.Invoke(isKeeped);
        }
    }
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

        diceInteraction.OnMouseClicked += () => IsKeeped = !IsKeeped;
    }

    private void OnDiceCollided()
    {
        if (!IsKeeped)
        {
            ChangeFace(1);
        }
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

        SetFace(faceIndex);

        diceMovement.Init(playboard);
        diceVisual.Init(this);
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
