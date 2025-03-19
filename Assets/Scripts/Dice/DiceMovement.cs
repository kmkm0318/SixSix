using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    [SerializeField] private Dice dice;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RollManager.Instance.OnRollDiceEvent += OnRollDice;
    }

    private void OnRollDice(float rollDiceForce)
    {
        RollDice(dice.Playboard.ForcePosition, rollDiceForce);
    }

    public void RollDice(Vector2 forceOrigin, float force)
    {
        var forceDirection = (Vector2)transform.position - forceOrigin;
        forceDirection = forceDirection.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        rb.AddTorque(forceDirection.x * force, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        dice.ChangeFace();
    }
}