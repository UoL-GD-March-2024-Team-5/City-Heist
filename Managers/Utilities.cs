using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A set of general helper functions to be used across the entire project.
public class Utilities : MonoBehaviour {
    [Header("Set Dynamically")]
    private static Utilities _S;
    public static Utilities S { get { return _S; } set { _S = value; } }

    void Awake() {
        S = this;
    }
}
