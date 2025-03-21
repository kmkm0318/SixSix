using System;
using System.Collections;
using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private float stopThreshold = 0.1f;
    [SerializeField] private float stopAngularThreshold = 5f;
    [SerializeField] private float stopTime = 0.5f;

    public event Action OnRollStarted;
    public event Action OnRollCompleted;
    public event Action OnDiceCollided;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        RollManager.Instance.OnRollPowerApplied += OnRollPowerApplied;
    }

    private void OnRollPowerApplied(float rollDiceForce)
    {
        RollDice(dice.Playboard.ForcePosition, rollDiceForce);
        StartCoroutine(CheckRollComplete());
    }

    public void RollDice(Vector2 forceOrigin, float force)
    {
        var forceDirection = (Vector2)transform.position - forceOrigin;
        forceDirection = forceDirection.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        rb.AddTorque(forceDirection.x * force, ForceMode2D.Impulse);

        OnRollStarted?.Invoke();
    }

    private IEnumerator CheckRollComplete()
    {
        float elapsedTime = 0f;
        while (elapsedTime < stopTime)
        {
            if (rb.linearVelocity.magnitude < stopThreshold && Mathf.Abs(rb.angularVelocity) < stopAngularThreshold)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime = 0f;
            }

            yield return null;
        }
        OnRollCompleted?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnDiceCollided?.Invoke();
    }
}