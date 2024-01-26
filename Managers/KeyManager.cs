using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Stores and displays the number of keys has collected and not used yet
public class KeyManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public Text keyCountText;

    [Header("Set Dynamically")]
    public int keyCount = 0;

    // Singleton
    private static KeyManager _S;
    public static KeyManager S { get { return _S; } set { _S = value; } }

    void Awake() {
        // Singleton
        S = this;
    }

    // Increment or decrement the player's key count 
    public void IncrementKeyCount(int amount) {
        // Increment or decrement 'keyCount' by 'amount'
        keyCount += amount;

        // Update UI
        UpdateKeyCountUI();
    }

    // Update the UI to reflect the current value of 'keyCount'
    void UpdateKeyCountUI() {
        keyCountText.text = "x" + keyCount.ToString();
    }

    public void ResetKeyCount() {
        keyCount = 0;

        // Update UI
        UpdateKeyCountUI();
    }
}