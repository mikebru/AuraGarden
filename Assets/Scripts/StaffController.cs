using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffController : MonoBehaviour {


	public float EffectRadius;
	public LayerMask EffectedLayer;
	public Transform AttractPoint;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private Color CurrentColor;

	// Use this for initialization
	void Start () {

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

	}
	
	// Update is called once per frame
	void Update () {


		ApplyParticleMotion ();

		if (Controller.velocity.magnitude > .5f) {
			CurrentColor = RotationToColor (Controller.velocity);
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
					hits [i].transform.gameObject.GetComponent<ParticleMovement> ().AttractAngularVelocity (AttractPoint.position, Controller.velocity.magnitude);
				}

				//hits [i].transform.gameObject.GetComponent<ParticleMovement> ().AddVelocity (AttractPoint.forward.normalized , Controller.velocity);
				hits [i].transform.gameObject.GetComponent<TrailRenderer> ().startColor = CurrentColor;
				hits [i].transform.gameObject.GetComponent<TrailRenderer> ().endColor = CurrentColor * .5f;

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
