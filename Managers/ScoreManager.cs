using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Stores and displays the total cash value of items they've stolen/picked up.
public class ScoreManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public Text cashValueText;

    [Header("Set Dynamically")]
    int cashValue = 0;

    // Singleton
    private static ScoreManager _S;
    public static ScoreManager S { get { return _S; } set { _S = value; } }

    void Awake() {
        // Singleton
        S = this;
    }

    // Increment the player's score by the value of the newly stolen item
    public void IncrementScore(int itemValue) {
        // Increment total cash by stolen item's value
        cashValue += itemValue;

        // Update UI
        UpdateScoreUI();
    }

    // Update the UI to reflect the current value of 'cashValue'
    void UpdateScoreUI() {
        cashValueText.text = "$" + cashValue.ToString() + ".00";
    }

    public void ResetScore() {
        cashValue = 0;

        // Update UI
        UpdateScoreUI();
    }
}