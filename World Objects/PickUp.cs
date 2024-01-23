﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
	[Header("Set in Inspector")]
	public int value = 0;

    void OnTriggerEnter2D(Collider2D coll) {
		PlayerController player = coll.gameObject.GetComponent<PlayerController>();
		if (player != null) {
			// Deactivate gameObject
			gameObject.SetActive(false);

			// Play SFX
			AudioManager.S.PlayPickUpSFX();

			// Instantiate floating score game object (temporarily displays this item's value to the user)
            GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, "+$" + value, Color.green);
        } 
	}
}