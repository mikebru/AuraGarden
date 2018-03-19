using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedParticleController : MonoBehaviour {

	public GameObject[] Staffs;
	private MeshRenderer Main_Mesh;

	public float EffectRadius;
	public LayerMask EffectedLayer;
	public Transform AttractPoint;

	public float CurrentVelocity { get; set;}

	private Vector3 lastPosition;

	// Use this for initialization
	void Start () {

		Main_Mesh = GetComponentInChildren<MeshRenderer> ();
		Staffs = GameObject.FindGameObjectsWithTag("Staff");

	}

	void FixedUpdate()
	{
		//search for two players, don't do anytrhing until then
		if (Staffs.Length < 2) {
			Staffs = GameObject.FindGameObjectsWithTag("Staff");
		}

		SetPostion ();
		SetVelocity ();
		ApplyParticleMotion ();

	}
		
	void SetPostion()
	{
		lastPosition = transform.position;
		Vector3 PlayerPositions = Vector3.zero;

		//takes the average postion of the two players 
		for (int i = 0; i < Staffs.Length; i++) {
			PlayerPositions += Staffs [i].transform.position;
		}

		PlayerPositions = PlayerPositions / Staffs.Length;

		transform.position = PlayerPositions;
	}


	void SetVelocity()
	{
		CurrentVelocity = Vector3.Distance (lastPosition, transform.position) / Time.fixedDeltaTime;
	}


	void ApplyParticleMotion()
	{
		//scaled area of effect based on velocity
		//float areaOfEffect = Mathf.Lerp (EffectRadius * 2, EffectRadius * 4, Controller.velocity.magnitude/4);

		RaycastHit[] hits;

		hits = Physics.SphereCastAll (transform.position, EffectRadius, transform.forward, 20f, EffectedLayer);

		//Debug.Log (Controller.angularVelocity.magnitude);

		//if we actually hit something
		if (hits.Length > 0) {
			for (int i = 0; i < hits.Length; i++) {
				if (CurrentVelocity > 1f) {
					hits [i].transform.gameObject.GetComponent<ParticleMovementNetwork> ().AttractAngularVelocity (AttractPoint.position, CurrentVelocity);
				}

			}

		}

	}
		

}
