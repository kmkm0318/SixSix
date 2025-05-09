using System;
using System.Collections;
using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    [SerializeField] private float stopSpeedThreshold = 0.1f;
    [SerializeField] private float stopAngularThreshold = 5f;
    [SerializeField] private float stopTimeThreshold = 0.5f;

    private bool isRolling = false;
    public bool IsRolling
    {
        get => isRolling;
        private set
        {
            if (isRolling == value) return;
            isRolling = value;
        }
    }
    private Dice dice;
    private Rigidbody2D rb;
    private Playboard playboard;

    private void Awake()
    {
        dice = GetComponent<Dice>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Playboard playboard)
    {
        this.playboard = playboard;
    }

    public void OnRollPowerApplied(float rollDiceForce)
    {
        RollDice(playboard.ForcePosition, rollDiceForce);
        StartCoroutine(CheckRollComplete());
    }

    public void RollDice(Vector2 forceOrigin, float force)
    {
        var forceDirection = (Vector2)transform.position - forceOrigin;
        forceDirection.x *= forceDirection.x;
        forceDirection = forceDirection.normalized;
        rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        rb.AddTorque(forceDirection.x * force, ForceMode2D.Impulse);

        IsRolling = true;
    }

    private IEnumerator CheckRollComplete()
    {
        float elapsedTime = 0f;
        while (elapsedTime < stopTimeThreshold)
        {
            if (rb.linearVelocity.magnitude < stopSpeedThreshold && Mathf.Abs(rb.angularVelocity) < stopAngularThreshold)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime = 0f;
            }

            yield return null;
        }

        IsRolling = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << (collision.gameObject.layer)) & DataContainer.Instance.PlayergroundLayerMask) != 0)
        {
            dice.HandleDiceCollided();
        }
    }
}