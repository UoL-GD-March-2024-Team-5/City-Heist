using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On trigger enter, add a key capable of opening locked doors to the player's inventory
public class KeyTrigger : MonoBehaviour {
    [Header("Set in Inspector")]
    public List<string> message = new List<string>() { "Hey, you found a key...", "...now you can unlock a door!", };

    [Header("Set Dynamically")]
    private SpriteRenderer sRend;
    private CircleCollider2D circleColl;

    private void Start() {
        sRend = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D coll) {
        PlayerController player = coll.gameObject.GetComponent<PlayerController>();
        if (player != null) {
            // Play short celebratory jingle, then resume playing previous played BGM
            StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(4));

            // Set gravity to 0
            Physics2D.gravity = Vector2.zero;

            // Increment key count by one
            KeyManager.S.IncrementKeyCount(1);

            // Display text
            DialogueManager.S.DisplayText(message);

            // Deactivate sprite renderer & trigger
            sRend.enabled = false;
            circleColl.enabled = false;

            // Deactivate this game object in approximately 4 seconds (the delay
            // is needed, otherwise coroutine would be stopped prematurely)
            Invoke("DeactivateThisObject", 4f);
        }
    }

    // Deactivate gameObject
    void DeactivateThisObject() {
        gameObject.SetActive(false);
    }
}