using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour {
	[Header("Set in Inspector")]
	public int		value = 0;
    public string	itemName = "";

    void OnTriggerEnter2D(Collider2D coll) {
		PlayerController player = coll.gameObject.GetComponent<PlayerController>();
		if (player != null) {
			// Deactivate gameObject
			gameObject.SetActive(false);

			// Play SFX
			AudioManager.S.PlaySFX(eSFXAudioClipName.itemTriggerSFX);

			// Instantiate floating score game object (temporarily displays this item's value to the user)
            GameManager.S.InstantiateFloatingScore(PlayerController.S.gameObject, (itemName + "\n+$" + value), Color.green);

			// Increment score/total value of stolen items
			ScoreManager.S.IncrementScore(value);
        } 
	}
}