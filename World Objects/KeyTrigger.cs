using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On trigger enter, add a key capable of opening locked doors to the player's inventory
public class KeyTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public List<string> message = new List<string>() { "Hey, you found a key...", "...now you can unlock a door!", };

    void OnTriggerEnter2D(Collider2D coll) {
        PlayerController player = coll.gameObject.GetComponent<PlayerController>();
        if (player != null) {
            // Deactivate gameObject
            gameObject.SetActive(false);

            // Freeze Player
            PlayerController.S.canMove = false;

            // Increment key count by one
            KeyManager.S.IncrementKeyCount(1);

            // Play SFX
            AudioManager.S.PlaySFX(eSFXAudioClipName.itemTriggerSFX);

            // Display text
            DialogueManager.S.DisplayText(message);
        }
    }
}