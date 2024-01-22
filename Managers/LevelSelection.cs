using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Acts as the game's main menu. Handles loading and setting up for gameplay scenes (Levels 1-3).
public class LevelSelection : MonoBehaviour {
    [Header("Set in Inspector")]
    public List<Button> levelButtons;

    void Start() {
        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);

        // Add level select button listeners
        levelButtons[0].onClick.AddListener(delegate { LevelButtonPressed(0); });
        levelButtons[1].onClick.AddListener(delegate { LevelButtonPressed(1); });
        levelButtons[2].onClick.AddListener(delegate { LevelButtonPressed(2); });

        // Deactivate player game object
        PlayerController.S.gameObject.SetActive(false);
    }

    // On button press, sets up for and loads a level scene
    public void LevelButtonPressed(int ndx) {
        // Load Scene
        SceneManager.LoadScene("Level_" + (ndx+1).ToString());

        // Start timer
        Timer.S.StartTimer();

        // Reset player game object position
        PlayerController.S.transform.position = Vector3.zero;

        // Activate player game object
        PlayerController.S.gameObject.SetActive(true);

        // Switch cam mode to follow player game object
        CameraManager.S.camMode = eCamMode.followAll;
    }
}