using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public string       firstScene;

    public GameObject   gameplayUIGameObject;

    public GameObject   floatingScoreGO;

    [Header("Set Dynamically")]
    // Pause menu
    public bool         paused;
    public GameObject   pauseMenu;

    // Stores all vision cones in current level; its contents are deactivated when player is hiding or lights are turned off
    public GameObject[] visionCones;

    // References to components handling separate parts of the app
    public LevelSelectManager   levelSelectManagerCS;
    public LevelEndManager      levelEndManagerCS;
    public PauseManager         pauseManagerCS;
    public HintPopUpManager     hintPopUpManagerCS;

    // Prevents activating NPC vision cones when moving betweeen two adjacent darkened rooms
    public int countOfRoomDarknessTriggersCurrentlyOccupiedByPlayer = 0;

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
        // Get components
        levelSelectManagerCS = GetComponent<LevelSelectManager>();
        levelEndManagerCS = GetComponent<LevelEndManager>();
        pauseManagerCS = GetComponent<PauseManager>();
        hintPopUpManagerCS = GetComponent<HintPopUpManager>();
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //
    //
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

    // Returns name of active scene
    public string GetActiveSceneName() {
        return SceneManager.GetActiveScene().name;
    }

    // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% //

    // Get and store all vision cones in current scene in an array;
    // its contents are deactivated when player is hiding or lights are turned off
    void GetVisionConesInScene() {
        visionCones = GameObject.FindGameObjectsWithTag("VisionCone");
    }

    // (De)Activate all vision cones in current scene
    public void ActivateVisionCones(bool activate = true) {
        for (int i = 0; i < visionCones.Length; i++) {
            visionCones[i].SetActive(activate);
        }
    }
}