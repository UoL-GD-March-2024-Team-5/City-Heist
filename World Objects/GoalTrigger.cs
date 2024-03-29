using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On player trigger enter, level completed (display how well player performed, unlock next level, & activate menu
// with buttons to 1) Proceed to next level, 2) Try level again, 3) Go back to level selection).
public class GoalTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool onPlayerTriggerEnterDisplayHintPopUp;

    [Header("Set Dynamically")]
    public bool playerIsInTrigger;

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    GameManager.S.levelEndManagerCS.LevelCompleted();
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
                GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press E to enter getaway car and exit level with your loot.");
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