using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAnimation : MonoBehaviour {

	public bool AnimationStarted;
	public bool StopAnimation { get; set;}

	// Use this for initialization
	void OnEnable () {
		StopAnimation = false;

		if (AnimationStarted == true) {
			StartAnimation ();
		}


	}

	void OnDisable() {

		StopAnimation = true;
	}

	public void StartAnimation()
	{
		AnimationStarted = true;
		StartCoroutine (GoThrough (false));
	}

	IEnumerator GoThrough(bool isActive)
	{
		yield return new WaitForFixedUpdate ();

		if (StopAnimation == false) {
			
			for (int i = 1; i < transform.childCount; i++) {
				
				transform.GetChild (i).gameObject.SetActive (isActive);

				//do not delay on symmetry pieces 
				if (!transform.GetChild (i).name.Contains ("Symmetry")) {
					yield return new WaitForSeconds (transform.GetChild (i).gameObject.GetComponent<ShapeAttributes> ().DisplayTime);
				}

				if (StopAnimation == true) {
					break;
				}

			}
			
			if (isActive == true) {
				StartCoroutine (GoThrough (false));
			} else {
				StartCoroutine (GoThrough (true));
			}
		}

	}

}
