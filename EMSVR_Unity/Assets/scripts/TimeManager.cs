using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float simTime = 0;
	public float timeMultiplier = 1;

	public float MAX_TIME = 500;

	public float SPEED_FACTOR = 1;

	float defaultTimeMult;

	bool timePaused = false; 
	// Use this for initialization
	void Start () {
		defaultTimeMult = timeMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeMultiplier > MAX_TIME)
			timeMultiplier = MAX_TIME;
		if (timeMultiplier < -MAX_TIME)
			timeMultiplier = -MAX_TIME;
		if(timePaused == false)
			simTime += Time.deltaTime * timeMultiplier;
	}

	public float getSimTime()
	{
		return simTime * 10000;
	}

	public void SpeedUpTime(float factor)
	{
		if (timePaused) {
			timeMultiplier = defaultTimeMult;
			timePaused = false; 
		} else {
			timeMultiplier += SPEED_FACTOR * factor;
		}
	}

	public void SlowTime(float factor)
	{
		if (timePaused) {
			timeMultiplier = -defaultTimeMult;
			timePaused = false; 
		} else {
			timeMultiplier -= SPEED_FACTOR * factor;
		}
	}

	public void TogglePauseTime()
	{
		timePaused = !timePaused;
	}
}
