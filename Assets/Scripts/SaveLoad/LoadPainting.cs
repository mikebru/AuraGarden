using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadPainting : MonoBehaviour {

	public GameObject parentObject;
	public GameObject[] BrushObject;
	public Mesh[] BrushMesh;

	public Material[] BrushMaterials;
	public string[] ColorNames;

	public bool LoadforViewing;
	private ScultputeViewer Viewer;

	public PaintingData LocalCopyOfData {get; set;}
	private int index =0;
	private int NumberofFiles; 

	private DistanceDeactivate DeactivateFunction;

	// Use this for initialization
	void Start () {
		DeactivateFunction = FindObjectOfType<DistanceDeactivate> ();

		if (LoadforViewing == true) {
			Viewer = FindObjectOfType<ScultputeViewer> ();
		}

		if (Directory.Exists ("Saves")) {
				ImportAll ();
		}

	}

	void ImportAll()
	{
		NumberofFiles = Directory.GetFiles ("Saves").Length;
		Debug.Log (NumberofFiles);

		if (LoadforViewing == true) {
			Viewer.SetArray (NumberofFiles);
		}

		if (NumberofFiles > 0) {
			LoadData (index);
		}
	}

	public void LoadData(int index)
	{
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Open("Saves/painting" + index.ToString() + ".binary", FileMode.Open);

		LocalCopyOfData = (PaintingData)formatter.Deserialize (saveFile);

		saveFile.Close();

		GeneratePainting (LocalCopyOfData);
	}


	void GeneratePainting(PaintingData newPaintingData)
	{
		GameObject newPainting = Instantiate (parentObject, newPaintingData.ParentLocation, Quaternion.identity);

		//if the painting is intended to be viewed, put it at the origin
		if (LoadforViewing == true) {
			newPainting.transform.position = Vector3.zero;
		}

		Material newMat = new Material (BrushMaterials [newPaintingData.SelectedMaterial]);

		if (ColorNames [newPaintingData.SelectedMaterial] != "") {
			newMat.SetColor (ColorNames [newPaintingData.SelectedMaterial], newPaintingData.MainColor);
		}

		//creates all of the brush strokes for the painting
		for (int i = 0; i < newPaintingData.ChildPositions.Length; i++) {
			//create a brush as a child of the main painting
			GameObject newBrush = Instantiate (BrushObject [newPaintingData.SelectedBrush], newPainting.transform);
			newBrush.transform.localPosition = newPaintingData.ChildPositions [i];
			newBrush.transform.localRotation = Quaternion.Euler(newPaintingData.ChildRotations [i]);
			newBrush.transform.GetChild (0).GetComponent<MeshRenderer> ().material = newMat;

			if (newPaintingData.isBig == true) {
				newBrush.transform.localScale = new Vector3 (6, 6, 6);
			}

		}

		//start the animation
		if (newPaintingData.isAnimating == true) {
			newPainting.GetComponent<GroupAnimation> ().StartAnimation ();
		}

		//if the painting is far away from the starting location, turn it off
		if (Vector3.Distance (Vector3.zero, newPainting.transform.position) > 20) {
			DeactivateFunction.SetChildrenActivate (false, newPainting.transform);
		}

		//fill the viewer array with scultptures
		if (LoadforViewing == true) {
			Viewer.Sculptures [index] = newPainting;

			//turn off all but the starting scultpture
			if (index > 0) {
				newPainting.SetActive (false);
			}
		}
			
		//generate and load the next painting
		if (index < NumberofFiles -1) {
			index++;

			LoadData (index);
		}

	}


}
