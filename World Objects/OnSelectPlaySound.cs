using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// On this UI object selected, play sound
public class OnSelectPlaySound : MonoBehaviour, ISelectHandler {
    [Header("Set in Inspector")]
    public AudioSource  audioSource;

    // Prevents playing 'selected' SFX for buttons that are dynamically selected on level load
    public bool         playSFXOnFirstTimeSelected = true;

    public void OnSelect(BaseEventData eventData) {
        // Play SFX 
        if (playSFXOnFirstTimeSelected) {
            audioSource.Play();
        } else {
            playSFXOnFirstTimeSelected = true;
        }

        // Activate Interactable Trigger
        InteractableCursor.S.Activate(gameObject.transform.position);
    }
}