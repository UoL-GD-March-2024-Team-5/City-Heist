using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A set of general helper functions to be used across the entire project.
public class Utilities : MonoBehaviour {
    [Header("Set Dynamically")]
    private static Utilities _S;
    public static Utilities S { get { return _S; } set { _S = value; } }

    void Awake() {
        S = this;
    }

    // Explicitly set a button's navigation
    public void SetButtonNavigation(Button button, Button buttonSelectOnUp = null, Button buttonSelectOnDown = null, Button buttonSelectOnLeft = null, Button buttonSelectOnRight = null) {
        // Get the Navigation data
        Navigation navigation = button.navigation;

        // Switch mode to Explicit to allow for custom assigned behavior
        navigation.mode = Navigation.Mode.Explicit;

        // Select buttons to navigate to on directional input
        navigation.selectOnUp = buttonSelectOnUp;
        navigation.selectOnDown = buttonSelectOnDown;
        navigation.selectOnLeft = buttonSelectOnLeft;
        navigation.selectOnRight = buttonSelectOnRight;

        // Reassign the struct data to the button
        button.navigation = navigation;
    }

    // Make a list of buttons interactable
    public void ButtonsInteractable(List<Button> buttons, bool isInteractable) {
        for (int i = 0; i <= buttons.Count - 1; i++) {
            buttons[i].interactable = isInteractable;
        }
    }
}