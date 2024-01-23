using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public string       firstScene;

    public GameObject   timerGO;

    public Button       goBackToLevelSelectButton;

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
        goBackToLevelSelectButton.onClick.AddListener(delegate { GoBackToLevelSelectMenu(); });

        // Deactivate timer game object
        timerGO.SetActive(false);
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
    }

    void UnpauseGame(bool unpauseTimer = true) {
        // Unpause timer
        if (unpauseTimer) {
            Timer.S.UnpauseTimer();
        }

        paused = false;
        Physics2D.gravity = new Vector2(0, -15f);

        // Deactivate pause menu
        pauseMenu.SetActive(false);
    }

    public void GoBackToLevelSelectMenu() {
        // Deactivate timer game object
        timerGO.SetActive(false);

        // Load Scene
        SceneManager.LoadScene("Level_Selection");

        // Unpause game without unpausing the timer
        UnpauseGame(false);
    }
}