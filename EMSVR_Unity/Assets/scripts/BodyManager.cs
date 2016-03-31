using UnityEngine;
using System.Collections;

public class BodyManager : MonoBehaviour {
	
	public GameObject model_sun;
	public GameObject model_earth;
	public GameObject model_moon;
	public GameObject body_sun;
	public GameObject body_earth;
	public GameObject body_moon;

	public ModelManager myModelManager;
	public Transform directionalLight;
	public Transform directionalLight_model;
	public Transform earthCam;

	public GameObject PlayerPos;
	public GameObject OVR_Camera;
	public GameObject modelPos;


	public float BODIES_FACTOR = 10f;

	// Use this for initialization
	void Start () {
		OVR_Camera.transform.SetParent (PlayerPos.transform, true);
		model_sun.transform.SetParent (modelPos.transform, true);
		OVR_Camera.transform.localPosition = Vector3.zero;
		model_sun.transform.localPosition = Vector3.zero;
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
		body_earth.transform.position = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * BODIES_FACTOR + body_sun.transform.position;
		body_moon.transform.position = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * BODIES_FACTOR + body_earth.transform.position;

		// Model
		//ModelManager.pos earthPos = myModelManager.GetEarthPos ();
		//ModelManager.pos moonPos = myModelManager.GetMoonPos ();
		model_earth.transform.position = new Vector3 (earthPos.x, earthPos.z, earthPos.y) + model_sun.transform.position;
		model_moon.transform.position = new Vector3 (moonPos.x, moonPos.z, moonPos.y) + model_earth.transform.position;
		directionalLight.LookAt (body_earth.transform.position);
		directionalLight_model.LookAt (model_earth.transform.position);
		earthCam.LookAt (body_moon.transform.position);
	
	}
}
