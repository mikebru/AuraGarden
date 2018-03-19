using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDestroy : ToolClass {

	private GameObject CurrentObject;

	// Use this for initialization
	void Start () {


	}

	//when the trigger is pressed
	public override void TriggerPress()
	{
		if (CurrentObject != null) {
			Destroy (CurrentObject);
			CurrentObject = null;
			StartCoroutine (Shape_Gen.UpdateNumberofShapes ());
		}

	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.transform.parent != null) {
			if (Other.transform.parent.tag == "SpawnShape") {
				CurrentObject = Other.transform.root.gameObject;
			}
		}
	}

	void OnTriggerExit(Collider Other)
	{
		if (Other.transform.parent != null) {
			if (Other.transform.parent.tag == "SpawnShape") {
				CurrentObject = null;
			}
		}
	}

}
