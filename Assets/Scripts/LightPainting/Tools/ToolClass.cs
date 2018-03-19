using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolClass : MonoBehaviour {

	public TextMesh DisplayText;
	public Text ToolTipDisplay;
	public string ToolTipText;

	public string ToolName;
	public GenerateShapes Shape_Gen { get; set;}

	// Use this for initialization
	void Awake () {
		Shape_Gen = GetComponentInParent<GenerateShapes> ();
		ToolTipDisplay = GameObject.FindGameObjectWithTag("Tips").GetComponent<Text> ();
	}

	void OnEnable()
	{
		DisplayText.text = ToolName;
		ToolTipDisplay.text = ToolTipText;

	}

	void Update()
	{
		
		if (Shape_Gen == null) {
			Shape_Gen = GameObject.FindObjectOfType<GenerateShapes> ();
			return;
		}

	}

	//called when the trigger is pressed
	public virtual void TriggerPress()
	{


	}

	public virtual void TriggerHeld()
	{


	}

	//called when the player is touching the thumb pad
	public virtual void PadTouch(Vector2 PadPosition)
	{


	}


	//called when the trigger is pressed
	public virtual void ChangedTool()
	{


	}

}
