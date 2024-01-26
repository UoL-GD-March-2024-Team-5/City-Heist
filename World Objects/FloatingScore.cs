using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gradually increases the attached game object's y-position, and optionally, its scale.
public class FloatingScore : MonoBehaviour {
    [Header("Set in Inspector")]
    public float speed = 1f;

    [Header("Set Dynamically")]
    public bool canScale;

    void OnEnable() {
        StartCoroutine("FixedUpdateCoroutine");
    }

    void OnDisable() {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public IEnumerator FixedUpdateCoroutine() {
        // Increase y-pos
        Vector3 tPos = transform.localPosition;
        tPos.y += speed * Time.fixedDeltaTime;
        transform.localPosition = tPos;

        // Increase scale
        if (canScale) {
            transform.localScale += (Vector3.one * Time.fixedDeltaTime) / 3;
        }

        // Start/loop coroutine again
        yield return new WaitForFixedUpdate();
        StartCoroutine("FixedUpdateCoroutine");
    }
}