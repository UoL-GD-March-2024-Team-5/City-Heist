using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On player trigger enter, if lights are turned off in this room, deactivate all NPC vision cones in scene
public class RoomDarknessTrigger : MonoBehaviour {
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            GameManager.S.countOfRoomDarknessTriggersCurrentlyOccupiedByPlayer += 1;

            // Dectivate all vision cones in current scene
            GameManager.S.ActivateVisionCones(false);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag == "PlayerTrigger") {
            GameManager.S.countOfRoomDarknessTriggersCurrentlyOccupiedByPlayer -= 1;

            // Activate all vision cones in current scene
            if(GameManager.S.countOfRoomDarknessTriggersCurrentlyOccupiedByPlayer < 1) {
                GameManager.S.ActivateVisionCones(true);
            }
        }
    }
}