using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// On level end, activate appropriate text and buttons to proceed to next level or try again
public class LevelEndManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject   levelEndMenuGO;
    public GameObject   goToNextLevelButtonGO;

    public Text         menuHeaderText;
    public Text         menuSubHeaderText;

    public Button       goToNextLevelButton;
    public Button       tryAgainButton;
    public Button       goBackToLevelSelectButton;

    private void Start() {
        // Add button listeners
        goToNextLevelButton.onClick.AddListener(delegate { LoadNextLevel(); });
        tryAgainButton.onClick.AddListener(delegate { LoadCurrentLevel(); });
        goBackToLevelSelectButton.onClick.AddListener(delegate { GameManager.S.GoBackToLevelSelectButtonPressed(); });
    }
    
    void LoadNextLevel() {
        StopAllCoroutines();

        GameManager.S.LoadLevel(GameManager.S.selectedLevelButtonNdx + 1);
    }

    void LoadCurrentLevel() {
        StopAllCoroutines();
        
        GameManager.S.LoadLevel(GameManager.S.selectedLevelButtonNdx);
    }

    // On both "level completed" and "game over", helps set up for level end
    void LevelEndHelper() {
        // Freeze player/pause game
        GameManager.S.pauseManagerCS.PauseGame(false);

        // Activate level end menu
        levelEndMenuGO.SetActive(true);

        // Deactivate interactable cursor
        InteractableCursor.S.Deactivate();

        // Calculate score/rank
        
    }

    // Helps set up specific "level completed" settings for level end
    public void LevelCompleted() {
        //
        LevelEndHelper();

        // Unlock next level

        // Activate next level button if there's a next level
        if(GameManager.S.selectedLevelButtonNdx < 2) {
            goToNextLevelButtonGO.SetActive(true);
        } else {
            goToNextLevelButtonGO.SetActive(false);
        }

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(goToNextLevelButtonGO);

        // Set button navigation
        Utilities.S.SetButtonNavigation(tryAgainButton, goToNextLevelButton, goBackToLevelSelectButton);
        Utilities.S.SetButtonNavigation(goBackToLevelSelectButton, tryAgainButton, goToNextLevelButton);

        // Display text
        menuHeaderText.text = "Level Completed!";
        menuSubHeaderText.text = "You've stolen _ of _ total items!";

        // Play short celebratory jingle, then resume playing previous played BGM
        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(4));
    }

    // Helps set up specific "game over" settings for level end
    public void GameOver() {
        
        LevelEndHelper();

        // Deactivate next level button 
        goToNextLevelButtonGO.SetActive(false);

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(tryAgainButton.gameObject);

        // Set button navigation
        Utilities.S.SetButtonNavigation(tryAgainButton, goBackToLevelSelectButton, goBackToLevelSelectButton);
        Utilities.S.SetButtonNavigation(goBackToLevelSelectButton, tryAgainButton, tryAgainButton);

        // Display text
        menuHeaderText.text = "Game Over!";
        menuSubHeaderText.text = "You've been caught!";

        // Play short dreary jingle, then resume playing previous played BGM
        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(5));
    }
}