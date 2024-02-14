using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, turn off lights, which makes the player invisible to detection
public class LightSwitchTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public bool             onPlayerTriggerEnterDisplayHintPopUp;

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

            // Activate interactable cursor
            InteractableCursor.S.Activate(gameObject);

            // Activate hint pop up
            if (onPlayerTriggerEnterDisplayHintPopUp) {
                GameManager.S.hintPopUpManagerCS.ActivateAndSetText("Press E to turn lights on/off.");
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