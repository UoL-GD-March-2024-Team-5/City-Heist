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

    [Header("Set Dynamically")]
    // Tracks the amount of items the player has stolen per level
    public List<float>  itemsStolenPerLevel = new List<float>(new float[] { 0, 0, 0 });

    // Stores the amount of total stealable items per level
    public List<float>  totalStealableItemsPerLevel = new List<float>(new float[] { 4, 4, 4 });

    // Stores the fastest time (in seconds) that the player has ever completed each level,
    // and the most amount of items they've ever stolen per level
    public List<float>  bestTimeLevelCompleted = new List<float>(new float[] { 0, 0, 0 });
    public List<float>  bestAmountOfItemsStolenPerLevel = new List<float>(new float[] { 0, 0, 0 });

    private void Start() {
        // Add button listeners
        goToNextLevelButton.onClick.AddListener(delegate { LoadNextLevel(); });
        tryAgainButton.onClick.AddListener(delegate { LoadCurrentLevel(); });
        goBackToLevelSelectButton.onClick.AddListener(delegate { GameManager.S.levelSelectManagerCS.GoBackToLevelSelectButtonPressed(); });
    }
    
    void LoadNextLevel() {
        StopAllCoroutines();

        GameManager.S.levelSelectManagerCS.LoadLevel(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx + 1);
    }

    void LoadCurrentLevel() {
        StopAllCoroutines();
        
        GameManager.S.levelSelectManagerCS.LoadLevel(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx);
    }

    // On both "level completed" and "game over", helps set up for level end
    void LevelEndHelper() {
        // Freeze player/pause game
        GameManager.S.pauseManagerCS.PauseGame(false);

        // Activate level end menu
        levelEndMenuGO.SetActive(true);

        // Deactivate interactable cursor
        InteractableCursor.S.Deactivate();

        // Deactivate hint pop up
        GameManager.S.hintPopUpManagerCS.Deactivate();
    }

    // Helps set up specific "level completed" settings for level end
    public void LevelCompleted() {
        LevelEndHelper();

        // Activate next level button if there's a next level
        if(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx < 2) {
            goToNextLevelButtonGO.SetActive(true);

            // Set selected game object
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(goToNextLevelButtonGO);

            // Set button navigation
            Utilities.S.SetButtonNavigation(tryAgainButton, goToNextLevelButton, goBackToLevelSelectButton);
            Utilities.S.SetButtonNavigation(goBackToLevelSelectButton, tryAgainButton, goToNextLevelButton);
        } else {
            goToNextLevelButtonGO.SetActive(false);

            // Set selected game object
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(tryAgainButton.gameObject);
            
            // Set button navigation
            Utilities.S.SetButtonNavigation(tryAgainButton, goBackToLevelSelectButton, goBackToLevelSelectButton);
            Utilities.S.SetButtonNavigation(goBackToLevelSelectButton, tryAgainButton, tryAgainButton);
        }

        // Display text
        menuHeaderText.text = "Level Completed!";
        menuSubHeaderText.text = (int)(GetPercentageOfItemsStolen(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx) * 100) + "% of total items stolen" + " of total items stolen\nwith " + Timer.S.GetTime() + " minutes to spare!";

        // Play short celebratory jingle, then resume playing previous played BGM
        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(4));

        // Check for and set new best time level completed
        CheckForAndSetNewBestTimeCompleted(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx);

        // Check for and set new best rank level completed
        CheckForAndSetNewBestRankCompleted(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx);
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
        menuSubHeaderText.text = (int)(GetPercentageOfItemsStolen(GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx) * 100) + "% of total items stolen" + " of total items stolen...\n " + "...but you've been caught!";

        // Play short dreary jingle, then resume playing previous played BGM
        StartCoroutine(AudioManager.S.PlayShortJingleThenResumePreviousBGM(5));
    }

    // On level completed, if this 'time completed' is better than saved 'best time completed', replace it
    void CheckForAndSetNewBestTimeCompleted(int levelNdx) {
        if (Timer.S.seconds > bestTimeLevelCompleted[levelNdx]) {
            GameManager.S.levelSelectManagerCS.bestLevelCompletedTime[levelNdx].text = "Best time: " + Timer.S.GetTime();
        }
    }

    // On level completed, if this 'rank' is better than saved 'best rank', replace it
    void CheckForAndSetNewBestRankCompleted(int levelNdx) {
        if (itemsStolenPerLevel[levelNdx] > bestAmountOfItemsStolenPerLevel[levelNdx]) {
            GameManager.S.levelSelectManagerCS.bestLevelCompletedRank[levelNdx].text = "Best rank: " + GetRankName(levelNdx) + "\n" + (int)(GetPercentageOfItemsStolen(levelNdx)*100) + "% of total items stolen"; ;
        }
    }

    // Increment count of items stolen from level that was just completed
    public void IncrementAmountStolen() {
        itemsStolenPerLevel[GameManager.S.levelSelectManagerCS.selectedLevelButtonNdx] += 1;
    }

    // Get the percentage of total items stolen from level that was just completed
    float GetPercentageOfItemsStolen(int levelNdx) {
        float percentage = itemsStolenPerLevel[levelNdx] / totalStealableItemsPerLevel[levelNdx];
        return percentage;
    }

    // Get cheeky rank name based off percentage of items stolen from level that was just completed
    string GetRankName(int levelNdx) {
        float percentageOfItemsStolen = GetPercentageOfItemsStolen(levelNdx);

        string rankName = "";
        if (percentageOfItemsStolen <= 0.10f) {
            rankName = "Fumbling Footpad";
        } else if (percentageOfItemsStolen > 0.11f && percentageOfItemsStolen <= 0.20f) {
            rankName = "Lazy Looter";
        } else if (percentageOfItemsStolen > 0.21f && percentageOfItemsStolen <= 0.30f) {
            rankName = "Cursory Crook";
        } else if (percentageOfItemsStolen > 0.31f && percentageOfItemsStolen <= 0.40f) {
            rankName = "Below Average Burglar";
        } else if (percentageOfItemsStolen > 0.41f && percentageOfItemsStolen <= 0.50f) {
            rankName = "Middling Mugger";
        } else if (percentageOfItemsStolen > 0.51f && percentageOfItemsStolen <= 0.60f) {
            rankName = "Burgeoning Bandit";
        } else if (percentageOfItemsStolen > 0.61f && percentageOfItemsStolen <= 0.70f) {
            rankName = "Pretty Good Pillager";
        } else if (percentageOfItemsStolen > 0.71f && percentageOfItemsStolen <= 0.80f) {
            rankName = "Seasoned Swindler";
        } else if (percentageOfItemsStolen > 0.81f && percentageOfItemsStolen <= 0.90f) {
            rankName = "Proud Pilferer";
        } else if (percentageOfItemsStolen > 0.91f && percentageOfItemsStolen <= 1.0f) {
            rankName = "Master Thief";
        }
        return rankName;
    }
}