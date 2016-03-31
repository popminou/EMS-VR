using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

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

	public GameObject camera;
	public GameObject PlayerPos;
	public GameObject OVR_Camera;
	public GameObject modelPos;
	public GameObject modelStationaryPos;

	public VectorLine model_earthOrbitLine;

	public bool DEBUG_STATIONARY_MODEL = true;


	//public float BODIES_FACTOR = 10f;

	// Use this for initialization
	void Start () {
		OVR_Camera.transform.SetParent (PlayerPos.transform, true);
		if(DEBUG_STATIONARY_MODEL == true)
			model_sun.transform.SetParent (modelStationaryPos.transform, true);
		else
			model_sun.transform.SetParent (modelPos.transform, true);
		OVR_Camera.transform.localPosition = Vector3.zero;
		model_sun.transform.localPosition = Vector3.zero;

		VectorLine.SetCamera3D(camera);
		CreateEarthOrbitLine(10, myModelManager.MODEL_EARTH_SUN_DISTANCE);
	}

	// Update is called once per frame
	void Update ()
	{
		SetBodyPositions ();
	}

	void LateUpdate ()
	{
		UpdateOrbitLines();
	}

	void SetBodyPositions()
	{
		ModelManager.BodyPos earthPos = myModelManager.GetEarthPos ();
		ModelManager.BodyPos moonPos = myModelManager.GetMoonPos ();

		// Bodies
		body_earth.transform.localPosition = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.BODY_EARTH_SUN_DISTANCE + body_sun.transform.localPosition;
		body_moon.transform.localPosition = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.BODY_MOON_EARTH_DISTANCE + body_earth.transform.localPosition;


		// Model
		model_earth.transform.localPosition = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.MODEL_EARTH_SUN_DISTANCE + model_sun.transform.localPosition;
		model_moon.transform.localPosition = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.MODEL_MOON_EARTH_DISTANCE + model_earth.transform.localPosition;
		directionalLight.LookAt (body_earth.transform.position);
		directionalLight_model.LookAt (model_earth.transform.position);
		earthCam.LookAt (body_moon.transform.position);
	
	}


	void CreateEarthOrbitLine (int _segmentCount, float _sizeFactor)
	{
		//float timeInc = ModelManager.SYNODIC_YEAR / _segmentCount;
		Vector3[] positions = myModelManager.calculateMultipleEarthPos(0, ModelManager.SYNODIC_YEAR, _segmentCount + 1);

		List<Vector3> orbitLinePts = new List<Vector3>();
		for(int i = 0; i < _segmentCount + 1; i++)
		{
			//Debug.Log("" + i + " : " + positions[i]);
			
			orbitLinePts.Add(new Vector3(positions[i].x * _sizeFactor, 0, positions[i].y * _sizeFactor));
			//orbitLinePts.Add(positions[i] * _sizeFactor);

			Debug.Log("" + i + " : " + orbitLinePts[i]);
		}
		orbitLinePts.Add(new Vector3(positions[0].x * _sizeFactor, 0, positions[0].y * _sizeFactor));
		//orbitLinePts.Add(positions[0] * _sizeFactor);
			
		model_earthOrbitLine = new VectorLine("model_earthOrbitLine", orbitLinePts, 0.005f, LineType.Continuous);
		model_earthOrbitLine.color = Color.red;

		//model_earthOrbitLine.gameObject.transform.SetParent(model_sun.transform, false);
		//model_earthOrbitLine.drawTransform = model_sun.transform;
		//model_earthOrbitLine.gameObject.transform.localScale = new Vector3(model_sun.transform.localScale.x, model_sun.transform.localScale.y, model_sun.transform.localScale.z);


	}

	void UpdateOrbitLines ()
	{
		//List<Vector3> modelEarthOrbitPoints = model_earthOrbitLine.points3;
		Matrix4x4 earthOrbitMatrix = new Matrix4x4();
		earthOrbitMatrix.SetTRS(model_sun.transform.position, Quaternion.identity, new Vector3(1f, 1f, 1f));
		model_earthOrbitLine.matrix = earthOrbitMatrix;
			
		if(model_earthOrbitLine != null)
			model_earthOrbitLine.Draw3D();

	}
}
