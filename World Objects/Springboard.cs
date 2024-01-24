using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Springboard : MonoBehaviour {
	[Header("Set in Inspector")]
	// Direction to launch
	public Vector3 springDirection = new Vector3(0, 1, 0);

	// Sprites
	public Sprite springUp;
	public Sprite springDown;

	[Header("Set Dynamically")]
	private SpriteRenderer sRend;

	// Amount of time until sprite changes back to springUp
	private float timeToPopUp = 0.5f;
	private float timeToPopUpDone = 0;

	void Start() {
		sRend = GetComponent<SpriteRenderer>();
	}

	public IEnumerator FixedUpdateCoroutine() {
		// Change sprite back to springUp
		if (Time.time >= timeToPopUpDone) {
			sRend.sprite = springUp;
		}

		// Call coroutine again
		yield return new WaitForFixedUpdate();
		StartCoroutine("FixedUpdateCoroutine");
	}

	void OnBecameVisible() {
		StartCoroutine("FixedUpdateCoroutine");
	}
	void OnBecameInvisible() {
		StopCoroutine("FixedUpdateCoroutine");
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			// Launch Player
			PlayerController.S.rigid.velocity = springDirection * 20;

			// Change sprite
			sRend.sprite = springDown;
			
			// Set timer 
			timeToPopUpDone = Time.time + timeToPopUp;

			// Play SFX
			AudioManager.S.PlaySFX(eAudioClipName.springboardSFX);
		}
	}
}