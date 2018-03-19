using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDeactivate : MonoBehaviour {

	public GenerateShapes Shape_Gen { get; set;}


	void Awake () {
		Shape_Gen = GameObject.FindObjectOfType<GenerateShapes> ();

	}

	// Use this for initialization
	void Start () {
		//Shape_Gen = GameObject.FindObjectOfType<GenerateShapes> ();

	}

	void Update()
	{
		if (Shape_Gen == null) {
			Shape_Gen = GameObject.FindObjectOfType<GenerateShapes> ();
		} else {
			return;
		}

	}
	
	void OnTriggerEnter(Collider Other)
	{
		if (Other.transform.tag == "Stroke") {
			SetChildrenActivate (true, Other.transform);
		}
	}

	void OnTriggerExit(Collider Other)
	{
		if (Other.transform.tag == "Stroke") {
			SetChildrenActivate (false, Other.transform);
		}
	}

	public void SetChildrenActivate(bool isActive, Transform newParent)
	{
		//turn the parents animation on or off 
		newParent.GetComponent<GroupAnimation> ().enabled = isActive;

		for (int i = 0; i < newParent.childCount; i++) {
			//beacon display
			if (i == 0) {
				newParent.GetChild (i).gameObject.SetActive (!isActive);
			} 
			//all other children
			else {
				newParent.GetChild (i).gameObject.SetActive (isActive);
			}
		}
			

		if (Shape_Gen == null) {
			Shape_Gen = GameObject.FindObjectOfType<GenerateShapes> ();
		} else {
			//recount how many shapes are created
			StartCoroutine (Shape_Gen.UpdateNumberofShapes ());
		}
	}


}
