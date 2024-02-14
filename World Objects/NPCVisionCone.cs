using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On player trigger enter, the burglar has been caught and the game is over.
public class NPCVisionCone : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject wordBubbleGO;

    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            // Activate "!" word bubble
            wordBubbleGO.SetActive(true);

            // The player has been seen by an NPC, game over!
            GameManager.S.levelEndManagerCS.GameOver();
        }
    }
}