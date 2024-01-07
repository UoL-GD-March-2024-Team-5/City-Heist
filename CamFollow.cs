using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCamMode { freezeCam, followAll, followLR };

public class CamFollow : MonoBehaviour {
	[Header("Set in Inspector")]
	public Transform		targetTrans;

	[Header("Set Dynamically")]
	public float 			camPosX;
	public float 			camPosY;  
	private float			camPosZ = -10;

	// Smooth Lerp
	private float			easing = 0.15f; 
	private Vector3 		velocity = Vector3.zero;

	private Vector3			destination;

	public eCamMode 		camMode;

	// Clamps CamPosX
	public bool 			hasMinMaxPosX; 
	public float 			minPosX;
	public float 			maxPosX;

	public bool				canLerp;

	// Singleton
	private static CamFollow _S;
	public static CamFollow S { get { return _S; } set { _S = value; } }

	private static bool exists;

	void Awake () {
		// Singleton
		S = this;

		// DontDestroyOnLoad
		if (!exists) {
			exists = true;
			DontDestroyOnLoad (transform.gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
        if (!targetTrans) {
			Debug.LogError("targetTrans has NOT been assigned a transform in the Inspector.");
		}
	}
		
	void LateUpdate () {
		switch (camMode) {
		case eCamMode.freezeCam:
			destination.x = camPosX;
			destination.y = camPosY; 
			break;
		case eCamMode.followAll:
			destination = targetTrans.localPosition;
				break;
		case eCamMode.followLR:
			destination = targetTrans.position;

			// Locks Camera Pos.Y
			destination.y = camPosY; 
			break;
		}
				
		// Clamps CamPosX
		if (hasMinMaxPosX) {
			destination.x = Mathf.Max (destination.x, minPosX);
			destination.x = Mathf.Min (destination.x, maxPosX);
		}

		// Interpolate from the current Camera position towards Destination
		if (canLerp) {
			destination = Vector3.SmoothDamp(transform.localPosition, destination, ref velocity, easing);
		}

		// Keeps Pos.Z at -10
		destination.z = camPosZ; 

		// Set the Camera Pos to destination
		transform.localPosition = destination;	
	}
}