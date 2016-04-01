using UnityEngine;
using System.Collections;

public class ModelManager : MonoBehaviour {


	public TimeManager myTimeManager;
	// Use this for initialization
	void Start () {
	
	}

	BodyPos moonPos;
	BodyPos earthPos;
	BodyPos sunPos;


	public BodyPos GetEarthPos(){ return earthPos;}
	public BodyPos GetMoonPos(){ return moonPos;}
	public BodyPos GetSunPos(){ return sunPos;}


	public float earthAngle = 0f;
	public float earthRot = 0f;

	public struct BodyPos
	{
		public float x;
		public float y;
		public float z;
		public float rot;
		public float angle;
		public float z_angle;
		public float planeOffset;
		public void set(float _x, float _y, float _z,float _rot, float _angle, float _z_angle, float _planeOffset)
		{
			x = _x;
			y = _y;
			z = _z;
			rot = _rot;
			angle = _angle;
			z_angle = _z_angle;
			planeOffset = _planeOffset;
		}
	};

	static public float mod(float number, float limit)
	{
		while(number < 0)
		{
			number += limit;
		}

		return Mathf.Repeat (number, limit);

	}

	Vector2 getOffestFromAngleAndDistance(float degreeAngle, float distance)
	{
		float radAngle = degreeAngle * Mathf.PI / 180;
		float xOffset = distance * Mathf.Cos(radAngle);
		float yOffset = distance * Mathf.Sin(radAngle);

		return new Vector2(xOffset,yOffset);
	}
		
