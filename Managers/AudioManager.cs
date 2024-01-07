using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [Header("Set in Inspector")]
    // Audio sources
    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;

    // Audio clips
    public AudioClip bgm;
    public AudioClip pickUpSFX;
    public AudioClip springboardSFX;

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
        bgmAudioSource.clip = bgm;
        bgmAudioSource.Play();
    }

    public void PlayPickUpSFX() {
        sfxAudioSource.clip = pickUpSFX;
        sfxAudioSource.Play();
    }

    public void PlaySpringboardSFX() {
        sfxAudioSource.clip = springboardSFX;
        sfxAudioSource.Play();
    }
}