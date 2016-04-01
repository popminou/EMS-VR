using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class BodyManager : MonoBehaviour {
	public GameObject model_parent;
	public GameObject model_sun;
	public GameObject model_earth;
	public GameObject model_moon;
	public GameObject model_earth_mesh;


	public GameObject body_sun;
	public GameObject body_earth;
	public GameObject body_moon;
	public GameObject body_earth_mesh;
	public GameObject body_earth_mesh_scaled;

	public ModelManager myModelManager;
	public Transform directionalLight;
	public Transform directionalLight_model;
	public Transform earthCam;

	public GameObject camera;
	public GameObject playerPosition;
	public GameObject groundObservatory;
	public GameObject skyObservatory;
	private bool viewingFromGround = true; 

	//	public GameObject PlayerPos;
//	public GameObject OVR_Camera;
//	public GameObject modelPos;
//	public GameObject modelStationaryPos;

	public VectorLine model_earthOrbitLine;
	//public Texture orbitTexture;
	public Material orbitMaterial;

	public bool DEBUG_STATIONARY_MODEL = false;

	private float lastRot = 0f;
	//public float BODIES_FACTOR = 10f;

	// Use this for initialization
	void Start () {
		//OVR_Camera.transform.SetParent (PlayerPos.transform, true);
		//if(DEBUG_STATIONARY_MODEL == true)
		//	model_parent.transform.SetParent (modelStationaryPos.transform, true);
		//else
		//	model_parent.transform.SetParent (modelPos.transform, true);

		//OVR_Camera.transform.localPosition = Vector3.zero;
		//model_parent.transform.localPosition = Vector3.zero;

		//VectorLine.SetCamera3D(camera);
		//CreateEarthOrbitLine(10, myModelManager.MODEL_EARTH_SUN_DISTANCE);

		SetPlayerPosition (groundObservatory);
	}


	void SetPlayerPosition(GameObject targetPos)
	{
		playerPosition.transform.SetParent (targetPos.transform, false);
		if (viewingFromGround) {
			//Debug.Log ("OFF");
			body_earth_mesh_scaled.GetComponent<Renderer> ().enabled = false;
		} else {
			//Debug.Log("ON");
			body_earth_mesh_scaled.GetComponent<Renderer> ().enabled = true;
		}

	}

	public void ToggleView()
	{
		viewingFromGround = !viewingFromGround;
		if (viewingFromGround) {
			SetPlayerPosition (groundObservatory);
		} else {
			SetPlayerPosition (skyObservatory);
		}

			
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
		float rotDiff = earthPos.rot - lastRot;
		lastRot = earthPos.rot;
		//body_earth_mesh.transform.RotateAround (body_earth.transform.position, Vector3.up, rotDiff);
		body_earth_mesh.transform.localRotation = Quaternion.Euler(0f,earthPos.rot,0f);
		//model_earth_mesh.transform.RotateAround (model_earth_mesh.transform.position, Vector3.up, rotDiff);
		model_earth_mesh.transform.localRotation = Quaternion.Euler(0f,earthPos.rot,0f);

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
			
		model_earthOrbitLine = new VectorLine("model_earthOrbitLine", orbitLinePts, 1f, LineType.Continuous);
		if(orbitMaterial != null)
			model_earthOrbitLine.material = orbitMaterial;

		model_earthOrbitLine.drawTransform = model_sun.transform;


	}

	void UpdateOrbitLines ()
	{
		if(model_earthOrbitLine != null)
			model_earthOrbitLine.Draw3D();

	}
}
