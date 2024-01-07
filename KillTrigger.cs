using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.tag == "Player") {
			Player.S.SetPlayerToStartingPosition();
		}
	}
}
