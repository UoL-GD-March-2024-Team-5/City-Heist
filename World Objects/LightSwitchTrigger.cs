using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, turn off lights, which makes the player invisible to detection
public class LightSwitchTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public Sprite           switchOffSprite, switchOnSprite;

    public GameObject       roomDarknessGO;

    [Header("Set Dynamically")]
    public SpriteRenderer   sRend;

    public bool             isTurnedOn = true;

    public bool             playerIsInTrigger;

    void Start() {
        sRend = GetComponent<SpriteRenderer>();

        roomDarknessGO.SetActive(false);
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    // Swap sprite, activate room darkness, & play SFX
                    if (isTurnedOn) {
                        sRend.sprite = switchOffSprite;
                        isTurnedOn = false;

                        roomDarknessGO.SetActive(true);

                        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
                    } else {
                        sRend.sprite = switchOnSprite;
                        isTurnedOn = true;

                        roomDarknessGO.SetActive(false);

                        AudioManager.S.PlaySFX(eSFXAudioClipName.buttonSelectedSFX);
                    }
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