using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour {

	public UnityEvent OnTouchEvent; 
	private MenuDisplayer DisplayMenu;
	public bool isMenuOption;
	// Use this for initialization
	void Start () {
		DisplayMenu = FindObjectOfType<MenuDisplayer> ();
	}

	void OnTriggerEnter(Collider Other)
	{
		if (isMenuOption == true) {
			if (Other.tag == "Staff" && DisplayMenu.MenuOpen == true) {
				OnTouchEvent.Invoke ();
			}
		} else { 
			if (Other.tag == "Staff") {
				OnTouchEvent.Invoke ();
			}
		}

	}
	

}
