using UnityEngine;

/// <summary>
/// 다이스가 플레이되는 보드 클래스
/// </summary>
public class Playboard : MonoBehaviour
{
    [Header("Dice Generate Settings")]
    [SerializeField] Transform diceGenerateTransform;
    [SerializeField] float randomGenerateRange = 2f;
    public Vector2 DiceGeneratePosition => diceGenerateTransform.position + Vector3.right * Random.Range(-randomGenerateRange, randomGenerateRange);

    [Header("Force Settings")]
    [SerializeField] Transform forceTransform;
    public Vector2 ForcePosition => forceTransform.position;

    [Header("Particle Settings")]
    [SerializeField] float _minSqrMagnitude = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        if (collision.contactCount == 0) return;
        if (collision.relativeVelocity.sqrMagnitude < _minSqrMagnitude) return;

        ParticleEvents.TriggerOnDiceCollide(collision.contacts[0].point, collision.relativeVelocity);
    }
}
