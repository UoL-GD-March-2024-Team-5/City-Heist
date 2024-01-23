using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public string       firstScene;

    public GameObject   gamplayUIGameObject;

    public Button       goBackToLevelSelectButton;

    public GameObject   floatingScoreGO;

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

        // Deactivate gameplay UI object($, time, etc.)
        gamplayUIGameObject.SetActive(false);
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
        // Deactivate gameplay UI object($, time, etc.)
        gamplayUIGameObject.SetActive(false);

        // Load Scene
        SceneManager.LoadScene("Level_Selection");

        // Unpause game without unpausing the timer
        UnpauseGame(false);
    }

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
}