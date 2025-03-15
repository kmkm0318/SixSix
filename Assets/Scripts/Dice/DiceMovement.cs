using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Roll(Vector2 forceOrigin, float force)
    {
        var forceDirection = (Vector2)transform.position - forceOrigin;
        forceDirection = forceDirection.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        rb.AddTorque(forceDirection.x * force, ForceMode2D.Impulse);
    }
}
