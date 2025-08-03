using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Key : Interactable
{
    private const float THROWING_SPEED = 10f;
    private const float PICKUP_DISTANCE = 2f;

    private Sprite defaultSprite;
    public Sprite gemopst;

    private Vector2 keySpawnPoint;
    private Interacter interacter;
    private Rigidbody2D rigidBody;
    private Vector2 throwingDirection;
    private bool pickedUp = false;
    private bool hasBeenPickedUpAtLeastOnce = false;
    private float initialGravityScale;
    private Interaction lastPickUpEvent;
    private List<Tuple<Interaction, Interaction>> pickUpThrowPairs = new();
    private SpriteRenderer ownSpriteRenderer;
    public SpriteRenderer childSpriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        this.keySpawnPoint = this.transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        this.initialGravityScale = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = ownSpriteRenderer.sprite;
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
            float distanceToKey = Vector3.Distance(interaction.GetInteracter().transform.position, this.transform.position);

            // if someone tries to pickup a key from out of range, it's a shadow trying to pick up a key that is no longer there
            // remove corresponding throw from interaction list
            if (distanceToKey > PICKUP_DISTANCE)
            {
                Tuple<Interaction, Interaction> pair = pickUpThrowPairs.Find(pair => pair.Item1 == interaction);

                if (pair != null)
                {
                    player.GetComponent<LoopManager>().AddInteractionToDeletionList(pair.Item2);
                }

                return false;
            }

            // Pick up key
            this.interacter = interaction.GetInteracter().GetComponent<Interacter>();

            if (this.interacter.gameObject != player.gameObject)
            {
                ownSpriteRenderer.sprite = gemopst;
                childSpriteRenderer.sprite = gemopst;
            }

            ownSpriteRenderer.enabled = true;
            childSpriteRenderer.enabled = false;
            hasBeenPickedUpAtLeastOnce = true;
            pickedUp = true;
            interaction.SetThrowingDirection(Vector2.zero);
            this.throwingDirection = Vector3.zero;
            rigidBody.gravityScale = 0f;
            rigidBody.angularVelocity = 0f;
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            lastPickUpEvent = interaction;
            SFXManager.Instance().PlaySFX("Key");
        } else if (pickedUp)
        {
            // someone else than the current key holder tries to interact with it
            // either the player tries to steal, or a shadow tries to interact with a key thats no longer there
            if (this.interacter.gameObject != interaction.GetInteracter())
            {
                Tuple<Interaction, Interaction> pair = pickUpThrowPairs.Find(pair => pair.Item1 == interaction);

                // if the player is not the interacter, then the interaction was unsuccessful and returns false as a result
                if (player.gameObject != interaction.GetInteracter() && pair != null)
                {
                    // if the pair is not null, then there is a shadow trying to throw the key later on -> this interaction needs to be removed from the list
                    if (pair != null)
                    {
                        player.GetComponent<LoopManager>().AddInteractionToDeletionList(pair.Item2);
                    }

                    return false;
                }

                // at this point in the code, the player tries to steal. he isnt allowed to do that, but there are also no further consequences
                return true;
            }

            // Throw key
            ownSpriteRenderer.sprite = defaultSprite;
            childSpriteRenderer.sprite = defaultSprite;
            throwingDirection = interaction.GetThrowingDirection();
            pickedUp = false;
            rigidBody.linearVelocity = Vector3.zero;
            rigidBody.gravityScale = this.initialGravityScale;
            rigidBody.AddForce(throwingDirection * THROWING_SPEED);
            this.interacter = null;
            pickUpThrowPairs.Add(new Tuple<Interaction, Interaction>(lastPickUpEvent, interaction));
            SFXManager.Instance().PlaySFX("Yeet");
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
        rigidBody.gravityScale = 0f;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        this.transform.position = this.keySpawnPoint;
        this.interacter = null;
        hasBeenPickedUpAtLeastOnce = false;
        ownSpriteRenderer.enabled = false;
        childSpriteRenderer.enabled = true;
        GetComponent<TrajectoryRenderer>().Reset();
    }

    public Vector2 GetThrowingDirection()
    {
        return throwingDirection;
    }

    public bool IsInPlayerHand()
    {
        return this.pickedUp && this.interacter.GetComponent<Interacter>() == player;
    }

    public bool GetHasBeenPickedUpAtLeastOnce()
    {
        return hasBeenPickedUpAtLeastOnce;
    }
}
