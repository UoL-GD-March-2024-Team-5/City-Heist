using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, hides the player within this object, obscuring them from the view of NPCs
public class HideTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             onPlayerTriggerEnterDisplayHintPopUp;

    public Sprite           openSprite, closedSprite;

    [Header("Set Dynamically")]
    public bool             playerIsHidingInside;

    public bool             playerIsInTrigger;

    public SpriteRenderer   sRend;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    // Swap sprite, hide player, & play SFX
                    if (playerIsHidingInside) {
                        sRend.sprite = openSprite;
                        playerIsHidingInside = false;

                        PlayerController.S.StopHiding();

                        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);

                        // Activate all vision cones in current scene
                        if(GameManager.S.countOfRoomDarknessTriggersCurrentlyOccupiedByPlayer <= 0) {
                            GameManager.S.ActivateVisionCones(true);
                        }
                    } else {
                        sRend.sprite = closedSprite;
                        playerIsHidingInside = true;

                        PlayerController.S.Hide();

                        AudioManager.S.PlaySFX(eSFXAudioClipName.buttonSelectedSFX);

                        // Set camera position to that of the player
                        CameraManager.S.SetCamPosition(PlayerController.S.gameObject.transform.position);

                        // Deactivate all vision cones in current scene
                        GameManager.S.ActivateVisionCones(false);
                    }
                }
            }
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = true;

            // Activate interactable cursor
            InteractableCursor.S.Activate(gameObject);

            // Activate hint pop up
            if (onPlayerTriggerEnterDisplayHintPopUp) {
                GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press E to hide inside or exit.");
            }
        }
    }
    
    protected virtual void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = false;

            // Deactivate interactable cursor
            InteractableCursor.S.Deactivate();

            // Deactivate hint pop up
            GameManager.S.hintPopUpManagerCS.Deactivate();
        }
    }
}
