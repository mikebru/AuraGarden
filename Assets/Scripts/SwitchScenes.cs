using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Alpha1)) {

			SceneManager.LoadScene (1);

		}else if(Input.GetKey (KeyCode.Alpha2))
		{
			SceneManager.LoadScene (2);

		}else if(Input.GetKey (KeyCode.Alpha3))
		{
			SceneManager.LoadScene (3);

		}
		else if(Input.GetKey (KeyCode.Alpha0))
		{
			SceneManager.LoadScene (0);

		}
		
	}
}
