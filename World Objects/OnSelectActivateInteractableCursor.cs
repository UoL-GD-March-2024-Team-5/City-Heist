using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Handles activating and positioning the interactable cursor above the position of the target transform.
public class OnSelectActivateInteractableCursor : MonoBehaviour, ISelectHandler {
    [Header("Set in Inspector")]
    public Transform    targetGameObjectTransform;
    public float        yPosOffset = 0.25f;

    public void OnSelect(BaseEventData eventData) {
        // Activate Interactable Trigger above target transform position
        InteractableCursor.S.Activate(targetGameObjectTransform.position, yPosOffset);
    }
}