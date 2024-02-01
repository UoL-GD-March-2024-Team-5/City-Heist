using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, opens this storage drawer/cabinet and instantiates collectable items for the player to steal.
public class DrawerCabinetTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             hasBeenOpened;

    public string           hasBeenOpenedMessage = "You've already looted this cabinet.\nIt's empty, you greedy pig.";

    public Sprite           openSprite;

    public List<GameObject> itemsToInstantiateOnOpen = new List<GameObject>();  

    [Header("Set Dynamically")]
    public SpriteRenderer   sRend;

    public bool             playerIsInTrigger;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (hasBeenOpened) {
                    // Display text
                    DialogueManager.S.DisplayText(hasBeenOpenedMessage);

                    // Play SFX
                    AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);

                    // Prevents calling DisplayText() above repeatedly on button press,
                    // and instead allows the DialogueManager to deactivate the text box.
                    playerIsInTrigger = false;
                } else {
                    // Instantiate collectable items
                    for (int i = 0; i < itemsToInstantiateOnOpen.Count; i++) {
                        GameObject go = Instantiate(itemsToInstantiateOnOpen[i], new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
                    }

                    // Swap sprite
                    sRend.sprite = openSprite;

                    // Play SFX
                    AudioManager.S.PlaySFX(eSFXAudioClipName.doorOpenSFX);

                    hasBeenOpened = true;
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
            InteractableCursor.S.Deactivate();
        }
    }
}