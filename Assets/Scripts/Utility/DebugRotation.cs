using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (transform.rotation.eulerAngles.z);

	}
}
