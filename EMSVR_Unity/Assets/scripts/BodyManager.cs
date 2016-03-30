using UnityEngine;
using System.Collections;

public class BodyManager : MonoBehaviour {

	public GameObject model_parent;
	public GameObject model_sun;
	public GameObject model_earth;
	public GameObject model_moon;

	public GameObject body_parent;
	public GameObject body_sun;
	public GameObject body_earth;
	public GameObject body_moon;

	public ModelManager myModelManager;
	public Transform directionalLight;
	public Transform earthCam;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		SetBodyPositions ();
	}


	void SetBodyPositions()
	{
		ModelManager.pos earthPos = myModelManager.GetEarthPos ();
		ModelManager.pos moonPos = myModelManager.GetMoonPos ();

		// Bodies
		body_earth.transform.localPosition = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.BODY_EARTH_SUN_DISTANCE + body_sun.transform.localPosition;
		body_moon.transform.localPosition = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.BODY_MOON_EARTH_DISTANCE + body_earth.transform.localPosition;

		// Model
		model_earth.transform.localPosition = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.MODEL_EARTH_SUN_DISTANCE + model_sun.transform.localPosition;
		model_moon.transform.localPosition = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.MODEL_MOON_EARTH_DISTANCE + model_earth.transform.localPosition;
		directionalLight.LookAt (model_earth.transform.position);
		earthCam.LookAt (model_moon.transform.position);
	
	}
}
