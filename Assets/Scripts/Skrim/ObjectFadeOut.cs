using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFadeOut : MonoBehaviour {

	public float FadeTime;

	public bool FadeIn;

	Color StartColor;

	// Use this for initialization
	void Start () {

		StartColor = GetComponent<MeshRenderer> ().material.GetColor ("_EmissionColor");

		if (FadeIn == true) {
			StartCoroutine (FadeInFunction ());
		} else {
			StartCoroutine (FadeOut ());
		}

	}


	IEnumerator FadeInFunction()
	{
		float t = 0;

		//Color EndColor = new Vector4 (StartColor.r, StartColor.g, StartColor.b, 0);

		while (t < FadeTime) {
			t += Time.deltaTime;

			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.Lerp(Vector4.zero, StartColor, t/FadeTime));

			yield return new WaitForFixedUpdate ();
		}

		StartCoroutine (FadeOut ());

		Destroy (this.gameObject);

	}

	IEnumerator FadeOut()
	{
		float t = 0;

		while (t < FadeTime) {
			t += Time.deltaTime;

			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.Lerp(StartColor, Vector4.zero, t/FadeTime));

			yield return new WaitForFixedUpdate ();
		}

		StartCoroutine (FadeInFunction ());
		//Destroy (this.gameObject);

	}


	IEnumerator WaitToDestroy()
	{
		yield return new WaitForSeconds (FadeTime);


		Destroy (this.gameObject);

	}
	

}