	public void init()
	{
		sunPos = new BodyPos ();
		sunPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);
		earthPos = new BodyPos ();
		earthPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);
		moonPos = new BodyPos ();
		moonPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);

		reset();
	}

	public void reset()
	{
		sunPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);
		earthPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);
		moonPos.set (0f, 0f, 0f, 0f, 0f, 0f, 0f);
		updateModelPositions();
	} 

	public void Update()
	{
		updateModel(); 
	}

	public void updateModel()
	{
		updateModelPositions();

		earthAngle = earthPos.angle;
		earthRot = earthPos.rot;

	}

	public void updateModelPositions()
	{
		//get simTime from timeManager, calculate positions	
		float currentDateInSeconds  =  myTimeManager.getSimTime();

		//calculateSunPos(currentDateInSeconds);
		calculateEarthPos(currentDateInSeconds);
		calculateMoonPos(currentDateInSeconds);
	}



	float calculateRotation(float time, float rotationRate)
	{
		return 360f * ((time * rotationRate) % 1);
	}


	void calculateEarthPos(float time)
	{
		float angle = mod(-(EARTH_STARTING_ANGLE + (360 * ((EARTH_ORBIT_RATE * time) % 1))), 360f);
		Vector2 BodyPos = getOffestFromAngleAndDistance(angle, 1/*EARTH_SUN_DISTANCE*/);

		earthPos.x = BodyPos.x;
		earthPos.y = BodyPos.y;
		earthPos.z = 0f;
		earthPos.rot =  calculateRotation(time, EARTH_ROTATION_RATE);
		earthPos.angle = angle;

	}

	int MAX_MULTIPLE_POSITIONS = 10000;

	public Vector3[] calculateMultipleEarthPos (float startTime, float endTime, int numPoints)
	{
		float deltaTime = endTime - startTime;
		float timeInc = deltaTime / numPoints;
		Vector3[] positions = new Vector3[MAX_MULTIPLE_POSITIONS];
		Vector3 newPosition = new Vector3();

		for(var i = 0; i < numPoints; i++)
		{
			calculateEarthPos(startTime + timeInc*i);
			//BodyPos = { x: computedEarthPosition.x, y: computedEarthPosition.y, z: computedEarthPosition.z, rot: computedEarthPosition.rot, angle: computedEarthPosition.angle };
			newPosition.x = earthPos.x;
			newPosition.y = earthPos.y;
			newPosition.z = earthPos.z;

			positions[i] = newPosition;
		}

		return positions;



//		 var calculateMultipleEarthPos = function (startTime, endTime, numPoints, useKeplerEquation)
//		{
//			var deltaTime = endTime - startTime;
//			var timeInc = deltaTime / numPoints;
//			var positions = [];
//
//			for(var i = 0; i < numPoints; i++)
//			{
//				calculateEarthPos(startTime + timeInc*i, useKeplerEquation);
//				var BodyPos = { x: computedEarthPosition.x, y: computedEarthPosition.y, z: computedEarthPosition.z, rot: computedEarthPosition.rot, angle: computedEarthPosition.angle };
//				positions.push(BodyPos);
//			}
//
//			return positions;
//		}
	
	}



	void calculateMoonPos(float time)
	{
		float baseAngle = -(MOON_STARTING_ANGLE + (360 * ((MOON_SIDEREAL_ORBIT_RATE * time) % 1)));
		float angleRotated = (STARTING_ORBITAL_ROTATION_OFFSET + (360* ((ORBITAL_ROTATION_OFFSET * time) % 1)));
		float angle = mod((baseAngle + angleRotated), 360f);
		moonPos.angle = angle;
		float radAngle = angle * Mathf.PI / 180;


		//Angle goes from -30 to +30 based on X value
		float orbitalTiltAngle = angle + ORBITAL_ANGLE_OFFSET;
		float radOrbitalTiltAngle = orbitalTiltAngle * Mathf.PI / 180;


		//Height of moon is sin of degree 		
		float flatOrbitWidth =  /*EARTH_MOON_DISTANCE*/1 * Mathf.Cos(MOON_ORBITAL_TILT  * Mathf.PI / 180);
		float xOffset = flatOrbitWidth * Mathf.Cos(radAngle);
		float yOffset = (/*EARTH_MOON_DISTANCE*/1) * Mathf.Sin(radAngle);
		float zAngle = MOON_ORBITAL_TILT * Mathf.Cos(radOrbitalTiltAngle);
		float radZAngle = zAngle * Mathf.PI / 180; 
		float zOffset = -(/*EARTH_MOON_DISTANCE*/1 * Mathf.Sin(radZAngle));	


			//Utils.log("At time " + time + ", z offset is " + zOffset);
		
		moonPos.x = /*earthPos.x +*/ xOffset;
		moonPos.y = /*earthPos.y +*/ yOffset;
		moonPos.z = /*earthPos.z +*/ zOffset;
		moonPos.z_angle = zAngle;
		moonPos.rot = calculateRotation(time, MOON_ROTATION_RATE);


	}

	public float GetCurrentDayFactor()
	{
		Debug.Log("Day time: " + (myTimeManager.getSimTime () % SYNODIC_DAY));
		return (myTimeManager.getSimTime () % SYNODIC_DAY) / SYNODIC_DAY;
	}


	static float SECOND = 1f;
	static float MINUTE = 60f * SECOND;
	static float HOUR = 60f * MINUTE; 
	static float DAY = 24f * HOUR;
	static float WEEK = 7f * DAY;
	static float MONTH = 4f * WEEK;

	//ACCURATE MEASUREMENTS
	static float SYNODIC_MONTH = 29f * DAY + 12f * HOUR + 44f * MINUTE + 2.8016f * SECOND;
	static float LUNAR_MONTH = SYNODIC_MONTH; //In case its easier to remember :P
	public static float SYNODIC_YEAR = 365f * DAY + 5f * HOUR + 48f * MINUTE + 46f * SECOND;
	static float SOLAR_DAY = 24.47f * HOUR;

	static float SYNODIC_DAY = 23f * HOUR + 56f * MINUTE + 4 * SECOND;

	static float SIDEREAL_MONTH = 2347126f + 9845f; 

	static float CALENDAR_YEAR = 2025f;

	static float START_DATE = 0f;//f; - Date of the first Lunar eclipse (useful when modifying values in order to align things)

	static float WORLD_SIZE_X = 40000f; //400f;
	static float WORLD_SIZE_Y = 40000f; //400f;
	static float WORLD_SIZE_Z = 40000f; //400f;

	static float SUN_ROTATION_RATE = 0f;//1/timeConfig.SOLAR_DAYf;  //Made this up...I assume it will just be for gfx of sun spinning (if any)
	public float EARTH_ROTATION_RATE = 1/DAY; //This may need to be changed to the super-accurate day
	public float MOON_ROTATION_RATE = 1/SYNODIC_MONTH; //Look it up! Its the same...huh

	public float EARTH_ORBIT_RATE = 1/SYNODIC_YEAR; //may need to modify this a bit!
	public float MOON_ORBIT_RATE = 1/(SYNODIC_MONTH);// * 1.01)f; //1/timeConfig.SYNODIC_MONTHf;

	float MOON_SIDEREAL_ORBIT_RATE = 1/SIDEREAL_MONTH;

	public float EARTH_SUN_DISTANCE = 149f *.5f;//14900.0000f;  //generic units. We cant show the realistic distance without losing perspective on earth/moon
	public float EARTH_MOON_DISTANCE = 38f * .25f;//38.4400f; 

	public float MOON_ORBITAL_TILT = 5f; //DEGREE of tilt of lunar orbit
	//static float MOON_ORBITAL_MAX_HEIGHT = Mathf.Abs(EARTH_MOON_DISTANCE * Mathf.Sin(Mathf.PI / 180 * MOON_ORBITAL_TILT));
	static float ORBITAL_ANGLE_OFFSET = 158.5f;//166f; //341.4   //Without this offset, z = 0 when x = 0, which is NOT the case

	static float EARTH_STARTING_ANGLE = 0f;

	static float MOON_STARTING_ANGLE = 0f;//171.5f;//211.5f;

	static float STARTING_ORBITAL_ROTATION_OFFSET = 16.5f;
	static float ORBITAL_ROTATION_OFFSET =0f;// 1/timeConfig.SYNODIC_YEARf; //1/timeConfig.SYNODIC_YEAR + 1/(timeConfig.SYNODIC_YEAR * 30)f; //amount that the orbital plane rotates


	public float BODY_EARTH_SUN_DISTANCE = 400f;
	public float BODY_MOON_EARTH_DISTANCE = 80f;

	public float MODEL_EARTH_SUN_DISTANCE = 10f;
	public float MODEL_MOON_EARTH_DISTANCE = 1f;


}
