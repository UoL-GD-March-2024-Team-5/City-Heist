using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject   levelSelectionUIGameObject;

    // On press loads a level to play
    public List<Button> levelButtons;

    // Displays the fastest time that the player has completed each level
    public List<Text>   bestLevelCompletedTime;

    // Displays the best rank achieved by the player for each completed level
    public List<Text>   bestLevelCompletedRank;

    [Header("Set Dynamically")]
    // Stores the last selected level select button.
    // On reload level select scene, sets this button to be currently selected game object.
    public int          selectedLevelButtonNdx = 0;

    void Start() {
        // Add level select button listeners
        levelButtons[0].onClick.AddListener(delegate { LoadLevel(0); });
        levelButtons[1].onClick.AddListener(delegate { LoadLevel(1); });
        levelButtons[2].onClick.AddListener(delegate { LoadLevel(2); });

        InitializeLevelSelectionScene();
    }

    public void InitializeLevelSelectionScene() {
        // Activate level selection UI 
        levelSelectionUIGameObject.SetActive(true);

        // Deactivate gameplay UI object($, time, etc.)
        GameManager.S.gameplayUIGameObject.SetActive(false);

        // Deactivate player game object
        PlayerController.S.gameObject.SetActive(false);

        // Prevent dynamically selected button from playing 'Selection' SFX
        OnSelectPlaySound onSelectPlaySound = levelButtons[0].GetComponent<OnSelectPlaySound>();
        if (onSelectPlaySound) {
            onSelectPlaySound.playSFXOnFirstTimeSelected = false;
        }

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(levelButtons[selectedLevelButtonNdx].gameObject);
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //
    // Sets up for and loads a level scene
    public void LoadLevel(int levelNdx) {
        // Disable button interactivity
        Utilities.S.ButtonsInteractable(levelButtons, false);

        // Close curtains
        LevelLoadTransition.S.Close();

        // Deactivate interactable cursor
        InteractableCursor.S.Deactivate();

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.buttonPressedSFX);

        // Cache button index
        selectedLevelButtonNdx = levelNdx;

        // Wait, then load level
        StartCoroutine(WaitForLoadLevel(levelNdx));
    }

    //
    public IEnumerator WaitForLoadLevel(int levelNdx) {
        // Wait
        yield return new WaitForSecondsRealtime(0.75f);

        // Reset player game object position
        PlayerController.S.transform.position = Vector3.zero;

        // Switch cam mode to follow player game object
        CameraManager.S.camMode = eCamMode.followAll;

        // Wait
        yield return new WaitForSecondsRealtime(0.25f);

        // Open curtains
        LevelLoadTransition.S.Open();

        // Load Scene
        SceneManager.LoadScene("Level_" + (levelNdx + 1).ToString());

        // Unpause game without unpausing the timer or playing any SFX
        GameManager.S.pauseManagerCS.UnpauseGame(false, false);

        // Ensure player game object is visible and can move
        PlayerController.S.StopHiding();

        // Activate player game object
        PlayerController.S.gameObject.SetActive(true);

        // Deactivate level selection UI 
        levelSelectionUIGameObject.SetActive(false);

        // Deactivate level end UI
        GameManager.S.levelEndManagerCS.levelEndMenuGO.SetActive(false);

        // Activate gameplay UI object ($, time, etc.)
        GameManager.S.gameplayUIGameObject.SetActive(true);

        // Start timer
        Timer.S.StartTimer();

        // Reset score/total value of stolen items
        ScoreManager.S.ResetScore();

        // Reset key count
        KeyManager.S.ResetKeyCount();

        // Reset amount of items stolen 
        GameManager.S.levelEndManagerCS.itemsStolenPerLevel[levelNdx] = 0;

        // Get and store all vision cones in newly loaded scene
        GameManager.S.Invoke("GetVisionConesInScene", 0.25f);

        // Display text & play BGM
        List<string> startMessage = new List<string>();
        switch (levelNdx) {
            case 0:
                startMessage = new List<string>() { "Hey, press the E key or space bar on your keyboard to move to the next batch of dialogue.", "You can also press E to open doors...", "...space bar to jump...", "...P or Esc to pause...", "...and hold left shift while moving to run.", "Got it?", "Well, you better!" };
                AudioManager.S.PlayBGM(eBGMAudioClipName.level1);
                break;
            case 1:
                startMessage = new List<string>() { "Hey, this is level 2.", "Not much going on here...", "...so maybe pause the game (press P or Esc) and click the 'go back' button to get outta here." };
                AudioManager.S.PlayBGM(eBGMAudioClipName.level2);
                break;
            case 2:
                startMessage = new List<string>() { "Wow, you've started level 3!", "Still not much going on here...", "...so maybe pause the game (press P or Esc) and click the 'go back' button to get outta here." };
                AudioManager.S.PlayBGM(eBGMAudioClipName.level3);
                break;
        }
        DialogueManager.S.DisplayText(startMessage);

        // Enable button interactivity
        Utilities.S.ButtonsInteractable(levelButtons, true);
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //

    public void GoBackToLevelSelectButtonPressed() {
        // Disable button interactivity
        GameManager.S.pauseManagerCS.goBackToLevelSelectButton.interactable = false;

        // Close curtains
        LevelLoadTransition.S.Close();

        // Deactivate Interactable Trigger
        InteractableCursor.S.Deactivate();

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.buttonPressedSFX);

        // Wait, then go back to level select
        StartCoroutine(GoBackToLevelSelect());
    }

    public IEnumerator GoBackToLevelSelect() {
        // Wait
        yield return new WaitForSecondsRealtime(1f);

        // Reset camera position and mode
        CameraManager.S.camMode = eCamMode.freezeCam;
        CameraManager.S.SetCamPosition(Vector2.zero);

        // Open curtains
        LevelLoadTransition.S.Open();

        // Load Scene
        SceneManager.LoadScene("Level_Selection");

        // Play BGM
        AudioManager.S.PlayBGM(eBGMAudioClipName.levelSelect);

        // Unpause game without unpausing the timer or playing any SFX
        GameManager.S.pauseManagerCS.UnpauseGame(false, false);

        GameManager.S.levelSelectManagerCS.InitializeLevelSelectionScene();

        // Enable button interactivity
        GameManager.S.pauseManagerCS.goBackToLevelSelectButton.interactable = true;
    }
}