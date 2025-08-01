using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private const float THROWING_SPEED = 10f;

    private Vector2 keySpawnPoint;
    public Interacter interacter;
    private Rigidbody2D rigidBody;
    private Vector2 throwingDirection;
    private bool pickedUp = false;
    private float initialGravityScale;
    private Interaction lastPickUpEvent;
    private List<Tuple<Interaction, Interaction>> pickUpThrowPairs = new();

    protected override void Awake()
    {
        base.Awake();
        this.keySpawnPoint = this.transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        this.initialGravityScale = rigidBody.gravityScale;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {
        if (pickedUp)
        {
            this.transform.position = this.interacter.transform.position;
        }
    }

    public override bool Interact(Interaction interaction)
    {
        if (!pickedUp)
        {
            // Pick up key
            pickedUp = true;
            this.interacter = interaction.GetInteracter().GetComponent<Interacter>();
            rigidBody.gravityScale = 0f;
            lastPickUpEvent = interaction;
        } else if (pickedUp)
        {
            if (this.interacter.gameObject != interaction.GetInteracter())
            {
                Interaction throwToDelete = pickUpThrowPairs.Find(pair => pair.Item1 == interaction).Item2;
                player.GetComponent<LoopManager>().AddInteractionToDeletionList(throwToDelete);
                return false;
            }

            // Throw key
            throwingDirection = interaction.GetThrowingDirection();
            pickedUp = false;
            rigidBody.AddForce(throwingDirection * THROWING_SPEED);
            rigidBody.gravityScale = this.initialGravityScale;
            this.interacter = null;
            pickUpThrowPairs.Add(new Tuple<Interaction, Interaction>(lastPickUpEvent, interaction));
        }

        return true;
    }

    public override Interaction RecordInteraction()
    {
        Interaction interaction = base.RecordInteraction();
        interaction.SetThrowingDirection(throwingDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);
        return interaction;
    }

    public override void Reset()
    {
        base.Reset();
        pickedUp = false;
        rigidBody.gravityScale = initialGravityScale;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        this.transform.position = this.keySpawnPoint;
    }

    public Vector2 GetThrowingDirection()
    {
        return throwingDirection;
    }

    public bool IsInPlayerHand()
    {
        return this.pickedUp && this.interacter.GetComponent<Interacter>() == player;
    }
}
