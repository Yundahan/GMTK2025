using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Collider2D feetCollider;

    private const int GROUND_LAYER = 6;
    private const float SPEED = 7f;
    private const float JUMP_FORCE = 150f;
    private const float BOOSTED_JUMP_FORCE = 180f;
    private const float SMOOTHING = 0.1f;
    private const float AIR_SMOOTHING = 0.2f;

    private List<Collider2D> groundColliders = new();
    private Rigidbody2D rigidBody;
    public SpriteRenderer idleRenderer;
    public SpriteRenderer movingRenderer;

    public Animator idleAnimator;
    public Animator movingAnimator;

    private Vector3 velocity = Vector3.zero;
    private Vector2 spawnPoint;
    // The direction in which the character moves, 0 if no movement
    private float move;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        idleAnimator = GetComponent<Animator>();

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

        if (rigidBody.linearVelocity.y < 0 && !IsGrounded())
        {
            idleAnimator.SetBool("isFalling", true);
            movingAnimator.SetBool("isFalling", true);
            idleAnimator.SetBool("isJumping", false);
            movingAnimator.SetBool("isJumping", false);
        } else
        {
            idleAnimator.SetBool("isFalling", false);
            movingAnimator.SetBool("isFalling", false);
        }
    }

    public void Move(float horizontalAxis)
    {
        float xSpeed = GetComponent<PlayerActions>().GetJumpBoosting() ? 0f : SPEED * horizontalAxis;
        Vector3 targetVelocity = new Vector3(xSpeed, rigidBody.linearVelocity.y, 0);
        rigidBody.linearVelocity = Vector3.SmoothDamp(rigidBody.linearVelocity, targetVelocity, ref velocity, IsGrounded() ? SMOOTHING : AIR_SMOOTHING);

        if (horizontalAxis < 0)
        {
            idleAnimator.SetBool("isRunning", true);
            movingRenderer.enabled = true;
            idleRenderer.enabled = false;
            movingRenderer.flipX = true;
        } else if(horizontalAxis > 0)
        {
            idleAnimator.SetBool("isRunning", true);
            movingRenderer.enabled = true;
            idleRenderer.enabled = false;
            movingRenderer.flipX = false;
        } else
        {
            idleAnimator.SetBool("isRunning", false);
            movingRenderer.enabled = false;
            idleRenderer.enabled = true;
        }
    }

    public void SetMove(float value)
    {
        this.move = value;
    }

    public void Jump()
    {
        if (GetComponent<PlayerActions>().GetJumpBoosting())
        {
            return;
        }

        idleAnimator.SetBool("isJumping", true);
        movingAnimator.SetBool("isJumping", true);

        if (TouchesJumpBoostingShadow())
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, BOOSTED_JUMP_FORCE, 0));
        }
        else if (IsGrounded())
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector3(0, JUMP_FORCE, 0));
        }
    }

    private bool TouchesJumpBoostingShadow()
    {
        foreach (GameObject shadow in GetComponent<LoopManager>().GetShadows())
        {
            ShadowActions shadowActions = shadow.GetComponent<ShadowActions>();
            Collider2D collider = shadow.GetComponent<Collider2D>();

            if (shadowActions.GetJumpBoosting() 
                && collider != null 
                && collider.IsTouching(feetCollider))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsGrounded()
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

    public Vector2 GetSpawnPoint()
    {
        return spawnPoint;
    }

    public Collider2D GetFeet()
    {
        return feetCollider;
    }

    public void Reset()
    {
        this.transform.position = spawnPoint;
        velocity = Vector3.zero;
    }
}
