using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScultputeViewer : MonoBehaviour {

	public GameObject[] Sculptures;
	public int CurrentIndex;


	// Use this for initialization
	void Start () {

	}

	public void SetArray(int ArraySize)
	{
		Sculptures = new GameObject[ArraySize];
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.RightArrow)) {

			if (CurrentIndex < Sculptures.Length - 1) {

				Sculptures [CurrentIndex].SetActive (false);

				CurrentIndex += 1;
				Sculptures [CurrentIndex].SetActive (true);

			} else {
				Sculptures [CurrentIndex].SetActive (false);

				CurrentIndex = 0;
				Sculptures [CurrentIndex].SetActive (true);

			}
		}


	}
}
