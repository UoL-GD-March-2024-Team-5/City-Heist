using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public string       firstScene;

    public GameObject   gamplayUIGameObject;
    public GameObject   levelSelectionUIGameObject;

    public Button       goBackToLevelSelectButton;

    public GameObject   floatingScoreGO;

    public List<Button> levelButtons;

    [Header("Set Dynamically")]
    // Pause
    public bool         paused;
    public GameObject   pauseMenu;

    // Singleton
    private static GameManager _S;
    public static GameManager S { get { return _S; } set { _S = value; } }

    // DontDestroyOnLoad
    public static bool exists;

    void Awake(){
        // Singleton
        S = this;

        // DontDestroyOnLoad
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        // Load Scene
        SceneManager.LoadScene(firstScene);
    }

    void Start() {
        // Add Loop() to UpdateManager
        UpdateManager.updateDelegate += Loop;

        // Add go back to level select button listener
        goBackToLevelSelectButton.onClick.AddListener(delegate { GoBackToLevelSelectButtonPressed(); });

        // Add level select button listeners
        levelButtons[0].onClick.AddListener(delegate { LevelButtonPressed(0); });
        levelButtons[1].onClick.AddListener(delegate { LevelButtonPressed(1); });
        levelButtons[2].onClick.AddListener(delegate { LevelButtonPressed(2); });

        InitializeLevelSelectionScene();
    }

    // Helps sets up the level selection scene on initial load, & subsequent ones (ex. going back from pause menu)
    public void InitializeLevelSelectionScene() {
        // Activate level selection UI 
        levelSelectionUIGameObject.SetActive(true);

        // Deactivate gameplay UI object($, time, etc.)
        gamplayUIGameObject.SetActive(false);

        // Deactivate player game object
        PlayerController.S.gameObject.SetActive(false);

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);
    }

    public void Loop() {
        // Pause Input
        if (!paused) {
            if (Input.GetButtonDown("Pause")) {
                PauseGame();
            }
        } else {
            if (Input.GetButtonDown("Pause") || Input.GetButtonDown("SNES B Button")) {
                UnpauseGame();
            }
        }
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //
    // On button press, sets up for and loads a level scene
    public void LevelButtonPressed(int levelNdx) {
        // Disable button interactivity
        SetButtonsInteractable(false);

        // Close curtains
        LevelLoadTransition.S.Close();

        // Deactivate interactable cursor
        InteractableCursor.S.Deactivate();

        // Reset player game object position
        PlayerController.S.transform.position = Vector3.zero;

        // Switch cam mode to follow player game object
        CameraManager.S.camMode = eCamMode.followAll;

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.buttonPressedSFX);

        // Wait, then load level
        StartCoroutine(LoadLevel(levelNdx));
    }

    public IEnumerator LoadLevel(int levelNdx) {
        // Wait
        yield return new WaitForSecondsRealtime(1f);

        // Open curtains
        LevelLoadTransition.S.Open();

        // Load Scene
        SceneManager.LoadScene("Level_" + (levelNdx + 1).ToString());

        // Activate player game object
        PlayerController.S.gameObject.SetActive(true);

        // Deactivate level selection UI 
        levelSelectionUIGameObject.SetActive(false);

        // Activate gameplay UI object ($, time, etc.)
        gamplayUIGameObject.SetActive(true);

        // Start timer
        Timer.S.StartTimer();

        // Reset score/total value of stolen items
        ScoreManager.S.ResetScore();

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
        SetButtonsInteractable(true);
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //
    void PauseGame() {
        // Pause timer
        Timer.S.PauseTimer();

        paused = true;
        Physics2D.gravity = Vector2.zero;

        // Activate pause menu
        pauseMenu.SetActive(true);

        // Set player velocity to 0
        PlayerController.S.rigid.velocity = Vector2.zero;

        // Set selected game object
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(goBackToLevelSelectButton.gameObject);

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.pauseAudioSource);
    }

    void UnpauseGame(bool unpauseTimer = true) {
        // Unpause timer
        if (unpauseTimer) {
            Timer.S.UnpauseTimer();
        }

        paused = false;
        Physics2D.gravity = new Vector2(0, -15f);

        // Deactivate Interactable Trigger
        InteractableCursor.S.Deactivate();

        // Deactivate pause menu
        pauseMenu.SetActive(false);

        // Play SFX
        AudioManager.S.PlaySFX(eSFXAudioClipName.unpauseSFX);
    }

    public void GoBackToLevelSelectButtonPressed() {
        // Disable button interactivity
        SetButtonsInteractable(false);

        // Close curtains
        LevelLoadTransition.S.Close();

        // Deactivate Interactable Trigger
        InteractableCursor.S.Deactivate();

        // Reset camera position and mode
        CameraManager.S.camMode = eCamMode.freezeCam;
        CameraManager.S.transform.position = new Vector3(0, 0, -10);

        // Wait, then go back to level select
        StartCoroutine(GoBackToLevelSelect());
    }

    public IEnumerator GoBackToLevelSelect() {
        // Wait
        yield return new WaitForSecondsRealtime(1f);

        // Open curtains
        LevelLoadTransition.S.Open();

        // Load Scene
        SceneManager.LoadScene("Level_Selection");

        // Play BGM
        AudioManager.S.PlayBGM(eBGMAudioClipName.levelSelect);

        // Unpause game without unpausing the timer
        UnpauseGame(false);

        InitializeLevelSelectionScene();

        // Enable button interactivity
        SetButtonsInteractable(true);
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //
    public void InstantiateFloatingScore(GameObject targetGO, string message, Color color, float yPosOffset = 0) {
        // Instantiate floating Score game object, & set its position to that of the target game object
        GameObject floatingScore = Instantiate(floatingScoreGO, targetGO.transform.position, Quaternion.identity);

        // Add y-pos offset to floating score position
        Vector2 tPos = floatingScore.transform.position;
        tPos.y += yPosOffset;
        floatingScore.transform.position = tPos;

        // Display and color floating score text
        if (floatingScore != null) {
            // Get text components (one for colored text in center, four for the black outline)
            Text[] texts = floatingScore.GetComponentsInChildren<Text>();
            for (int i = 0; i < texts.Length; i++) {
                // Display text
                texts[i].text = message;
                // Set color of text in center
                if (i == texts.Length - 1) {
                    if (message != "0") {
                        texts[i].color = color;
                    } else {
                        texts[i].color = Color.white;
                    }
                }
            }
        }
    }

    // Used to prevent the user from clicking buttons additional times after they've already been pressed
    void SetButtonsInteractable(bool isInteractable) {
        levelButtons[0].interactable = isInteractable;
        levelButtons[1].interactable = isInteractable;
        levelButtons[2].interactable = isInteractable;
        goBackToLevelSelectButton.interactable = isInteractable;
    }
}