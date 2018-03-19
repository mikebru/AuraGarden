using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour {

	public Material matToChange;

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{

		if (other.transform.parent.gameObject.GetComponent<PaintingController> () != null) {
			UpdateColor (other.transform.parent.gameObject.GetComponent<PaintingController> ().CurrentColor);
		}

	}

	public void UpdateColor(Color newColor)
	{

		matToChange.SetColor ("_Color", newColor);

	}

}
