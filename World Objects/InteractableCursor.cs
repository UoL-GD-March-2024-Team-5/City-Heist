using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

// Contains scripts related to on player trigger enter an interactable game object's trigger, activate and display a cursor
// above said object to help indicate that the user can interact with it.
public class InteractableCursor : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject cursorGO;

    [Header("Set Dynamically")]
    private static InteractableCursor _S;
    public static InteractableCursor S { get { return _S; } set { _S = value; } }

    void Awake() {
        S = this;
    }

    // Activates and positions cursor game object slightly above target game object
    public void Activate(GameObject newParentGO) {
        // Set cursor position to slighlty above target game object
        Vector2 tPos = newParentGO.transform.position;
        tPos.y += 0.25f;
        cursorGO.transform.position = tPos;

        // Set the cursor's parent game object
        cursorGO.transform.SetParent(newParentGO.transform);

        // Activate cursor game object
        cursorGO.SetActive(true);
    }

    // Activates and positions cursor game object at a specific position (Vector2)
    public void Activate(Vector2 newPosition, float yPosOffset = 0.25f) {
        // Set cursor position to newPosition
        Vector2 tPos = newPosition;
        tPos.y += yPosOffset;
        cursorGO.transform.position = tPos;

        // Activate cursor game object
        cursorGO.SetActive(true);
    }

    // Dectivates cursor game object & resets its parent game object
    public void Deactivate() {
        // Set the cursor's parent game object back to Main Camera
        cursorGO.transform.SetParent(CameraManager.S.transform);

        // Deactivate cursor game object
        cursorGO.SetActive(false);
    }
}