using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// On level start, initiates and displays a timer that counts down from 5:00 minutes, one second at a time
public class Timer : MonoBehaviour {
    [Header("Set in Inspector")]
    public Text     timerTextValue;

    [Header("Set Dynamically")]
    // Amount of time until the police arrive 
    public int      seconds = 0;
    public int      minutes = 5;

    // Time at which time is up and the police have arrived
    public float    timeDone;

    // Singleton
    private static Timer _S;
    public static Timer S { get { return _S; } set { _S = value; } }

    void Awake() {
        // Singleton
        S = this;
    }

    // Starts the timer counting down from 5:00 minutes 
    public void StartTimer() {
        seconds = 0;
        minutes = 5;
        timeDone = Time.time + 300;

        StartCoroutine("DecrementTimeCoroutine");
    }

    // Returns the current time as a string in '0:00' format
    public string GetTime() {
        string zeroString = "";

        if (seconds < 10) {
            zeroString = "0";
        }

        return minutes.ToString() + ":" + zeroString + seconds.ToString();
    }

    //
    public void PauseTimer() {
        StopAllCoroutines();
    }

    public void UnpauseTimer() {
        StartCoroutine("DecrementTimeCoroutine");
    }

	// Decrement time and update UI
	public IEnumerator DecrementTimeCoroutine() {
		// Decrement seconds
		if (Time.time <= timeDone) {
			seconds -= 1;
        } else {
            //Debug.Log("Time is up, game over!");
            DialogueManager.S.DisplayText("Time is up, game over!");
            //StopCoroutine("DecrementTimeCoroutine");
            StopAllCoroutines();
        }

		// Decrement minutes & reset seconds
		if (seconds < 0) {
			minutes -= 1;
			seconds = 59;
		}

        // Display time to user
        timerTextValue.text = GetTime();

        // Loop this coroutine
        //yield return new WaitForFixedUpdate();
        yield return new WaitForSecondsRealtime(1);
		StartCoroutine("DecrementTimeCoroutine");
	}
}