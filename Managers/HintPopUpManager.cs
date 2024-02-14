using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// On player trigger enter displays context sensitive tips to guide the player
public class HintPopUpManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject   hintPopUpGO;

    public Text         hintPopUpText;

    public void ActivateAndSetText(string message) {
        // Add to count of active 'hint pop up' triggers
        GameManager.S.countOfHintPopUpTriggersCurrentlyOccupiedByPlayer += 1;

        // Activate 'hint pop up' game object
        hintPopUpGO.SetActive(true);

        // Set text
        hintPopUpText.text = message;
    }

    public void Deactivate() {
        // Decrement from count of active 'hint pop up' triggers
        GameManager.S.countOfHintPopUpTriggersCurrentlyOccupiedByPlayer -= 1;

        // Deactivate 'hint pop up' game object
        if (GameManager.S.countOfHintPopUpTriggersCurrentlyOccupiedByPlayer < 1) {
            hintPopUpGO.SetActive(false);
        }
    }
}