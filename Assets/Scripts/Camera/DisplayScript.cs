using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		StartCoroutine (WaitToStart ());
	}

	//for some reason there needs to be a slight delay in order for the monitor to display this camera
	IEnumerator WaitToStart()
	{
		yield return new WaitForSeconds (.1f);

		//turn the viewer camera on 
		this.gameObject.GetComponent<Camera> ().enabled = true;
	}


}
