using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press while intersected with PlayerTrigger, opens this door (changes sprite, & deactivates collider & trigger).
public class DoorTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             doorIsLocked;

    public bool             doorOpensToLeft;

    public string           doorIsLockedMessage = "This door is locked. Find a key!";
    public string           doorIsUnlockedMessage = "Great, you unlocked the door. Now open it!";

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
                if (!doorIsLocked) {
                    OpenDoor();
                } else {
                    if(KeyManager.S.keyCount > 0) {
                        // Display text
                        DialogueManager.S.DisplayText(doorIsUnlockedMessage);

                        // Unlock door
                        doorIsLocked = false;

                        // Decrement key count
                        KeyManager.S.IncrementKeyCount(-1);

                        // Play short celebratory jingle, then resume playing previous played BGM
                        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(4));

                        OpenDoor();
                    } else {
                        // Display text
                        DialogueManager.S.DisplayText(doorIsLockedMessage);

                        // Play SFX
                        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
                    }

                    // Prevents calling DisplayText() above repeatedly on button press,
                    // and instead allows the DialogueManager to deactivate the text box.
                    playerIsInTrigger = false;
                }
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
        AudioManager.S.PlaySFX(eSFXAudioClipName.doorOpenSFX);
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