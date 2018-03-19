using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : ToolClass {

	public GameObject SpawnParent;
	//public GameObject ColorDisplay;

	private MenuDisplayer SelectionMenu;
	private string ColorName;

	private Vector2 CurrentPosition;
	public Color CurrentColor {get; set;}

	private bool ShouldAnimate;

	private GroupAttributes Display;

	private ControllerInput InputControl;

	// Use this for initialization
	void Start () {
		//default color name
		ColorName = "_TintColor";
		Display = FindObjectOfType<GroupAttributes> ();

		SelectionMenu = FindObjectOfType<MenuDisplayer> ();
		InputControl = GetComponentInParent<ControllerInput> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Shape_Gen == null) {
			Shape_Gen = GetComponentInParent<GenerateShapes> ();
			return;
		}
	}

	public void SetEffect(bool isAnimating)
	{
		ShouldAnimate = isAnimating;
	}

	public override void TriggerPress()
	{

		//start spawning
		if (Shape_Gen.isSpawning == false) {
			CreateParent ();

			Shape_Gen.isSpawning = true;
			Shape_Gen.ColorName = ColorName;
			SelectionMenu.FastmenuClose ();

			ChangeColor (Color.cyan);

			//make LED pulse fast
			if (InputControl != null) {
				InputControl.isFastPulse = true;
			}
		} 
		// stop spawning
		else if (Shape_Gen.isSpawning == true) {
			StopSpawning ();

			//stop fast pulse
			if (InputControl != null) {
				InputControl.isFastPulse = false;
			}
		}
			
		//clear old shapes
		/*
		if (Shape_Gen.isSpawning == false && Shape_Gen.NumberofShapes > 0) {
			GameObject[] shapes = GameObject.FindGameObjectsWithTag ("SpawnShape");

			for (int i = 0; i < shapes.Length; i++) {
				Destroy (shapes [i]);

			}
			Shape_Gen.NumberofShapes = 0;
		}*/

	}


	void CreateParent()
	{
		Shape_Gen.CurrentParent = Instantiate (SpawnParent, transform.root.position, Quaternion.identity);
	}

	public void ChangeColor(Color newColor)
	{
		GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", newColor);
	}


	//called when the tool is changed
	public override void ChangedTool()
	{
		StopSpawning ();
	}

	public void StopSpawning()
	{
		SelectionMenu.gameObject.SetActive (true);

		Shape_Gen.isSpawning = false;
		//SelectionMenu.SetActive (true);
		ChangeColor (Color.gray);

		if (Shape_Gen.CurrentParent != null) {
			
			if (Shape_Gen.CurrentParent.GetComponent<GroupAnimation> ().AnimationStarted == false && ShouldAnimate == true) {
				Shape_Gen.CurrentParent.GetComponent<GroupAnimation> ().StartAnimation ();
			}

			//when we finish a drawing store all of that information
			StorePaintingData ();
		}
	}

	//passes all of the current information about the painting into a save painting class so that it can be saved later
	void StorePaintingData()
	{
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetParentLocation (Shape_Gen.CurrentParent.transform.position);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetBrush (Shape_Gen.SelectedBrush);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetMaterial (Shape_Gen.SelectedMaterial);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetChildScale (Shape_Gen.isBig);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetAnimation (ShouldAnimate);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetColor (Shape_Gen.CurrentColor);

		Vector3[] ChildPositions = new Vector3[Shape_Gen.CurrentParent.transform.childCount - 1];
		Vector3[] ChildRots = new Vector3[Shape_Gen.CurrentParent.transform.childCount - 1];

		for (int i = 0; i < ChildPositions.Length; i++) {
			//get child i+1 because the beacon is the first child
			ChildPositions[i] = Shape_Gen.CurrentParent.transform.GetChild(i+1).transform.localPosition;
			ChildRots[i] = Shape_Gen.CurrentParent.transform.GetChild(i+1).transform.localRotation.eulerAngles;
		}

		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetChildPositions (ChildPositions);
		Shape_Gen.CurrentParent.GetComponent<SavePainting> ().SetChildRotation (ChildRots);
	}


	public void SetMaterialName(string newName)
	{
		ColorName = newName;

		Shape_Gen.ColorName = newName;
	}

	public override void PadTouch(Vector2 PadPosition)
	{
		CurrentColor = new Color (PadPosition.x, PadPosition.y, .5f, .5f);

		//ColorDisplay.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", CurrentColor);

		if (ColorName == "_TintColor") {
			CurrentColor = CurrentColor * .5f;
		}

		//if it is the vertex shader do something special
		if (ColorName == "_Value1") {
			Shape_Gen.SetVertex (PadPosition);
			Display.SetVertex (PadPosition);
		} else {
			Shape_Gen.SetColor (ColorName, CurrentColor);
			Display.SetColor (ColorName, CurrentColor);
		}
	}

}
