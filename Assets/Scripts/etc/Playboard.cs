using UnityEngine;

public class Playboard : MonoBehaviour
{
    [SerializeField] Transform diceGenerateTransform;
    [SerializeField] float randomGenerateRange = 2f;
    public Vector2 DiceGeneratePosition => diceGenerateTransform.position + Vector3.right * Random.Range(-randomGenerateRange, randomGenerateRange);
    [SerializeField] Transform forceTransform;
    public Vector2 ForcePosition => forceTransform.position;

    void Start()
    {
        RollManager.Instance.OnRollStarted += StartJiggle;
        RollManager.Instance.OnRollCompleted += StopJiggle;
    }

    private void StartJiggle()
    {

    }

    private void StopJiggle()
    {

    }
}
