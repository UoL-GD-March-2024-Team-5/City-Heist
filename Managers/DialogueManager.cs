using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles activating and displaying text messages to the user.
public class DialogueManager : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject   frameImage;
    public Text         dialogueText;
    public GameObject   cursorGO;

    [Header("Set Dynamically")]
    public bool         dialogueFinished = true;
    public int          dialogueNdx = 99;
    public List<string> message;

    // Singleton
    private static DialogueManager _S;
    public static DialogueManager S { get { return _S; } set { _S = value; } }

    void Awake() {
        // Singleton
        S = this;
    }

    private void Start() {
        frameImage.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        cursorGO.SetActive(false);
    }

    public void Update() {
        if (dialogueText.isActiveAndEnabled && dialogueFinished) {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
                if (dialogueNdx <= 0) {
                    DeactivateTextBox();
                } else if (dialogueNdx > 0) { // For Multiple Lines
                    if (message.Count > 0) {
                        List<string> tMessage;

                        tMessage = message;

                        tMessage.RemoveAt(0);

                        // Call DisplayText() with one less line of "messages"
                        DisplayText(tMessage);        
                    }
                }

                // Play SFX
                AudioManager.S.PlaySFX(eSFXAudioClipName.dialogueEndSFX);
            }
        }  
    }

    // Display a SINGLE string
    public void DisplayText(string messageToDisplay) {
        // Reset Dialogue
        dialogueFinished = true;
        dialogueNdx = 0;

        // Convert message string into a list of strings
        List<string> tMessage = new List<string> { messageToDisplay };
        DisplayText(tMessage);
    }

    // Display a LIST of strings
    public void DisplayText(List<string> text) {
        StopAllCoroutines();
        StartCoroutine(DisplayTextCo(text));
    }
    IEnumerator DisplayTextCo(List<string> text) {
        message = text;

        // Freeze Player
        PlayerController.S.canMove = false;

        // Set player velocity to 0
        PlayerController.S.rigid.velocity = Vector2.zero;

        // Deactivate Cursor
        cursorGO.SetActive(false);

        // Activate text and frame game objects
        frameImage.SetActive(true);
        dialogueText.gameObject.SetActive(true);

        // Get amount of Dialogue Strings
        dialogueNdx = text.Count;

        if (text.Count > 0) {
            dialogueFinished = false;

            string dialogueSentences = null;

            // Split text argument w/ blank space
            string[] dialogueWords = text[0].Split(' ');
            // Display text one word at a time
            for (int i = 0; i < dialogueWords.Length; i++) {
                // Play SFX
                AudioManager.S.PlaySFX(eSFXAudioClipName.dialogueSFX);

                dialogueSentences += dialogueWords[i] + " ";
                this.dialogueText.text = dialogueSentences;
                yield return new WaitForSeconds(0.05f);
            }

            // Activate cursor
            cursorGO.SetActive(true);

            dialogueNdx -= 1;

            dialogueFinished = true;
        }
    }

    // Set Text Instantly 
    // - No delay/stagger between displaying each word)
    public void SetText(string text, bool upperLeftAlignment = false, bool activateSubMenu = false) {
        StopCoroutine("DisplayTextCo");

        // Activate text, frame, & cursor game objects
        frameImage.SetActive(true);
        dialogueText.gameObject.SetActive(true);
        cursorGO.SetActive(true);

        dialogueText.text = text;
    }

    public void DeactivateTextBox(bool canMove = true) {
        dialogueNdx = 0;

        // Deactivate Text Box & Cursor
        frameImage.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        cursorGO.SetActive(false);

        // Reset Dialogue
        dialogueFinished = false;

        // Unfreeze Player
        PlayerController.S.canMove = true;

        // Reset gravity to its normal value
        Physics2D.gravity = new Vector2(0, -15f);
    }
}