using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On level load, close then open black “curtains” to hide level initialization from the user.
public class LevelLoadTransition : MonoBehaviour {
    [Header("Set Dynamically")]
    public Animator anim;

    private static LevelLoadTransition _S;
    public static LevelLoadTransition S { get { return _S; } set { _S = value; } }

    void Awake() {
        S = this;
    }

    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Open() {
        if (Random.value > 0.5f) {
            anim.CrossFade("HorizontalOpen", 0);
        } else {
            anim.CrossFade("VerticalOpen", 0);
        }
    }

    public void Close() {
        if (Random.value > 0.5f) {
            anim.CrossFade("HorizontalClose", 0);
        } else {
            anim.CrossFade("VerticalClose", 0);
        }
    }
}