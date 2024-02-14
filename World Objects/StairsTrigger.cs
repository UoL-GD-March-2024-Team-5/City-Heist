using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, sets the player's position to the opposite end (top or bottom) of this staircase
public class StairsTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public Transform    destinationTransform;
    public Animator     directionArrowAnim;

    public bool         isGoingUpstairs;
    public bool         onPlayerTriggerEnterDisplayHintPopUp;

    [Header("Set Dynamically")]
    public bool         playerIsInTrigger;

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (isGoingUpstairs) {
                    if (Input.GetAxisRaw("Vertical") > 0f) {
                        SetPlayerPosToOppositeEndOfStairs();
                    }
                } else {
                    if (Input.GetAxisRaw("Vertical") < 0f) {
                        SetPlayerPosToOppositeEndOfStairs();
                    }
                }
            }
        }
    }

    // Relocate player position to the opposite end of this staircase
    void SetPlayerPosToOppositeEndOfStairs() {
        // Set player to destination position 
        PlayerController.S.gameObject.transform.position = destinationTransform.position;

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.stairsSFX);
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = true;

            // Play direction arrow on animation clip
            directionArrowAnim.CrossFade("On", 0);

            // Activate hint pop up
            if (onPlayerTriggerEnterDisplayHintPopUp) {
                if (isGoingUpstairs) {
                    GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press Up Arrow or W to go upstairs.");
                } else {
                    GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press Down Arrow or S to go downstairs.");
                }
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = false;

            // Play direction arrow off animation clip
            directionArrowAnim.CrossFade("Off", 0);

            // Deactivate hint pop up
            GameManager.S.hintPopUpManagerCS.Deactivate();
        }
    }
}