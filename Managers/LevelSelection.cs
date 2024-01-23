using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

// Acts as the game's main menu. Handles loading and setting up for gameplay scenes (Levels 1-3).
public class LevelSelection : MonoBehaviour {
    [Header("Set in Inspector")]
    public List<Button> levelButtons;

    // Singleton
    private static LevelSelection _S;
    public static LevelSelection S { get { return _S; } set { _S = value; } }

    void Awake() {
        // Singleton
        S = this;
    }

    void Start() {
        // Add level select button listeners
        levelButtons[0].onClick.AddListener(delegate { LevelButtonPressed(0); });
        levelButtons[1].onClick.AddListener(delegate { LevelButtonPressed(1); });
        levelButtons[2].onClick.AddListener(delegate { LevelButtonPressed(2); });

        // Deactivate player game object
        PlayerController.S.gameObject.SetActive(false);

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);
    }

    // On button press, sets up for and loads a level scene
    public void LevelButtonPressed(int ndx) {
        // Load Scene
        SceneManager.LoadScene("Level_" + (ndx+1).ToString());

        // Deactivate gameplay UI object ($, time, etc.)
        GameManager.S.gamplayUIGameObject.SetActive(true);

        // Start timer
        Timer.S.StartTimer();

        // Reset score/total value of stolen items
        ScoreManager.S.ResetScore();

        // Reset player game object position
        PlayerController.S.transform.position = Vector3.zero;

        // Activate player game object
        PlayerController.S.gameObject.SetActive(true);

        // Switch cam mode to follow player game object
        CameraManager.S.camMode = eCamMode.followAll;

        // Display text
        List<string> startMessage = new List<string>() { "Hey, press the L button on your keyboard to move to the next batch of dialogue.", "Is it working?", "Well, it better because I'm sick of working on this." };
        DialogueManager.S.DisplayText(startMessage);
    }
}