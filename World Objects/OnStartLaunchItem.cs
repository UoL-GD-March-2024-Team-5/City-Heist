using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On start, launches this object with a random force and torque until it collides with the ground.
public class OnStartLaunchItem : MonoBehaviour {
    [Header("Set in Inspector")]
    public float    thrust = 2;

    [Header("Set dynamically")]
    Rigidbody2D     rigid;

    float           torque;

    bool            stopApplyingTorque;

    private void Start() {
        // Get rigid body
        rigid = GetComponent<Rigidbody2D>();

        // Get random force
        float xForce = Random.Range(-1.0f, 2.0f);
        float yForce = Random.Range(2.0f, 5.0f);

        // Get random torque
        torque = Random.Range(-1.0f, 2.0f);

        // Add force
        rigid.AddForce((new Vector2(xForce, yForce) * thrust), ForceMode2D.Impulse);
    }

    // Add torque
    private void FixedUpdate() {
        if (!stopApplyingTorque) {
            rigid.AddTorque((torque * thrust));
        }  
    }

    // On contact with ground, stop applying torque to this object
    protected virtual void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ground") {
            stopApplyingTorque = true;
        }
    }
}