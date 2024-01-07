using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Set in Inspector")]
    public string       firstScene;

    [Header("Set Dynamically")]
    // Pause
    public bool         paused;
    public GameObject   pauseBlackScreen;

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
    }

    public void Loop() {
        // Pause Input
        if (!paused) {        
            if (Input.GetButtonDown("Pause")) {
                paused = true;
                Physics2D.gravity = Vector2.zero;
                pauseBlackScreen.SetActive(true);

                Player.S.rigid.velocity = Vector2.zero;
            }
        } else {
            if (Input.GetButtonDown("Pause") || Input.GetButtonDown("SNES B Button")) {
                paused = false;
                Physics2D.gravity = new Vector2(0, -15f);
                pauseBlackScreen.SetActive(false);
            }         
        }
    }
}