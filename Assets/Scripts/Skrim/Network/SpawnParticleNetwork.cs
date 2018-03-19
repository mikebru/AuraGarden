using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnParticleNetwork : NetworkBehaviour {

	public GameObject ShapeToSpawn;
	public Transform SpawnPoint;
	public float SpawnRate;

	private MovementControl M_Control;

	[SyncVar]
	private Color CurrentColor;


	// Use this for initialization
	void Start () {

		M_Control = this.GetComponent<MovementControl> ();

		StartCoroutine (SpawnTimer ());

	}

	void Update()
	{

		this.GetComponentInChildren<MeshRenderer> ().material.SetColor ("_TintColor", CurrentColor);
		//GetComponent<AudioSource> ().pitch = Mathf.Lerp (1f, 1.5f, Controller.velocity.magnitude/5);
	}


	IEnumerator SpawnTimer()
	{

		yield return new WaitForSeconds (SpawnRate);

		if (GetComponent<Rigidbody>().velocity.magnitude > .1f) {
			GameObject SpawnedShape = Instantiate (ShapeToSpawn, SpawnPoint.position, SpawnPoint.rotation);

			//Get the color by getting the current forward direction
			CurrentColor = RotationToColor (SpawnedShape.transform.forward);

			//Defines the color based on the current velocity
			SpawnedShape.GetComponent<MeshRenderer> ().material.SetColor ("_TintColor", CurrentColor);

			NetworkServer.Spawn (SpawnedShape);
		}

		StartCoroutine (SpawnTimer ());

	}


	Color RotationToColor(Vector3 CurrentVector)
	{


		float R = CurrentVector.x;
		float G = CurrentVector.y;
		float B = CurrentVector.z;

		Color DirectionColor = new Color (R, G, B, 1);


		return DirectionColor;

	}

}
