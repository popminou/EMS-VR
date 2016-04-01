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
	public GameObject mountains;

	public GameObject body_sun;
	public GameObject body_earth;
	public GameObject body_moon;
	public GameObject body_earth_mesh;
	public GameObject body_earth_mesh_scaled;

	public ModelManager myModelManager;
	public Transform directionalLight;
	public Transform directionalLight_model;
	public Transform earthCam;

	public Camera playerView;

	//public GameObject camera;
	public GameObject playerPosition;
	public GameObject groundObservatory;
	public GameObject skyObservatory;


	public Color ground_nightColor;
	public Color ground_dayColor;
	public Color spaceColor;

	public TimeManager timeManager;
	private bool viewingFromGround = true; 

	//	public GameObject PlayerPos;
//	public GameObject OVR_Camera;
//	public GameObject modelPos;
//	public GameObject modelStationaryPos;

	//public VectorLine model_earthOrbitLine;
	//public Texture orbitTexture;
	//public Material orbitMaterial;

	public GameObject playerIndicatorGround;
	public GameObject playerIndicatorSky;

	public bool DEBUG_STATIONARY_MODEL = false;

	public Material skyboxMat;

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
		timeManager = GameObject.FindObjectOfType<TimeManager> ();
	}


	void SetPlayerPosition(GameObject targetPos)
	{
		playerPosition.transform.SetParent (targetPos.transform, false);
		if (viewingFromGround) {
			Debug.Log ("GROUND");
			body_earth_mesh_scaled.GetComponent<Renderer> ().enabled = false;
			mountains.SetActive (true);
		} else {
			Debug.Log("NOTGROUND");
			body_earth_mesh_scaled.GetComponent<Renderer> ().enabled = true;
			mountains.SetActive (false);
		}

		SetPlayerIndicator(viewingFromGround);

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
		//SetCameraColor ();
	}

	void LateUpdate ()
	{
		//UpdateOrbitLines();
		//UpdateCamera();
	}

	public float curColor = 0f;
	void SetBodyPositions()
	{
		ModelManager.BodyPos earthPos = myModelManager.GetEarthPos ();
		ModelManager.BodyPos moonPos = myModelManager.GetMoonPos ();

		// Bodies
		body_earth.transform.localPosition = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.BODY_EARTH_SUN_DISTANCE + body_sun.transform.localPosition;
		body_moon.transform.localPosition = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.BODY_MOON_EARTH_DISTANCE + body_earth.transform.localPosition;
	
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
	
		SetCameraColor (ModelManager.mod(earthPos.rot - 40f, 360f) + ModelManager.mod(earthPos.angle, 360f));

	}


	void SetCameraColor(float earthRot)
	{
		
		float colorLerp = (Mathf.Sin((earthRot) * Mathf.Deg2Rad) + 1)/2f;

		//Debug.Log (colorLerp);
		if (viewingFromGround) {
			
			skyboxMat.SetFloat("_Blend", colorLerp);//rot);
		} else {
			
			skyboxMat.SetFloat("_Blend", 0f);
		}
	}
	

//	void CreateEarthOrbitLine (int _segmentCount, float _sizeFactor)
//	void CreateEarthOrbitLine (int _segmentCount, float _sizeFactor)
//	{
//		//float timeInc = ModelManager.SYNODIC_YEAR / _segmentCount;
//		Vector3[] positions = myModelManager.calculateMultipleEarthPos(0, ModelManager.SYNODIC_YEAR, _segmentCount + 1);
//
//		List<Vector3> orbitLinePts = new List<Vector3>();
//		for(int i = 0; i < _segmentCount + 1; i++)
//		{
//			//Debug.Log("" + i + " : " + positions[i]);
//
//			orbitLinePts.Add(new Vector3(positions[i].x * _sizeFactor, 0, positions[i].y * _sizeFactor));
//			//orbitLinePts.Add(positions[i] * _sizeFactor);
//
//			Debug.Log("" + i + " : " + orbitLinePts[i]);
//		}
//		orbitLinePts.Add(new Vector3(positions[0].x * _sizeFactor, 0, positions[0].y * _sizeFactor));
//		//orbitLinePts.Add(positions[0] * _sizeFactor);
//			
//		model_earthOrbitLine = new VectorLine("model_earthOrbitLine", orbitLinePts, 1f, LineType.Continuous);
//		if(orbitMaterial != null)
//			model_earthOrbitLine.material = orbitMaterial;
//
//		model_earthOrbitLine.drawTransform = model_sun.transform;
//
//
//	}
//		
//
//	void UpdateOrbitLines ()
//	{
//		if(model_earthOrbitLine != null)
//			model_earthOrbitLine.Draw3D();
//
//	}

	public float posFactor = 0.08f;

	private void SetPlayerIndicator (bool _viewFromGround)
	{
		if(playerIndicatorGround != null)
			playerIndicatorGround.SetActive( _viewFromGround );
		if(playerIndicatorSky != null)
			playerIndicatorSky.SetActive( !_viewFromGround );
	}

	/*
	void UpdateCamera ()
	{
		if(camera.activeSelf)
		{
			camera.transform.localPosition = new Vector3(model_earth.transform.localPosition.x, model_earth.transform.localPosition.y + 40, model_earth.transform.localPosition.z);
			camera.transform.LookAt( model_earth.transform);
		}
	}
	*/
}
