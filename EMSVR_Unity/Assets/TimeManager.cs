using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float simTime = 0;
	public float timeMultiplier = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		simTime += Time.deltaTime * timeMultiplier;
	}

	public float getSimTime()
	{
		return simTime * 10000;
	}
}
