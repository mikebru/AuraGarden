using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousPulse : MonoBehaviour {


	public float FadeTime;

	private Color[] StartColor;

	// Use this for initialization
	void Start () {

		StartColor = new Color[GetComponent<MeshRenderer> ().materials.Length];

		for (int i = 0; i < GetComponent<MeshRenderer> ().materials.Length; i++) {
			StartColor[i] = GetComponent<MeshRenderer> ().materials[i].GetColor ("_Color");
		}


		StartCoroutine (FadeOut ());

	}
	
	IEnumerator FadeInFunction()
	{
		float t = 0;

		//Color EndColor = new Vector4 (StartColor.r, StartColor.g, StartColor.b, 0);

		while (t < FadeTime) {
			t += Time.deltaTime;

			for (int i = 0; i < GetComponent<MeshRenderer> ().materials.Length; i++) {
				GetComponent<MeshRenderer> ().materials[i].SetColor ("_Color", Color.Lerp(Vector4.zero, StartColor[i], t/FadeTime));
			}
				
			yield return new WaitForFixedUpdate ();
		}

		StartCoroutine (FadeOut ());

	}

	IEnumerator FadeOut()
	{
		float t = 0;

		while (t < FadeTime) {
			t += Time.deltaTime;

			for (int i = 0; i < GetComponent<MeshRenderer> ().materials.Length; i++) {
				GetComponent<MeshRenderer> ().materials[i].SetColor ("_TintColor", Color.Lerp(StartColor[i], Vector4.zero, t/FadeTime));
			}
			yield return new WaitForFixedUpdate ();
		}

		StartCoroutine (FadeInFunction ());

	}
}
