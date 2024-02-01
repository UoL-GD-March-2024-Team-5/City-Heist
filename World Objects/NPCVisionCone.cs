using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On player trigger enter, the burglar has been caught and the game is over.
public class NPCVisionCone : MonoBehaviour {
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            // Game over
            Debug.Log("Burglar detected! Game is over!");
        }
    }
}