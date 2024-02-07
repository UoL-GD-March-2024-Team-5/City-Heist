using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press while intersected with PlayerTrigger, opens this door (changes sprite, & deactivates collider & trigger).
public class DoorTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             doorIsLocked;

    public bool             doorOpensToLeft;

    public string           doorIsLockedMessage = "This door is locked. Find a key!";
    public string           doorIsUnlockedMessage = "Great, you unlocked the door!";

    public Sprite           closedDoorSprite, openDoorLeftSprite, openDoorRightSprite, lockedDoorSprite;

    public EdgeCollider2D   solidColl;
    public BoxCollider2D    triggerColl;

    [Header("Set Dynamically")]
    public SpriteRenderer   sRend;

    public bool             playerIsInTrigger;

    int                     secondsLeftToLockpick = 5;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();

        // Set initial sprite to render
        if (doorIsLocked) {
            sRend.sprite = lockedDoorSprite;
        }
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    if (!doorIsLocked) {
                        OpenDoor();
                    } else {
                        if (KeyManager.S.keyCount > 0) {
                            UnlockAndOpenDoor();
                        } else {
                            // Reset seconds left to lockpick door
                            secondsLeftToLockpick = 5;

                            StartCoroutine("AttemptToLockpickDoor", secondsLeftToLockpick);
                        }

                        // Prevents calling DisplayText() above repeatedly on button press,
                        // and instead allows the DialogueManager to deactivate the text box.
                        playerIsInTrigger = false;
                    }
                }
            }
        }
    }

    void OpenDoor() {
        // Swap sprite
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

    void UnlockAndOpenDoor() {
        // Display text
        DialogueManager.S.DisplayText(doorIsUnlockedMessage);

        // Unlock door
        doorIsLocked = false;

        // Decrement key count
        KeyManager.S.IncrementKeyCount(-1);

        // Play short celebratory jingle, then resume playing previous played BGM
        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(4));

        OpenDoor();
    }

    // Unused after '5 seconds to lockpick' mechanic introduced
    void DoorIsLocked() {
        // Display text
        DialogueManager.S.DisplayText(doorIsLockedMessage);

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
    }

    // Countdown from 5 seconds until the player has lockpicked this door
    public IEnumerator AttemptToLockpickDoor(int secondsLeft) {
        // Instantiate a floating number that displays the amount of seconds left until the player's lockpicked this door
        InstantiateSecondsLeftToLockpickFloatingNumber(secondsLeft);

        // Decrement seconds left until door unlocked
        secondsLeft -= 1;

        // Wait for 1 second
        yield return new WaitForSecondsRealtime(1);

        if (secondsLeft > 0) {
            // Play SFX
            AudioManager.S.PlaySFX(eSFXAudioClipName.dialogueEndSFX);

            // Loop this coroutine
            StartCoroutine("AttemptToLockpickDoor", secondsLeft);
        } else {
            // Play SFX
            AudioManager.S.PlaySFX(eSFXAudioClipName.pauseAudioSource);

            // Finally lockpick and open door
            UnlockAndOpenDoor();
        }
    }

    // Instantiate a floating number that displays the amount of seconds left until the player's lockpicked this door
    void InstantiateSecondsLeftToLockpickFloatingNumber(int secondsLeft) {
        switch (secondsLeft) {
            case 5:
            case 4:
                GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, secondsLeft.ToString(), Color.red);
                break;
            case 3:
            case 2:
                GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, secondsLeft.ToString(), Color.yellow);
                break;
            case 1:
                GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, secondsLeft.ToString(), Color.green);
                break;
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
            InteractableCursor.S.Deactivate();

            StopCoroutine("AttemptToLockpickDoor");
        }
    }
}