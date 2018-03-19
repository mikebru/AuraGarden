using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplayer : MonoBehaviour {

	public GameObject Menu;
	public Transform StartTransform;
	public Transform EndTransform;

	public ApplyHighlight MeshHighlight;
	public ApplyHighlight BrushHighlight;

	public bool MenuOpen;
	// Use this for initialization
	void Start () {
		MenuOpen = false;
	}
		

	public void menuClose()
	{
		if (MenuOpen == true) {
			StartCoroutine (CloseMenu ());
		}
	}

	public void FastmenuClose()
	{
			//StartCoroutine (CloseMenu ());
			MenuOpen = false;
			BrushHighlight.RemoveHightlight ();
			MeshHighlight.RemoveHightlight ();

			Menu.transform.position = StartTransform.position;
			Menu.transform.localScale = StartTransform.localScale;
			Menu.SetActive (false);
			this.gameObject.SetActive (false);

	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Staff") {

			if (MenuOpen == false) {
				StartCoroutine (OpenMenu ());
			} else {
				StartCoroutine (CloseMenu ());
			}
		}
	}


	IEnumerator OpenMenu()
	{
		float t = 0;

		Menu.SetActive (true);

		while (t < 1) {
			t += Time.deltaTime;

			Menu.transform.position = Vector3.Lerp (StartTransform.position, EndTransform.position, t/1);
			Menu.transform.localScale = Vector3.Lerp (StartTransform.localScale, EndTransform.localScale, t/1);

			yield return new WaitForFixedUpdate ();
		}

		BrushHighlight.AddHightlight ();
		MeshHighlight.AddHightlight ();
		MenuOpen = true;

	}


	IEnumerator CloseMenu()
	{
		MenuOpen = false;

		float t = 0;

		BrushHighlight.RemoveHightlight ();
		MeshHighlight.RemoveHightlight ();

		while (t < 1) {
			t += Time.deltaTime;

			Menu.transform.position = Vector3.Lerp (EndTransform.position, StartTransform.position,  t/1);
			Menu.transform.localScale = Vector3.Lerp (EndTransform.localScale, StartTransform.localScale,  t/1);

			yield return new WaitForFixedUpdate ();
		}

		Menu.SetActive (false);

	}

}
