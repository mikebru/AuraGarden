using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnShapeNetwork : NetworkBehaviour {

	public float SpawnSpeed;

	public GameObject ShapeToSpawn;
	public Transform SpawnPoint;
	public float SpawnRate;

	private MovementControl M_Control;

	private Color CurrentColor;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private MeshRenderer Main_Mesh;

	// Use this for initialization
	void Start () {

		M_Control = this.GetComponent<MovementControl> ();


		if (GetComponent<MeshRenderer> () == null) {
			Main_Mesh = GetComponentInChildren<MeshRenderer> ();
		} else {
			Main_Mesh = GetComponent<MeshRenderer> ();
		}

		StartCoroutine (SpawnTimer ());

	}



	IEnumerator SpawnTimer()
	{

		yield return new WaitForSeconds (SpawnRate);

		if (GetComponent<Rigidbody>().velocity.magnitude >= SpawnSpeed) {
			GameObject SpawnedShape = Instantiate (ShapeToSpawn, SpawnPoint.position, SpawnPoint.rotation);

			//Get the color
			CurrentColor = Main_Mesh.material.GetColor("_EmissionColor");

			//Defines the color based on the current velocity
			SpawnedShape.GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", CurrentColor);

			NetworkServer.Spawn (SpawnedShape);
		}

		StartCoroutine (SpawnTimer ());

	}
		

}
