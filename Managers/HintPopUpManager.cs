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
        hintPopUpGO.SetActive(true);

        hintPopUpText.text = message;
    }

    public void Deactivate() {
        hintPopUpGO.SetActive(false);
    }
}