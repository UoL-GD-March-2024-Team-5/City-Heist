using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if (player != null) {
			// Deactivate gameObject
			gameObject.SetActive(false);
		} 
	}
}