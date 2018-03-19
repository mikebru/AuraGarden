using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour {

	public MenuOptions Options;
	public int OptionIndex;

	//public UnityEvent OnTouchEvent; 
	private MenuDisplayer DisplayMenu;
	// Use this for initialization
	void Start () {
		DisplayMenu = FindObjectOfType<MenuDisplayer> ();
	}

	void OnTriggerEnter(Collider Other)
	{
		if (Other.tag == "Staff" && DisplayMenu.MenuOpen == true) {
			//OnTouchEvent.Invoke ();

			CallEffects (Other.transform.parent.gameObject);
		}
	}

	//sets the currently used staff with the selected effect 
	void CallEffects(GameObject currentStaff)
	{
		switch (Options) {

		case MenuOptions.Material:
			currentStaff.GetComponent<GenerateShapes> ().SetMaterial (OptionIndex);
			break;

		case MenuOptions.Animation:
			if (OptionIndex == 0) {
				currentStaff.GetComponent<GenerateShapes> ().SetEffect (false);
			} else {
				currentStaff.GetComponent<GenerateShapes> ().SetEffect (true);
			}
			break;

		case MenuOptions.Scale:
			currentStaff.GetComponent<GenerateShapes> ().SetSize (OptionIndex);
			break;

		case MenuOptions.Color:
			if (OptionIndex == 0) {
				currentStaff.GetComponent<GenerateShapes> ().SetColorEffect (false);
			} else {
				currentStaff.GetComponent<GenerateShapes> ().SetColorEffect (true);
			}
			break;

		case MenuOptions.Symmetry:
			if (OptionIndex == 0) {
				currentStaff.GetComponent<GenerateShapes> ().SetSymmetry (false);
			} else {
				currentStaff.GetComponent<GenerateShapes> ().SetSymmetry (true);
			}
			break;

		case MenuOptions.Brushed:
			currentStaff.GetComponent<GenerateShapes> ().SetSpawnShape(OptionIndex);
			break;

		}

	}

}
