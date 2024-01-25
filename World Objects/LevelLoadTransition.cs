using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On level load, close then open black “curtains” to hide level initialization from the user.
public class LevelLoadTransition : MonoBehaviour {
    [Header("Set in Inspector")]
    public Animator anim;

    [Header("Set Dynamically")]
    public bool isOpen;

    private static LevelLoadTransition _S;
    public static LevelLoadTransition S { get { return _S; } set { _S = value; } }

    void Awake() {
        S = this;
    }

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if(isOpen) {
                Close();
            } else {
                Open();
            }
        }
    }

    public void Open() {
        Debug.Log("OPEN");

        isOpen = true;

        if (Random.value > 0.5f) {
            //anim.Play("HorizontalOpen");
            anim.CrossFade("HorizontalOpen", 0);
        } else {
            //anim.Play("VerticalOpen");
            anim.CrossFade("VerticalOpen", 0);
        }
    }

    public void Close() {
        Debug.Log("CLOSE");

        isOpen = false;

        if (Random.value > 0.5f) {
            //anim.Play("HorizontalClose");
            anim.CrossFade("HorizontalClose", 0);
        } else {
            //anim.Play("VerticalClose");
            anim.CrossFade("VerticalClose", 0);
        }
    }
}