using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	TimeManager timeManager;
	BodyManager bodyManager;
	// Use this for initialization
	void Start () {
		timeManager = GameObject.FindObjectOfType<TimeManager> ();
		bodyManager = GameObject.FindObjectOfType<BodyManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		float LST = Input.GetAxis ("LeftShoulderTrigger");
		float RST = Input.GetAxis ("RightShoulderTrigger");
		if (LST > 0) {
			Debug.Log ("LST: " + LST);
			timeManager.SlowTime (LST);
		}
		if (RST > 0) {
			Debug.Log ("RST: " + RST);
			timeManager.SpeedUpTime (RST);
		}
		if (Input.GetButtonDown ("Fire1")) {
			timeManager.TogglePauseTime ();
			Debug.Log ("Fire1"); 
		}
		if (Input.GetButtonDown ("Fire2")) {
			Debug.Log ("Fire2"); 
			bodyManager.ToggleView ();
		}
	
	}
}
