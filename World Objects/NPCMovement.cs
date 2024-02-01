using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMovement { walkRandomly, idle, patrol };
//
// Automates movement for non-player characters.
public class NPCMovement : MonoBehaviour {
    [Header("Set in Inspector")]
    // How the NPC will move ( 1: standing still, 2: patrolling between two points,
    // or 3: walking and waiting in random directions for random amounts of time)
    public eMovement    mode = eMovement.walkRandomly;

    // Object that the NPC will move towards 
    public Transform    movePoint;

    // If patrolling, these are the two points NPC will walk/patrol between
    public Transform    patrolTransformLeft, patrolTransformRight;

    // Layers that the NPC should not be able to walk through (doors, walls, etc.)
    public LayerMask    bounds;

    // Min and max values for how long to wait 
    public Vector2      waitDuration = new Vector2(1.0f, 4.0f);

    [Header("Set Dynamically")]
    Animator            anim;

    float               speed = 2f;

    bool                isWalking;

    float               timer = 1;

    // Direction the NPC's sprite is facing 
    bool                facingRight;

    // Direction the NPC is walking towards
    bool                isWalkingTowardsLeft;

    void Start() {
        anim = GetComponent<Animator>();

        // De-child movePoint from this game object
        movePoint.parent = null;

        // Start coroutine loop
        StartCoroutine("FixedUpdateCoroutine");
    }
    
    public IEnumerator FixedUpdateCoroutine() {
        // If not paused, and there isn't any dialogue being displayed...
        if (!GameManager.S.paused && !DialogueManager.S.frameImage.activeInHierarchy) {
            // Update NPC's actions depending on movement type and whether they're walking or waiting to walk
            switch (mode) {
                case eMovement.walkRandomly:
                    if (isWalking) {
                        Walk();
                    } else {
                        // Decrement timer
                        timer -= Time.deltaTime;

                        // If timer < 0, get a new destination to move towards and start moving
                        if (timer <= 0) {
                            SetRandomDirectionAndDistanceToMove();
                        }
                    }
                    break;
                case eMovement.patrol:
                    if (isWalking) {
                        Walk();
                    } else {
                        // Decrement timer
                        timer -= Time.deltaTime;

                        // If timer < 0, get a new destination to move towards and start moving
                        if (timer <= 0) {
                            StartPatrolling();
                        }
                    }
                    break;
            }
        }

        // Restart/loop coroutine 
        yield return new WaitForFixedUpdate();
        StartCoroutine("FixedUpdateCoroutine");
    }

    // Initialize NPC to start waiting
    public void StartWaiting() {
        isWalking = false;

        // Reset timer with random amount
        timer = Random.Range(waitDuration.x, waitDuration.y);

        // Play animation clip
        anim.CrossFade("Idle", 0);
    }

    // Initialize NPC to start walking
    public void StartWalking() {
        isWalking = true;

        // Play animation clip
        anim.CrossFade("Walk", 0);

        // Ensure NPC's sprite is facing the direction it's moving towards
        if (isWalkingTowardsLeft) {
            if (!facingRight) { Flip(); }
        } else {
            if (facingRight) { Flip(); }
        }
    }

    // Initialize NPC to start patrolling 
    void StartPatrolling() {
        // Swap direction to move towards
        isWalkingTowardsLeft = !isWalkingTowardsLeft;

        // Set move point position to opposite end
        if (isWalkingTowardsLeft) {
            movePoint.position = patrolTransformLeft.position;
        } else {
            movePoint.position = patrolTransformRight.position;
        }

        StartWalking();
    }

    // Moves NPC towards movePoint until they have reached it
    void Walk() {
        // Move NPC towards movePoint
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        // If NPC has reached the movePoint, then start waiting before moving again
        if (Vector3.Distance(transform.position, movePoint.position) == 0f) {
            StartWaiting();
        }
    }

    // Checks if the NPC's destination is valid (not inside of a door or wall),
    // if not, get another random destination and try again
    void CheckIfDestinationIsValid(Vector3 destinationPos) {
        // If NPC's trigger is not overlapping with any bounds (doors, walls, etc.)...
        if (!Physics2D.OverlapCircle(movePoint.position + destinationPos, 0.2f, bounds)) { 
            // Move movePoint to destination position
            movePoint.position += destinationPos;

            StartWalking();
        } else {
            SetRandomDirectionAndDistanceToMove();
        }
    }

    // Sets a random distance and direction for the NPC to move towards
    void SetRandomDirectionAndDistanceToMove() {
        // Get random direction to walk towards (left or right)
        if (Random.value > 0.5f) {
            isWalkingTowardsLeft = false;
        } else {
            isWalkingTowardsLeft = true;
        }

        // Get random distance to move
        float distanceToMove = Random.Range(0.5f, 0.75f);

        // If destination is valid, move position of movePoint and start moving NPC towards it 
        if (isWalkingTowardsLeft) {
            CheckIfDestinationIsValid(new Vector3(-distanceToMove, 0f, 0f));
        } else {
            CheckIfDestinationIsValid(new Vector3(distanceToMove, 0f, 0f));
        }
    }

    // Flip which direction the NPC's sprite is facing horizontally (left or right)
    public void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}