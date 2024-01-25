using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press while intersected with PlayerTrigger, opens this door (changes sprite, & deactivates collider & trigger).
public class DoorTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             doorOpensToLeft;

    public string           doorIsLockedMessage = "This door is locked. Find a key!";
    public string           doorIsUnlockedMessage = "Great! You unlocked the door!";

    public Sprite           closedDoorSprite, openDoorLeftSprite, openDoorRightSprite;

    public EdgeCollider2D   solidColl;
    public BoxCollider2D    triggerColl;

    [Header("Set Dynamically")]
    public SpriteRenderer   sRend;

    public bool             playerIsInTrigger;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (Input.GetKeyDown(KeyCode.E)) {
                OpenDoor();
            }
        }
    }

    void OpenDoor() {
        // Change sprite
        if (doorOpensToLeft) {
            sRend.sprite = openDoorLeftSprite;
        } else {
            sRend.sprite = openDoorRightSprite;
        }

        // Disable collider & trigger
        solidColl.enabled = false;
        triggerColl.enabled = false;

        // Deactivate Interactable Trigger
        InteractableCursor.S.Deactivate();

        // Play SFX
        AudioManager.S.PlaySFX(eAudioClipName.doorOpenSFX);
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
            InteractableCursor.S.Deactivate();
        }
    }
}