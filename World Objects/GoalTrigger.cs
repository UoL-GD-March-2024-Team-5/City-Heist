using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On player trigger enter, level completed (display how well player performed, unlock next level, & activate menu
// with buttons to 1) Proceed to next level, 2) Try level again, 3) Go back to level selection).
public class GoalTrigger : MonoBehaviour {
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            GameManager.S.levelEndManagerCS.LevelCompleted();
        }
    }
}