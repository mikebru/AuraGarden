using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedStaff : MonoBehaviour {

	private GameObject[] Staffs;
	private MeshRenderer Main_Mesh;

	// Use this for initialization
	void Start () {

		Main_Mesh = GetComponentInChildren<MeshRenderer> ();
		Staffs = GameObject.FindGameObjectsWithTag("Staff");
		
	}

	void Update()
	{
		//search for two players, don't do anytrhing until then
		if (Staffs.Length < 2) {
			Staffs = GameObject.FindGameObjectsWithTag("Staff");
		}
			
		SetPostion ();
		SetRotation ();
		SetColor ();

	}


	void SetPostion()
	{
		Vector3 PlayerPositions = Vector3.zero;

		//takes the average postion of the two players 
		for (int i = 0; i < Staffs.Length; i++) {
			PlayerPositions += Staffs [i].transform.position;
		}

		PlayerPositions = PlayerPositions / Staffs.Length;

		transform.position = PlayerPositions;
	}


	void SetRotation()
	{
		Vector3 PlayerRotations = new Vector3(0,0,0);

		//takes the average postion of the two players 
		for (int i = 0; i < Staffs.Length; i++) {
			PlayerRotations += Staffs [i].transform.rotation.eulerAngles;
		}

		PlayerRotations = PlayerRotations/Staffs.Length;

		transform.rotation = Quaternion.Euler(new Vector3(PlayerRotations.x,PlayerRotations.y,PlayerRotations.z + 90));
	}

	void SetColor()
	{
		Main_Mesh.material.SetColor ("_EmissionColor", new Color(transform.forward.x, transform.forward.y, transform.forward.z, 1));
	}
		

}
