using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSFXAudioClipName { buttonPressedSFX, itemTriggerSFX, springboardSFX, doorOpenSFX, dialogueSFX, dialogueEndSFX, pauseAudioSource, unpauseSFX };
public enum eBGMAudioClipName { levelSelect, level1, level2, level3, win };

public class AudioManager : MonoBehaviour {
    [Header("Set in Inspector")]
    // BGM audio sources
    public List<AudioSource> bgmCS = new List<AudioSource>();

    // SFX audio sources
    public AudioSource buttonPressedAudioSource;
    public AudioSource dialogueAudioSource;
    public AudioSource dialogueEndAudioSource;
    public AudioSource doorOpenAudioSource;
    public AudioSource pauseAudioSource;
    public AudioSource itemTriggerAudioSource;
    public AudioSource springboardAudioSource;
    public AudioSource unpauseAudioSource;

    [Header("Set Dynamically")]
    public int currentSongNdx;

    // Singleton
    private static AudioManager _S;
    public static AudioManager S { get { return _S; } set { _S = value; } }

    private static bool exists;

    void Awake() {
        // Singleton
        S = this;

        // DontDestroyOnLoad
        if (!exists) {
            exists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Start playing background music
        PlayBGM(eBGMAudioClipName.levelSelect);
    }

    // Plays a specific SFX audio source based on its input, an enum called 'eSFXAudioClipName'
    public void PlaySFX(eSFXAudioClipName clipName) {
        switch (clipName) {
            case eSFXAudioClipName.buttonPressedSFX:
                buttonPressedAudioSource.Play();
                break;
            case eSFXAudioClipName.dialogueSFX:
                dialogueAudioSource.Play();
                break;
            case eSFXAudioClipName.dialogueEndSFX:
                dialogueEndAudioSource.Play();
                break;
            case eSFXAudioClipName.doorOpenSFX:
                doorOpenAudioSource.Play();
                break;
            case eSFXAudioClipName.pauseAudioSource:
                pauseAudioSource.Play();
                break;
            case eSFXAudioClipName.itemTriggerSFX:
                itemTriggerAudioSource.Play();
                break;
            case eSFXAudioClipName.springboardSFX:
                springboardAudioSource.Play();
                break;
            case eSFXAudioClipName.unpauseSFX:
                unpauseAudioSource.Play();
                break;
        }
    }

    // Plays a specific BGM audio source based on its input, an enum called 'eBGMAudioClipName'
    public void PlayBGM(eBGMAudioClipName clipName) {
        // Set current song index
        currentSongNdx = (int)clipName;

        // Stop ALL BGM
        for (int i = 0; i < bgmCS.Count; i++) {
            bgmCS[i].Stop();
        }

        // Play song
        switch (clipName) {
            case eBGMAudioClipName.levelSelect: bgmCS[0].Play(); break;
            case eBGMAudioClipName.level1: bgmCS[1].Play(); break;
            case eBGMAudioClipName.level2: bgmCS[2].Play(); break;
            case eBGMAudioClipName.level3: bgmCS[3].Play(); break;
            case eBGMAudioClipName.win: bgmCS[4].Play(); break;
        }
    }
}