using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skybox_Transition : MonoBehaviour {

	public Material ReflectionMaterial;
	public GameObject ReflectionObject;
	public Material[] Skyboxes;
	public GameObject[] GroundPlanes;

	public float ChangeDistance;
	private Vector3 StartLocation;
	private int CurrentSkyBox;

	// Use this for initialization
	void Start () {
		StartLocation = transform.position;

		ReflectionMaterial.SetTexture ("_Cube", Skyboxes [CurrentSkyBox].GetTexture("_Tex"));
		ReflectionObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture ("_Cube", Skyboxes [CurrentSkyBox].GetTexture("_Tex"));

	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance (StartLocation, transform.position) > ChangeDistance) {
			StartCoroutine (FadeTransition ());
			Debug.Log ("Sky Switch");
		}


	}


	IEnumerator FadeTransition()
	{
		int lastSkybox;
		lastSkybox = CurrentSkyBox;
		StartLocation = transform.position;

		float t = 0;

		while (t < 2) {
			t += Time.deltaTime;

			Skyboxes [CurrentSkyBox].SetFloat ("_Exposure", (1-(t/2)));

			yield return new WaitForFixedUpdate ();
		}

		if (GroundPlanes [CurrentSkyBox] != null) {
			GroundPlanes [CurrentSkyBox].SetActive (false);
		}

		if (CurrentSkyBox < Skyboxes.Length - 1) {
			CurrentSkyBox += 1;
		} else {
			CurrentSkyBox = 0;
		}

		if (GroundPlanes [CurrentSkyBox] != null) {
			GroundPlanes [CurrentSkyBox].SetActive (true);
		}

		ReflectionObject.GetComponent<MeshRenderer>().material.SetTexture ("_Cube", Skyboxes [CurrentSkyBox].GetTexture("_Tex"));
		ReflectionMaterial.SetTexture ("_Cube", Skyboxes [CurrentSkyBox].GetTexture("_Tex"));

		Skyboxes [CurrentSkyBox].SetFloat ("_Exposure", 0);
		RenderSettings.skybox = Skyboxes [CurrentSkyBox];

		t = 0;

		while (t < 2) {
			t += Time.deltaTime;

			Skyboxes [CurrentSkyBox].SetFloat ("_Exposure", (t/2));

			yield return new WaitForFixedUpdate ();
		}

		Skyboxes [lastSkybox].SetFloat ("_Exposure", 1);


	}
		
}
