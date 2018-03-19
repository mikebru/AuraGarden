using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void SetPosition(Transform newPosition)
	{
		transform.position = newPosition.position;
	}

}
