using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StaffControllerNetwork : MonoBehaviour {


	public float EffectRadius;
	public LayerMask EffectedLayer;
	public Transform AttractPoint;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private Color CurrentColor;

	private GameObject Player;
	private bool isReady = false;

	// Use this for initialization
	void Start () {

		Player = transform.root.gameObject;

		if (Player.GetComponent<NetworkIdentity> ().isLocalPlayer == true) {
			Debug.Log ("Initializing");
			StartCoroutine (WaitToInitialize ());
		}

	}

	IEnumerator WaitToInitialize()
	{
		yield return new WaitForSeconds (1);


		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		isReady = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (isReady == true) {

			ApplyParticleMotion ();

			if (Controller.velocity.magnitude > 1f) {
				CurrentColor = RotationToColor (Controller.velocity);
			}
		}
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


				if (Controller.velocity.magnitude > 1f) {
					//set authority of this object to the current player
					Player.GetComponent<VRPawnParticle> ().CmdSetAuth (hits [i].transform.gameObject.GetComponent<NetworkIdentity> ().netId, Player.GetComponent<NetworkIdentity> ());

					hits [i].transform.gameObject.GetComponent<ParticleMovementNetwork> ().AttractAngularVelocity (AttractPoint.position, Controller.velocity.magnitude);
				}

				//hits [i].transform.gameObject.GetComponent<ParticleMovement> ().AddVelocity (AttractPoint.forward.normalized , Controller.velocity);
				//hits [i].transform.gameObject.GetComponent<TrailRenderer> ().startColor = CurrentColor;
			}

		}

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
