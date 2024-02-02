using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On button press, activate this object, a TV or radio, to lure people and pets away
public class DistractionTrigger : MonoBehaviour {
    [Header("Set Dynamically")]
    public Animator anim;

    public bool     isTurnedOn;

    public bool     playerIsInTrigger;

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Update() {
        if (playerIsInTrigger) {
            if (!GameManager.S.paused) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    // Swap animation  & play SFX
                    if (isTurnedOn) {
                        anim.CrossFade("Off", 0);
                        isTurnedOn = false;

                        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
                    } else {
                        anim.CrossFade("On", 0);
                        isTurnedOn = true;

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