using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Collider2D feetCollider;

    private const int GROUND_LAYER = 6;
    private const float SPEED = 7f;
    private const float JUMP_FORCE = 150f;
    private const float SMOOTHING = 0.1f;

    private List<Collider2D> groundColliders = new();
    private Rigidbody2D rigidBody;

    private Vector3 velocity = Vector3.zero;
    private Vector2 spawnPoint;
    // The direction in which the character moves, 0 if no movement
    private float move;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;

        foreach (Collider2D collider2D in FindObjectsByType<Collider2D>(FindObjectsSortMode.None))
        {
            if (collider2D.gameObject.layer == GROUND_LAYER)
            {
                groundColliders.Add(collider2D);
            }
        }
    }
    void FixedUpdate()
    {
        Move(move);
    }

    public void Move(float horizontalAxis)
    {
        float xSpeed = SPEED * horizontalAxis;
        Vector3 targetVelocity = new Vector3(xSpeed, rigidBody.linearVelocity.y, 0);
        rigidBody.linearVelocity = Vector3.SmoothDamp(rigidBody.linearVelocity, targetVelocity, ref velocity, SMOOTHING);
    }

    public void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JUMP_FORCE, 0));
    }

    public void SetMove(float value)
    {
        this.move = value;
    }

    private bool IsGrounded()
    {
        foreach (Collider2D collider in groundColliders)
        {
            if (collider.IsTouching(feetCollider))
            {
                return true;
            }
        }

        return false;
    }

    public void Reset()
    {
        this.transform.position = spawnPoint;
        velocity = Vector3.zero;
    }
}
