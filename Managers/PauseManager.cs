using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles pausing and unpausing the game.
public class PauseManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject pauseMenuGO;

    void Start() {
        // Add Loop() to UpdateManager
        UpdateManager.updateDelegate += Loop;
    }

    public void Loop() {
        // Pause Input
        if(GameManager.S.GetActiveSceneName() != "Level_Selection") {
            if (!GameManager.S.paused) {
                if (Input.GetButtonDown("Pause")) {
                    PauseGame();
                }
            } else {
                if (Input.GetButtonDown("Pause") || Input.GetButtonDown("SNES B Button")) {
                    UnpauseGame();
                }
            }
        }
    }

    void PauseGame() {
        // Pause timer
        Timer.S.PauseTimer();

        GameManager.S.paused = true;

        // Set gravity to 0
        Physics2D.gravity = Vector2.zero;

        // Activate pause menu
        pauseMenuGO.SetActive(true);

        // Set player velocity to 0
        PlayerController.S.rigid.velocity = Vector2.zero;

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(GameManager.S.goBackToLevelSelectButton.gameObject);

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.pauseAudioSource);
    }

    public void UnpauseGame(bool unpauseTimer = true, bool playSFX = true) {
        // Unpause timer
        if (unpauseTimer) {
            Timer.S.UnpauseTimer();
        }

        GameManager.S.paused = false;

        // Reset gravity to its normal value
        Physics2D.gravity = new Vector2(0, -15f);

        // Deactivate Interactable Trigger
        InteractableCursor.S.Deactivate();

        // Deactivate pause menu
        pauseMenuGO.SetActive(false);

        // Play SFX
        if (playSFX) {
            AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
        }
    }
}
