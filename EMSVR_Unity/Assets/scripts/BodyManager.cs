using UnityEngine;
using System.Collections;

public class BodyManager : MonoBehaviour {

	public GameObject body_sun;
	public GameObject body_earth;
	public GameObject body_moon;
	public GameObject model_sun;
	public GameObject model_earth;
	public GameObject model_moon;

	public ModelManager myModelManager;
	public Transform directionalLight;
	public Transform earthCam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SetBodyPositions ();
	}


	void SetBodyPositions()
	{
		ModelManager.pos moonPos = myModelManager.GetMoonPos ();
		ModelManager.pos earthPos = myModelManager.GetEarthPos ();
		body_earth.transform.position = new Vector3 (earthPos.x, earthPos.z, earthPos.y);
		body_moon.transform.position = new Vector3 (moonPos.x, moonPos.z, moonPos.y);
		directionalLight.LookAt (body_earth.transform.position);
		earthCam.LookAt (body_moon.transform.position);
	
	}
}
