using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAttributes : MonoBehaviour {

	private MeshRenderer[] ChildRenderers;
	private MeshFilter[] ChildFilters;

	private Material CurrentMaterial;

	// Use this for initialization
	void Start () {
		ChildRenderers = GetComponentsInChildren<MeshRenderer> ();
		ChildFilters = GetComponentsInChildren<MeshFilter> ();
	}

	//change all materials for children
	public void ChangeMaterial(Material newMat)
	{
		CurrentMaterial = new Material(newMat);

		for (int i = 0; i < ChildRenderers.Length; i++) {
			ChildRenderers [i].material = CurrentMaterial;
		}
	}

	//change all meshes for children
	public void ChangeMesh(GameObject newObject)
	{
		for (int i = 0; i < ChildFilters.Length; i++) {
			ChildFilters [i].mesh = newObject.GetComponentInChildren<MeshFilter> ().sharedMesh;
		}
	}

	//change all meshes for children
	public void ChangeEffect(bool isAnimating)
	{
		GetComponent<GroupAnimation> ().StopAnimation = !isAnimating;

		if (isAnimating == true) {
			GetComponent<GroupAnimation> ().StartAnimation ();
		}


		if (isAnimating == false) {
			for (int i = 0; i < ChildFilters.Length; i++) {
				ChildFilters [i].transform.parent.gameObject.SetActive (true);
			}
		}
	}

	public void SetColor(string ColorName, Color newColor)
	{
		if (CurrentMaterial != null) {
			CurrentMaterial.SetColor (ColorName, newColor);
		}
	}

	public void SetVertex(Vector2 newDirections)
	{
		if (CurrentMaterial != null) {
			CurrentMaterial.SetFloat ("_Value1", newDirections.x * .3f);
			CurrentMaterial.SetFloat ("_Value4", newDirections.y * .3f);
		}
	}

}
