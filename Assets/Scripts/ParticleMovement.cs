using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour {

	public Material[] ParticleMaterials;
	public bool inZone;

	// Use this for initialization
	void Start () {
		
	}

	public void AddVelocity(Vector3 heading, Vector3 newVelocity)
	{
		//Debug.Log (Mathf.Abs (newVelocity.magnitude));
		if (GetComponent<Rigidbody> ().velocity.magnitude < 1f && inZone == true) {
			GetComponent<Rigidbody> ().velocity = heading * newVelocity.magnitude;
		}
	}

	//move to the staff
	public void AttractAngularVelocity(Vector3 newDirection, float speed)
	{
			
			Vector3 newHeading = newDirection - this.transform.position;
			newHeading = newHeading.normalized;

			GetComponent<Rigidbody> ().velocity = newHeading * speed;

	}

	void OnTriggerEnter(Collider otherObject)
	{

		if (otherObject.tag == "Staff") {

			GetComponent<MeshRenderer> ().material = ParticleMaterials [1];
			inZone = true;
		}

	}


	void OnTriggerExit(Collider otherObject)
	{

		if (otherObject.tag == "Staff") {

			GetComponent<MeshRenderer> ().material = ParticleMaterials [0];
			inZone = false;

		}

	}

}
