using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		PlayerController player = coll.gameObject.GetComponent<PlayerController>();
		if (player != null) {
			// Deactivate gameObject
			gameObject.SetActive(false);

			// Play SFX
			AudioManager.S.PlayPickUpSFX();
		} 
	}
}