using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour {
	[Header("Set Dynamically")]
	// Singleton
	private static UpdateManager _S;
	public static UpdateManager S { get { return _S; } set { _S = value; } }

	public delegate void 	LoopDelegate();
	public static event 	LoopDelegate updateDelegate;

	public delegate void 	FixedLoopDelegate();
	public static event 	FixedLoopDelegate fixedUpdateDelegate;

	void Awake () {
		S = this;
	}

	void Update () {
		if (updateDelegate != null) {
			updateDelegate ();
		}
	}

	void FixedUpdate(){
		if (fixedUpdateDelegate != null) {
			fixedUpdateDelegate ();
		}
	}
}
