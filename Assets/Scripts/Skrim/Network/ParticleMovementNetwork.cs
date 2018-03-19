using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParticleMovementNetwork : NetworkBehaviour {

	public Material[] ParticleMaterials;
	public bool inZone;

	[SyncVar (hook = "SyncColor")]
	public Color CurrentColor;

	// Use this for initialization
	void Start () {

		//GetComponent<NetworkIdentity> ().AssignClientAuthority (GetComponent<NetworkIdentity> ().clientAuthorityOwner);

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

		//update the trail color 
		UpdateColor (newHeading, speed);

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

	void UpdateColor(Vector3 newHeading, float speed)
	{
		CurrentColor = new Color(newHeading.x * speed/5, newHeading.y* speed/5, newHeading.z* speed/5, 1);

		//sync color across network
		CmdSyncColor (CurrentColor);

		//change color locally
		if (GetComponent<TrailRenderer> () != null) {
			GetComponent<TrailRenderer> ().startColor = CurrentColor;
			GetComponent<TrailRenderer> ().endColor = CurrentColor * .2f;
		} else {
			GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", CurrentColor);
		}
	}

	[Command]
	void CmdSyncColor(Color value)
	{
		if (GetComponent<TrailRenderer> () != null) {
			GetComponent<TrailRenderer> ().startColor = value;
			GetComponent<TrailRenderer> ().endColor = value * .2f;
		} else {
			GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value);
		}

	}

	//gets called by hook
	void SyncColor(Color value)
	{
		if (GetComponent<TrailRenderer> () != null) {
			GetComponent<TrailRenderer> ().startColor = value;
			GetComponent<TrailRenderer> ().endColor = value * .2f;
		} else {
			GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value);
		}
	}




}
