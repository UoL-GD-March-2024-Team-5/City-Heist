using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, opens this storage drawer/cabinet and instantiates collectable items for the player to steal.
public class DrawerCabinetTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             isLocked;

    public bool             onPlayerTriggerEnterDisplayHintPopUp;

    public string           hasBeenOpenedMessage = "You've already looted this cabinet.\nIt's empty, you greedy pig.";

    public Sprite           openSprite;

    public List<GameObject> itemsToInstantiateOnOpen = new List<GameObject>();  

    [Header("Set Dynamically")]
    public SpriteRenderer   sRend;

    public bool             hasBeenOpened;

    public bool             playerIsInTrigger;

    int                     secondsLeftToLockpick = 5;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    if (hasBeenOpened) {
                        // Display text
                        DialogueManager.S.DisplayText(hasBeenOpenedMessage);

                        // Play SFX
                        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
                    } else {
                        if (!isLocked) {
                            Open();
                        } else {
                            // Reset seconds left to lockpick this object
                            secondsLeftToLockpick = 5;

                            StartCoroutine("AttemptToLockpick", secondsLeftToLockpick);
                        }
                    }

                    // Prevents calling DisplayText() above repeatedly on button press,
                    // and instead allows the DialogueManager to deactivate the text box.
                    playerIsInTrigger = false;
                }
            }
        }
    }

    void Open() {
        // Deactivate hint pop up
        GameManager.S.hintPopUpManagerCS.Deactivate();

        // Instantiate collectable items
        for (int i = 0; i < itemsToInstantiateOnOpen.Count; i++) {
            GameObject go = Instantiate(itemsToInstantiateOnOpen[i], new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
        }

        // Swap sprite
        sRend.sprite = openSprite;

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.doorOpenSFX);

        isLocked = false;
        hasBeenOpened = true;
    }

    // Countdown from 5 seconds until the player has lockpicked this object
    public IEnumerator AttemptToLockpick(int secondsLeft) {
        // Instantiate a floating number that displays the amount of seconds left until the player's lockpicked this object
        InstantiateSecondsLeftToLockpickFloatingNumber(secondsLeft);

        // Decrement seconds left until this object is unlocked
        secondsLeft -= 1;

        // Wait for 1 second
        yield return new WaitForSecondsRealtime(1);

        if (secondsLeft > 0) {
            // Play SFX
            AudioManager.S.PlaySFX(eSFXAudioClipName.dialogueEndSFX);

            // Loop this coroutine
            StartCoroutine("AttemptToLockpick", secondsLeft);
        } else {
            // Play SFX
            AudioManager.S.PlaySFX(eSFXAudioClipName.pauseAudioSource);

            // Finally lockpick and open this object
            Open();
        }
    }

    // Instantiate a floating number that displays the amount of seconds left until the player's lockpicked this object
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

            // Activate interactable cursor
            InteractableCursor.S.Activate(gameObject);

            // Activate hint pop up
            if (!hasBeenOpened) {
                if (!isLocked) {
                    GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press E to open.");
                } else {
                    GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press E to lockpick in 5 seconds.");
                }
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

            StopCoroutine("AttemptToLockpick");
        }
    }
}