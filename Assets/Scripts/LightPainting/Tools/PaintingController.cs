using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingController : ToolClass {

	public GameObject ColorDisplay;

	public Color CurrentColor {get; set;}

	private string ColorName;

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Staff") {
			//other.GetComponent<MeshRenderer> ().sharedMaterial.SetColor (ColorName, CurrentColor);
			other.transform.parent.GetComponent<GenerateShapes> ().SetColor (ColorName, CurrentColor);
		}
	}

	public override void TriggerPress()
	{

	}
		
	public void SetMaterialName(string newName)
	{
		ColorName = newName;
	}

	public override void PadTouch(Vector2 PadPosition)
	{
		CurrentColor = new Color (PadPosition.x, PadPosition.y, .5f, .5f);

		ColorDisplay.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", CurrentColor);

		if (ColorName == "_TintColor") {
			CurrentColor = CurrentColor * .75f;
		}

		Shape_Gen.SetColor (ColorName, CurrentColor);

	}

}
