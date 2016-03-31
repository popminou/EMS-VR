using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float LST = Input.GetAxis ("LeftShoulderTrigger");
		float RST = Input.GetAxis ("RightShoulderTrigger");
		if (LST > 0)
			Debug.Log ("LST: " + LST);
		if(RST > 0)
			Debug.Log ("RST: " + RST);

		if (Input.GetButtonDown ("Fire1"))
			Debug.Log ("Fire1"); 
		if (Input.GetButtonDown ("Fire2"))
			Debug.Log ("Fire2"); 
	
	}
}
