using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShape : MonoBehaviour {


	public GameObject ShapeToSpawn;
	public Transform SpawnPoint;
	public float SpawnRate;

	public Color SlowColor;
	public Color FastColor;

	private MovementControl M_Control;

	private Color CurrentColor;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	// Use this for initialization
	void Start () {

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		M_Control = this.GetComponent<MovementControl> ();

		StartCoroutine (SpawnTimer ());

	}

	void Update()
	{

		this.GetComponentInChildren<MeshRenderer> ().material.SetColor ("_EmissionColor", CurrentColor);
		//GetComponent<AudioSource> ().pitch = Mathf.Lerp (1f, 1.5f, Controller.velocity.magnitude/5);
	}


	IEnumerator SpawnTimer()
	{

		yield return new WaitForSeconds (SpawnRate);

		if (Controller.velocity.magnitude > .1f) {
			GameObject SpawnedShape = Instantiate (ShapeToSpawn, SpawnPoint.position, SpawnPoint.rotation);

			//Get the color by getting the current forward direction
			CurrentColor = RotationToColor (SpawnedShape.transform.forward);

			//Defines the color based on the current velocity
			SpawnedShape.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", CurrentColor);
		}

		StartCoroutine (SpawnTimer ());

	}


	Color RotationToColor(Vector3 CurrentVector)
	{


		float R = CurrentVector.x * Controller.velocity.x/10;
		float G = CurrentVector.y * Controller.velocity.y/10;
		float B = CurrentVector.z * Controller.velocity.z/10;

		Color DirectionColor = new Color (R, G, B, 1);


		return DirectionColor;

	}

}
