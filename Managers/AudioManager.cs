using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAudioClipName { buttonPressedSFX, pickUpSFX, springboardSFX, doorOpenSFX, dialogueSFX, dialogueEndSFX, unpauseSFX };

public class AudioManager : MonoBehaviour {
    [Header("Set in Inspector")]
    // BGM audio source
    public AudioSource bgmAudioSource;

    // SFX audio sources
    public AudioSource buttonPressedAudioSource;
    public AudioSource dialogueAudioSource;
    public AudioSource dialogueEndAudioSource;
    public AudioSource doorOpenAudioSource;
    public AudioSource pickUpAudioSource;
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
        //bgmAudioSource.clip = bgm;
        //bgmAudioSource.Play();
    }

    // Plays a specific SFX audio source based on its input, an enum called 'eAudioClipName'
    public void PlaySFX(eAudioClipName clipName) {
        switch (clipName) {
            case eAudioClipName.buttonPressedSFX:
                buttonPressedAudioSource.Play();
                break;
            case eAudioClipName.dialogueSFX:
                dialogueAudioSource.Play();
                break;
            case eAudioClipName.dialogueEndSFX:
                dialogueEndAudioSource.Play();
                break;
            case eAudioClipName.doorOpenSFX:
                doorOpenAudioSource.Play();
                break;
            case eAudioClipName.pickUpSFX:
                pickUpAudioSource.Play();
                break;
            case eAudioClipName.springboardSFX:
                springboardAudioSource.Play();
                break;
            case eAudioClipName.unpauseSFX:
                unpauseAudioSource.Play();
                break;
        }
    }
}