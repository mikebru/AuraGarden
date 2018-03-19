using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyHighlight : MonoBehaviour {

	public Material HighlightMat;
	public int CurrentChild;

	// Use this for initialization
	void Start () {
		
	}


	public void RemoveHightlight()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Material[] Currentmat = new Material[1];
			Currentmat[0] = transform.GetChild (i).GetComponent<MeshRenderer> ().materials [0];
			transform.GetChild (i).GetComponent<MeshRenderer> ().materials = Currentmat;
		}
	}

	public void AddHightlight()
	{
		Material[] newMaterials =  new Material[2];
		newMaterials[0] = transform.GetChild (CurrentChild).GetComponent<MeshRenderer> ().materials [0];
		newMaterials [1] = HighlightMat;

		transform.GetChild (CurrentChild).GetComponent<MeshRenderer> ().materials = newMaterials;
	}

	public void ApplyHighLight(int childcount)
	{
		Material[] Currentmat = new Material[1];
		Currentmat[0] = transform.GetChild (CurrentChild).GetComponent<MeshRenderer> ().materials [0];
		transform.GetChild (CurrentChild).GetComponent<MeshRenderer> ().materials = Currentmat;

		Material[] newMaterials =  new Material[2];
		newMaterials[0] = transform.GetChild (childcount).GetComponent<MeshRenderer> ().materials [0];
		newMaterials [1] = HighlightMat;

		transform.GetChild (childcount).GetComponent<MeshRenderer> ().materials = newMaterials;

		CurrentChild = childcount;
	}

}
