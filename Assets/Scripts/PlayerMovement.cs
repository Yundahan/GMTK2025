using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Animator animator;

    private Vector3 velocity = Vector3.zero;
    private Vector2 spawnPoint;
    // The direction in which the character moves, 0 if no movement
    private float move;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spawnPoint = transform.position;
        animator = GetComponent<Animator>();

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
        float xSpeed = GetComponent<PlayerActions>().GetJumpBoosting() ? 0f : SPEED * horizontalAxis;
        Vector3 targetVelocity = new Vector3(xSpeed, rigidBody.linearVelocity.y, 0);
        rigidBody.linearVelocity = Vector3.SmoothDamp(rigidBody.linearVelocity, targetVelocity, ref velocity, IsGrounded() ? SMOOTHING : AIR_SMOOTHING);

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

        if (Input.GetKeyDown(KeyCode.W)) 

            animator.SetBool("isJumping", true);
        else
            animator.SetBool("isJumping", false);

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
        foreach (ShadowMovement shadow in GetComponent<ShadowManager>().GetShadows())
        {
            ShadowActions shadowActions = shadow.gameObject.GetComponent<ShadowActions>();
            Collider2D collider = shadow.gameObject.GetComponent<Collider2D>();

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

    public void Reset()
    {
        this.transform.position = spawnPoint;
        velocity = Vector3.zero;
    }
}
