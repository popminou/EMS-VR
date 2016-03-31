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

	public VectorLine model_earthOrbitLine;


	//public float BODIES_FACTOR = 10f;

	// Use this for initialization
	void Start () {
		OVR_Camera.transform.SetParent (PlayerPos.transform, true);
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
		UpdateVectorLines();
	}

	void SetBodyPositions()
	{
		ModelManager.BodyPos earthPos = myModelManager.GetEarthPos ();
		ModelManager.BodyPos moonPos = myModelManager.GetMoonPos ();

		// Bodies
		body_earth.transform.position = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.BODY_EARTH_SUN_DISTANCE + body_sun.transform.position;
		body_moon.transform.position = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.BODY_MOON_EARTH_DISTANCE + body_earth.transform.position;

		// Model
		//ModelManager.pos earthPos = myModelManager.GetEarthPos ();
		//ModelManager.pos moonPos = myModelManager.GetMoonPos ();

		model_earth.transform.position = new Vector3 (earthPos.x, earthPos.z, earthPos.y) * myModelManager.MODEL_EARTH_SUN_DISTANCE + model_sun.transform.position;
		model_moon.transform.position = new Vector3 (moonPos.x, moonPos.z, moonPos.y) * myModelManager.MODEL_MOON_EARTH_DISTANCE + model_earth.transform.position;
		directionalLight.LookAt (body_earth.transform.position);
		directionalLight_model.LookAt (model_earth.transform.position);
		earthCam.LookAt (body_moon.transform.position);
	
	}


	void CreateEarthOrbitLine (int _segmentCount, float _sizeFactor)
	{
		float timeInc = ModelManager.SYNODIC_YEAR / _segmentCount;
		Vector3[] positions = myModelManager.calculateMultipleEarthPos(0, ModelManager.SYNODIC_YEAR, _segmentCount + 1);

		List<Vector3> orbitLinePts = new List<Vector3>();
		for(int i = 0; i < _segmentCount + 1; i++)
		{
			//Debug.Log("" + i + " : " + positions[i]);

			orbitLinePts.Add(positions[i] * _sizeFactor);
		}
		orbitLinePts.Add(positions[0] * _sizeFactor);
			
		model_earthOrbitLine = new VectorLine("model_earthOrbitLine", orbitLinePts, 10, LineType.Continuous);
		model_earthOrbitLine.color = Color.red;



		//model_earthOrbitLine = new VectorLine(
		/*	
		var segmentCount = numSegments;
		var geometry = new THREE.Geometry();
		var material_base = new THREE.LineBasicMaterial({ color: color, linewidth: 0.5 });
		var material_alpha = new THREE.LineBasicMaterial({ color: color, linewidth: 2.0 });
		material_alpha.transparent = true;
		material_alpha.opacity = 0.5;
		material_alpha.blending = THREE.NormalBlending;

		var timeInc = ;
		var positions = modelManager.calculateMultipleEarthPos(0, timeConfig.SYNODIC_YEAR, numSegments + 1, simConfig.USE_KEPLER_MODEL, false);

		for(var i = 0; i < positions.length; i++)
		{
			geometry.vertices.push(
		        new THREE.Vector3(
		            positions[i].x * factor,
		            sceneConfig.EARTH_ORBIT_OFFSET_Y,
		            positions[i].y * factor
		            )); 

			//Utils.log("Earth orbit pos: " + (positions[i].x * factor) + ";" + (positions[i].y * factor));
		}
		// complete the circle
		geometry.vertices.push(
	        new THREE.Vector3(
	            positions[0].x * factor,
	            sceneConfig.EARTH_ORBIT_OFFSET_Y,
	            positions[0].y * factor
	            ));

		var line_base = new THREE.Line(geometry, material_base);
		var line_alpha = new THREE.Line(geometry, material_alpha);

    	var orbitObject = new THREE.Object3D();
    	orbitObject.add(line_alpha);
    	orbitObject.add(line_base);

		//return new THREE.Line(geometry, material);
		return orbitObject;
		*/
	}

	void UpdateVectorLines ()
	{
		if(model_earthOrbitLine != null)
			model_earthOrbitLine.Draw3D();
	}
}
