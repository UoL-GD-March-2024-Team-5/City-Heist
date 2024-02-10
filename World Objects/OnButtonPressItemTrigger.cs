using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
// On button press, add this item to the player's haul of stolen items
public class OnButtonPressItemTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public int  value = 0;

    [Header("Set Dynamically")]
    public bool playerIsInTrigger;

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    // Play SFX
                    AudioManager.S.PlaySFX(eSFXAudioClipName.itemTriggerSFX);

                    // Instantiate floating score game object (temporarily displays this item's value to the user)
                    GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, "+$" + value, Color.green);

                    // Increment total value of items stolen UI text
                    ScoreManager.S.IncrementScore(value);

                    // Increment amount of items stolen
                    GameManager.S.levelEndManagerCS.IncrementAmountStolen();

                    // Deactivate gameObject
                    gameObject.SetActive(false);
                }
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = true;

            // Activate Interactable Trigger
            InteractableCursor.S.Activate(gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            playerIsInTrigger = false;

            // Deactivate Interactable Trigger
            if (gameObject.activeInHierarchy) {
                InteractableCursor.S.Deactivate();
            }
        }
    }
}
