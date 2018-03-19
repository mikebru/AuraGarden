using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTool : ToolClass {


	public GameObject LocationDisplay;
	public LayerMask CastLayer;

	private Vector3 TeleportLocation;

	// Use this for initialization
	void Start () {
		LocationDisplay = GameObject.FindGameObjectWithTag ("Teleport");
	}

	void OnEnable()
	{
		if (LocationDisplay == null) {
			LocationDisplay = GameObject.FindGameObjectWithTag ("Teleport");
		}
		
		ToolTipDisplay.text = ToolTipText;
		DisplayText.text = ToolName;
		LocationDisplay.SetActive (true);
	}

	void OnDisable()
	{
		//LocationDisplay.SetActive (false);
	}

	void FixedUpdate()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, 100, CastLayer)) {
			TeleportLocation = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
			LocationDisplay.transform.position = TeleportLocation;
		} else {
			TeleportLocation = Vector3.zero;
		}
			
	}
		
	//called when the trigger is pressed
	public override void TriggerPress()
	{
		if (TeleportLocation != Vector3.zero) {
			StartCoroutine (LerpMovement (TeleportLocation));
			//transform.root.position = TeleportLocation;
		}
	}


	IEnumerator LerpMovement(Vector3 NewLocation)
	{
		float t = 0;

		Vector3 StartPosition = transform.root.position;

		while (t < 3) {
			t += Time.deltaTime;

			transform.root.position = Vector3.Lerp (StartPosition, NewLocation, t / 3);
			yield return new WaitForFixedUpdate ();
		}

	}

	//change the scaling and placement distance of the spawned objects 
	/*
	public override void PadTouch(Vector2 PadPosition)
	{
		Shape_Gen.SizeScale = Mathf.Lerp (1, 10, PadPosition.x);

		Shape_Gen.DistanceScale = Mathf.Lerp (1, 10, PadPosition.y);

	}*/
	

}
