using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSFXAudioClipName { buttonPressedSFX, itemTriggerSFX, springboardSFX, doorOpenSFX, dialogueSFX, dialogueEndSFX, pauseAudioSource, unpauseSFX };
public enum eBGMAudioClipName { levelSelect, level1, level2, level3, win };

public class AudioManager : MonoBehaviour {
    [Header("Set in Inspector")]
    // BGM audio source
    public AudioSource bgmAudioSource;
    public AudioSource levelSelectAudioSource;
    public AudioSource level1AudioSource;
    public AudioSource level2AudioSource;
    public AudioSource level3AudioSource;
    public AudioSource winAudioSource;

    // SFX audio sources
    public AudioSource buttonPressedAudioSource;
    public AudioSource dialogueAudioSource;
    public AudioSource dialogueEndAudioSource;
    public AudioSource doorOpenAudioSource;
    public AudioSource pauseAudioSource;
    public AudioSource itemTriggerAudioSource;
    public AudioSource springboardAudioSource;
    public AudioSource unpauseAudioSource;

    // BGM audio clip
    public AudioClip bgm;

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
        // Stop playing all BGM audio sources
        levelSelectAudioSource.Stop();
        level1AudioSource.Stop();
        level2AudioSource.Stop();
        level3AudioSource.Stop();
        winAudioSource.Stop();

        // Play specified BGM audio source
        switch (clipName) {
            case eBGMAudioClipName.levelSelect:
                levelSelectAudioSource.Play();
                break;
            case eBGMAudioClipName.level1:
                level1AudioSource.Play();
                break;
            case eBGMAudioClipName.level2:
                level2AudioSource.Play();
                break;
            case eBGMAudioClipName.level3:
                level3AudioSource.Play();
                break;
            case eBGMAudioClipName.win:
                winAudioSource.Play();
                break;
        }
    }
}